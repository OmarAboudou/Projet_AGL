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
            string[] ipAddresses = IP.GetLocalAddresses();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            using UdpClient server = new(SERVER_PORT);
            while (!cancellationToken.IsCancellationRequested)
            {
                UdpReceiveResult result = await server.ReceiveAsync(cancellationToken);
                string response = result.Buffer.GetStringFromUtf8();
                if (response == DISCOVERY_MESSAGE)
                {
                    foreach (string ipAddress in ipAddresses)
                    {
                        Dictionary responseDictionary = new()
                        {
                            { "ip", ipAddress },
                            { "port", port },
                        };
                        byte[] responseData = Json.Stringify(responseDictionary).ToUtf8Buffer();
                        await server.SendAsync(responseData, result.RemoteEndPoint, cancellationToken);
                    }
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