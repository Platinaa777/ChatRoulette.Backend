using Chat.Application.Commands.CloseRoom;
using Chat.Domain.Aggregates.Room;
using Chat.Domain.Errors;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MassTransit.Client.EventBus;
using Moq;

namespace Application.Tests;

public class CloseRoomCommandTests
{
    [Fact]
    public async Task Handle_WhenZeroRoomsExists_ReturnsFailure()
    {
        // Arrange
        var roomRepositoryMock = new Mock<IRoomRepository>();
        roomRepositoryMock.Setup(repo => repo.GetAllRooms())
            .ReturnsAsync(new List<TwoSeatsRoom>());
        var chatUserRepositoryMock = new Mock<IChatUserRepository>();
        var busClientMock = new Mock<IEventBusClient>();
        
        
        var handler = new CloseRoomCommandHandler(roomRepositoryMock.Object, chatUserRepositoryMock.Object, busClientMock.Object);
        var command = new CloseRoomCommand() {ConnectionId = "12345"};

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RoomErrors.RoomNotFoundByParticipant, result.Error);
    }

    [Fact]
    public async Task Handle_WhenRoomNotFoundByParticipant_ReturnsFailure()
    {
        // Arrange
        List<TwoSeatsRoom> rooms = new List<TwoSeatsRoom>()
        {
            new TwoSeatsRoom(Guid.NewGuid().ToString(), new List<UserLink>()
            {
                new UserLink("test1@mail.ru", "12345"),
                new UserLink("test2@mail.ru", "123456"),
            }, DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(15)))
        };
        
        var roomRepositoryMock = new Mock<IRoomRepository>();
        roomRepositoryMock.Setup(repo => repo.GetAllRooms())
            .ReturnsAsync(rooms);
        var chatUserRepositoryMock = new Mock<IChatUserRepository>();
        var busClientMock = new Mock<IEventBusClient>();
        
        
        var handler = new CloseRoomCommandHandler(roomRepositoryMock.Object, chatUserRepositoryMock.Object, busClientMock.Object);
        var command = new CloseRoomCommand() {ConnectionId = "not-found"};

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RoomErrors.RoomNotFoundByParticipant, result.Error);
    }
    
    [Fact]
    public async Task Handle_WhenNotFullRoomClosed_ReturnsFailure()
    {
        // Arrange
        string roomId = Guid.NewGuid().ToString();
        var room1 = new TwoSeatsRoom(roomId, new List<UserLink>()
        {
            new UserLink("test1@mail.ru", "12345"),
        }, DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(15)));
        
        List<TwoSeatsRoom> rooms = new List<TwoSeatsRoom>() { room1 };
        
        var roomRepositoryMock = new Mock<IRoomRepository>();
        roomRepositoryMock.Setup(repo => repo.GetAllRooms())
            .ReturnsAsync(rooms);
        roomRepositoryMock.Setup(x => x.FindRoomById(roomId))
            .ReturnsAsync(room1);
        roomRepositoryMock.Setup(x => x.CloseRoom(roomId))
            .ReturnsAsync(true);
        
        var chatUserRepositoryMock = new Mock<IChatUserRepository>();
        var busClientMock = new Mock<IEventBusClient>();
        
        
        var handler = new CloseRoomCommandHandler(roomRepositoryMock.Object, chatUserRepositoryMock.Object, busClientMock.Object);
        var command = new CloseRoomCommand() {ConnectionId = "12345"};

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RoomErrors.NotFullRoom, result.Error);
    }
}