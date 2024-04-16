using Chat.Grpc;
using Chat.GrpcClient.Configuration;
using Chat.GrpcClient.Models;
using Grpc.Net.Client;

namespace Chat.GrpcClient;

public class ChatGrpcClient : IChatGrpcClient
{
    private readonly ChatApiConnectionString _apiConnectionString;

    public ChatGrpcClient(
        ChatApiConnectionString apiConnectionString)
    {
        _apiConnectionString = apiConnectionString;
    }
    
    public async Task<List<RecentPeerInfoGrpc>> GetRecentPeers(string email)
    {
        using var channel = GrpcChannel.ForAddress($"http://{_apiConnectionString.Host}:{_apiConnectionString.Port}");

        var client = new ChatApiGrpc.ChatApiGrpcClient(channel);

        var response = await client.GetRecentUserPeersAsync(new GetRecentUserPeersRequest() { Email = email });
        
        return response.RecentPeerEmails.Select(recentPeerEmail => new RecentPeerInfoGrpc()
        {
            Email = recentPeerEmail
        }).ToList();
    }
}