using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using Project_AGL.Shared.Server_Discovery;

namespace Project_AGL.Server.Server_Discovery;

using static ServerConstants;

/// <summary>
/// The role of this class is to listen for server requests
/// </summary>
public partial class ServerDiscoveryResponder : Node
{
    private CancellationTokenSource _cancellationTokenSource = new();

    public async Task StartRespondingDiscoveryRequests(string ipAddress, int port)
    {
        try
        {
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            UdpClient server = new(SERVER_PORT);
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
        catch (Exception e)
        {
            GD.PrintErr(e);
        }
        
    }

    public void StopRespondingDiscoveryRequests()
    {
        this._cancellationTokenSource.CancelAsync();
    }
}