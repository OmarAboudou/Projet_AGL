using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace Server;

using static ServerConstants;

/// <summary>
/// The role of this class is to send discovery requests and raise a signal when a server is discovered
/// </summary>
public static class ServerDiscoveryRequester
{
    public delegate void ServerDiscovered(string ip, int port);
    public static event ServerDiscovered OnServerDiscovered;
    
    private static readonly CancellationTokenSource _cancellationTokenSource = new();
    
    public static async void SearchServer()
    {
        try
        {
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            using UdpClient client = new();
            client.EnableBroadcast = true;
            IPEndPoint endPoint = new(IPAddress.Broadcast, SERVER_PORT);
            byte[] data = Encoding.ASCII.GetBytes(DISCOVERY_MESSAGE);
            GD.Print("Client send discovery request");
            await client.SendAsync(data, endPoint, cancellationToken);
            await ListenForResponses(cancellationToken, client);
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

    private static async Task ListenForResponses(CancellationToken cancellationToken, UdpClient client)
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
                OnServerDiscovered?.Invoke(
                    responseDictionary["ip"].AsString(),
                    responseDictionary["port"].AsInt32()
                );
            }
        }
    }

    public static void StopSearchingServer()
    {
        _cancellationTokenSource.CancelAsync();
    }
}
