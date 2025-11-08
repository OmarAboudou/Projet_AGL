using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using Project_AGL.Shared.Server_Discovery;

namespace Project_AGL.Server.Server_Discovery;

using static ServerConstants;

public partial class ServerDiscoveryResponder : Node
{
    private CancellationTokenSource _cancellationTokenSource = new();

    public override void _Ready()
    {
        base._Ready();
        StartRespondingDiscoveryRequests(new ServerInfo("localhost", SERVER_PORT));
    }

    public async Task StartRespondingDiscoveryRequests(ServerInfo serverInfo)
    {
        try
        {
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            UdpClient server = new(SERVER_PORT);
            Dictionary responseDictionary = new()
            {
                { "ip", serverInfo.ipAddress },
                { "port", serverInfo.port },
            };
            while (!cancellationToken.IsCancellationRequested)
            {
                UdpReceiveResult result = await server.ReceiveAsync(cancellationToken);
                string response = result.Buffer.GetStringFromUtf8();
                if (response == DISCOVERY_MESSAGE)
                {
                    GD.Print("Server received discovery request");
                    byte[] responseData = Json.Stringify(responseDictionary).ToUtf8Buffer();
                    GD.Print($"Server send discovery response {serverInfo.ipAddress}:{serverInfo.port}");
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