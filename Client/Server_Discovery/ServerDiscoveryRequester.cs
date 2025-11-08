using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Godot;

namespace Project_AGL.Client.Server_Discovery;

public partial class ServerDiscoveryRequester : Node
{
    [Signal]
    public delegate void ServerDiscoveredEventHandler(string serverAddress, string serverMessage);

    private Thread _listenThread;
    private bool _searching;
    private UdpClient? _udpClient;

    private const int BroadcastPort = 49000;
    private const int ListenPort = 49001;
    private const string DiscoveryMessage = "DISCOVER_SERVER_REQUEST";

    public void SearchServer()
    {
        if (_searching)
            return;

        _searching = true;

        // On envoie un broadcast sur tout le LAN
        _udpClient = new UdpClient();
        _udpClient.EnableBroadcast = true;

        byte[] data = Encoding.UTF8.GetBytes(DiscoveryMessage);
        IPEndPoint broadcastEP = new IPEndPoint(IPAddress.Broadcast, BroadcastPort);

        GD.Print($"[ServerDiscoveryRequester] Broadcasting discovery message on port {BroadcastPort}...");

        _udpClient.Send(data, data.Length, broadcastEP);

        // Lancement d’un thread pour écouter les réponses
        _listenThread = new Thread(ListenForResponses);
        _listenThread.IsBackground = true;
        _listenThread.Start();
    }

    public void StopSearchingServer()
    {
        _searching = false;

        try
        {
            _udpClient?.Close();
        }
        catch (Exception e)
        {
            GD.PrintErr($"[ServerDiscoveryRequester] Error stopping search: {e.Message}");
        }

        if (_listenThread != null && _listenThread.IsAlive)
            _listenThread.Join();
    }

    private void ListenForResponses()
    {
        try
        {
            using var listener = new UdpClient(ListenPort);
            listener.Client.ReceiveTimeout = 3000; // 3s de timeout

            GD.Print($"[ServerDiscoveryRequester] Listening for responses on port {ListenPort}...");

            while (_searching)
            {
                try
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = listener.Receive(ref remoteEP);
                    string message = Encoding.UTF8.GetString(data);

                    GD.Print($"[ServerDiscoveryRequester] Response from {remoteEP.Address}: {message}");

                    // Emet le signal vers le thread principal Godot
                    CallDeferred(SignalName.ServerDiscovered, remoteEP.Address.ToString(), message);
                }
                catch (SocketException)
                {
                    // timeout, on continue la boucle
                }
            }
        }
        catch (Exception e)
        {
            GD.PrintErr($"[ServerDiscoveryRequester] Listening error: {e.Message}");
        }
    }

    public override void _ExitTree()
    {
        StopSearchingServer();
    }
}
