using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using Project_AGL.Shared.Server_Discovery;

namespace Project_AGL.Client.Server_Discovery;

using static ServerConstants;

public partial class ServerDiscoveryRequester : Node
{
    [Signal]
    public delegate void ServerDiscoveredEventHandler(string ipAddress, int port);
    
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public override void _Ready()
    {
        base._Ready();
        this.SearchServer();
    }

    public async Task SearchServer()
    {
        try
        {
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            UdpClient client = new();
            client.EnableBroadcast = true;
            IPEndPoint endPoint = new(IPAddress.Broadcast, SERVER_PORT);
            byte[] data = Encoding.ASCII.GetBytes(DISCOVERY_MESSAGE);
            GD.Print("Client send discovery request");
            await client.SendAsync(data, endPoint, cancellationToken);
            await this.ListenForResponses(cancellationToken, client);
        }
        catch (OperationCanceledException)
        {
            GD.Print("Server discovery stopped");
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
        }
    }

    private async Task ListenForResponses(CancellationToken cancellationToken, UdpClient client)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            UdpReceiveResult result = await client.ReceiveAsync(cancellationToken);
            byte[] response = result.Buffer;
            string responseString = response.GetStringFromUtf8();
            
            Dictionary responseDictionary = Json.ParseString(responseString).Obj as Dictionary;
            
            if (responseDictionary != null && responseDictionary.ContainsKey("ip") &&
                responseDictionary.ContainsKey("port"))
            {
                GD.Print($"Client received discovery response {Json.Stringify(responseDictionary)}");
                this.EmitSignalServerDiscovered(responseDictionary["ip"].AsString(),
                    responseDictionary["port"].AsInt32());
            }
        }
    }

    private void StopSearchingServer()
    {
        this._cancellationTokenSource.CancelAsync();
    }
}
