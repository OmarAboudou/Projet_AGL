using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace Server;

using static ServerConstants;

public static class ServerDiscoveryResponder
{
    private static CancellationTokenSource _cancellationTokenSource = new();

    public static async Task StartRespondingDiscoveryRequests(int port)
    {
        try
        {
            string ipAddress = IP.GetLocalAddresses()[0];
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            using UdpClient server = new(SERVER_PORT);
            Dictionary responseDictionary = new()
            {
                { "ip", ipAddress },
                { "port", port },
            };
            while (!cancellationToken.IsCancellationRequested)
            {
                UdpReceiveResult result = await server.ReceiveAsync(cancellationToken);
                string response = result.Buffer.GetStringFromUtf8();
                if (response == DISCOVERY_MESSAGE)
                {
                    byte[] responseData = Json.Stringify(responseDictionary).ToUtf8Buffer();
                    await server.SendAsync(responseData, result.RemoteEndPoint, cancellationToken);
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            StopRespondingDiscoveryRequests();
        }
    }

    public static void StopRespondingDiscoveryRequests()
    {
        _cancellationTokenSource.CancelAsync();
    }
}