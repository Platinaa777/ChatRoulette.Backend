using Chat.GrpcClient.Models;

namespace Chat.GrpcClient;

public interface IChatGrpcClient
{
    Task<List<RecentPeerInfoGrpc>> GetRecentPeers(string email);
}