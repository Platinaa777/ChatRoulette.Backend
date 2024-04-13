using Chat.Api.BackgroundJobs;
using Chat.Application.Commands.ChooseAnswer;
using Chat.Application.Commands.CloseRoom;
using Chat.Application.Commands.ConnectUser;
using Chat.Application.Commands.StartRound;
using Chat.Application.Queries.GetRoom;
using Chat.Application.Queries.GetWinner;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using Quartz.Impl;

namespace Chat.Api.WebSockets;

public class ChatHub : Hub
{
    private readonly IMediator _mediator;

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override Task OnConnectedAsync()
    {
        Console.WriteLine("Connection Id: " + Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public async Task GetId()
    {
        await Clients.Client(Context.ConnectionId).SendAsync("id", Context.ConnectionId);
    }
    
    // for client side (for messaging)
    public async Task SendMessageInRoom(string message, string roomId, string email)
    {
        await Clients.Groups(roomId).SendAsync("onReceiveMessage", 
            $"{email}: {message}");
    }

    public async Task FindRoom(string connectionId, string email)
    {
        var findRoomCommand = new ConnectUserCommand() { ConnectionId = connectionId, Email = email };

        var result = await _mediator.Send(findRoomCommand);
        
        if (result.IsFailure)
            return;
        
        // add client to special group
        await Groups.AddToGroupAsync(Context.ConnectionId, result.Value.RoomId);
        // Console.WriteLine($"Client {Context.ConnectionId} was joined in room {response?.RoomId}");
        if (result.Value.CreateOffer)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("PeerConnection",
                result.Value.RoomId,
                "",
                "offer");
        }
        else
        {
            await Clients.Client(Context.ConnectionId).SendAsync("PeerConnection",
                result.Value.RoomId,
                "", // offer || answer
                ""); // command
        }
    }

    public async Task OnPeerOffer(string roomId, string offer)
    {
        var room = await _mediator.Send(new GetRoomQuery() { RoomId = roomId });
        
        if (room is null)
            return;

        foreach (var peerLink in room.PeerLinks)
        {
            if (peerLink.ConnectionId != Context.ConnectionId)
            {
                await Clients.Client(peerLink.ConnectionId).SendAsync("PeerConnection",
                    roomId,
                    offer,
                    "answer");
                return;
            }
        }
    }
    
    public async Task OnPeerAnswer(string roomId, string answer)
    {
        var room = await _mediator.Send(new GetRoomQuery() { RoomId = roomId });
        
        if (room is null)
            return;
        
        foreach (var peer in room.PeerLinks)
        {
            if (peer.ConnectionId != Context.ConnectionId)
            {
                await Clients.Client(peer.ConnectionId).SendAsync("PeerConnection",
                    roomId,
                    answer,
                    "confirmAnswer");
                return;
            }
        }
    }
    
    public async Task OnStartRelayIce(string roomId)
    {
        await Clients.Groups(roomId).SendAsync("PeerConnection",
            roomId,
            "",
            "relay-ice");
    }
    
    public async Task OnIceCandidate(string roomId, string candidates)
    {
        var room = await _mediator.Send(new GetRoomQuery() { RoomId = roomId });
        
        if (room is null)
            return;
        
        foreach (var peer in room.PeerLinks)
        {
            if (peer.ConnectionId != Context.ConnectionId)
            {
                // Console.WriteLine($"From {Context.ConnectionId} IceCandidateTo: {peer}");
                await Clients.Client(peer.ConnectionId).SendAsync("PeerConnection",
                    roomId,
                    candidates,
                    "candidate");
                return;
            }
        }
    }

    public async Task OnNextRoom()
    {
        await SwitchRoom("next-room");
    }
    
    public async Task OnLeaveRoom()
    {
        await SwitchRoom("leave-room");
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await SwitchRoom("leave-room");
    }

    private async Task SwitchRoom(string command)
    {
        var result = await _mediator.Send(new CloseRoomCommand()
        {
            ConnectionId = Context.ConnectionId
        });
        
        if (result.IsFailure)
            return;
        
        // send to peers in group to stop their video tracks
        await Clients.Groups(result.Value.RoomId).SendAsync("PeerConnection",
            result.Value.RoomId,
            "",
            command);
        
        if (result.Value.FirstUser is not null)
        {
            await Groups.RemoveFromGroupAsync(
                result.Value.FirstUser.ConnectionId, 
                result.Value.RoomId);    
        }
        
        if (result.Value.SecondUser is not null)
        {
            await Groups.RemoveFromGroupAsync(
                result.Value.SecondUser.ConnectionId,
                result.Value.RoomId);    
        }
    }

    public async Task StartGame(string roomId)
    {
        var room = await _mediator.Send(new GetRoomQuery()
        {
            RoomId = roomId
        });
        
        if (room is null || !room.IsFullRoom())
            return;
        
        var result = await _mediator.Send(new StartRoundCommand()
        {
            FirstEmail = room.PeerLinks[0].Email,
            SecondEmail = room.PeerLinks[1].Email
        });
        
        if (result.IsFailure)
            return;

        await Clients.Group(roomId).SendAsync(
            "onStartGame", 
            result.Value.Word, 
            result.Value.ListTranslates,
            result.Value.RoundId);

        ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
        IScheduler scheduler = await schedulerFactory.GetScheduler();

        // Start the scheduler
        await scheduler.Start();

        // Define the job
        IJobDetail job = JobBuilder.Create<DefineWinnerJob>()
            .UsingJobData("roomId", roomId)
            .UsingJobData("roundId", result.Value.RoundId)
            .Build();

        // Define the trigger to run after 10 seconds
        ITrigger trigger = TriggerBuilder.Create()
            .StartAt(DateTime.UtcNow.AddSeconds(10))
            .Build();

        // Schedule the job with the trigger
        await scheduler.ScheduleJob(job, trigger);
    }

    public async Task SelectItemInGame(string roomId, string email, string selectedWord, string roundId)
    {
        var result = await _mediator.Send(new ChooseAnswerCommand()
        {
            PlayerEmail = email,
            RoundId = roundId,
            Answer = selectedWord
        });
    }

    public async Task DefineWinner(string roomId, string roundId)
    {
        var winnerResult = await _mediator.Send(new DefineWinnerQuery()
        {
            RoundId = roundId
        });
        
        if (winnerResult.IsFailure)
            return;

        var room = await _mediator.Send(new GetRoomQuery() { RoomId = roomId });
        
        if (room is null || !room.IsFullRoom())
            return;

        await Clients.Groups(roomId).SendAsync("onWinRound", winnerResult);
    } 
}