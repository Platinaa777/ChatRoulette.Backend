using Chat.Api.WebSockets;
using Quartz;

namespace Chat.Api.BackgroundJobs;

[DisallowConcurrentExecution]
public class DefineWinnerJob : IJob
{
    private readonly ChatHub _chatHub;

    public DefineWinnerJob(ChatHub chatHub)
    {
        _chatHub = chatHub;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        // Retrieve job data
        string roomId = context.JobDetail.JobDataMap.GetString("roomId")!;
        string roundId = context.JobDetail.JobDataMap.GetString("roundId")!;

        await _chatHub.DefineWinner(roomId, roundId);
    }
}