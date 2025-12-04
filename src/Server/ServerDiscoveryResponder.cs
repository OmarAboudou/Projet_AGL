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
                    GD.Print("Server received discovery request");
                    byte[] responseData = Json.Stringify(responseDictionary).ToUtf8Buffer();
                    GD.Print($"Server send discovery response {ipAddress}:{port}");
                    await server.SendAsync(responseData, result.RemoteEndPoint, cancellationToken);
                }
            }
        }
        catch (OperationCanceledException)
        {
            GD.Print("Server response stopped");
        }
        /*catch (Exception e)
        {
            GD.PrintErr(e);
        }*/
        
    }

    public static void StopRespondingDiscoveryRequests()
    {
        _cancellationTokenSource.CancelAsync();
    }
}