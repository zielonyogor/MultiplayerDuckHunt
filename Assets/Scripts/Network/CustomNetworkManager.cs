using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CustomNetworkManager : NetworkManager
{
    [Header("Player Prefabs")]
    [SerializeField] private PlayerLobby lobbyPlayerPrefab;
    [SerializeField] private PlayerGameManager gamePlayerPrefab;

    [HideInInspector]
    public List<PlayerLobby> lobbyPlayers = new List<PlayerLobby>();
    [HideInInspector]
    public List<PlayerGameManager> gamePlayers = new List<PlayerGameManager>();

    private List<NetworkConnectionToClient> connectionsToReplace = new List<NetworkConnectionToClient>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("hellooo");
        PlayerLobby player = Instantiate(lobbyPlayerPrefab);
        player.playerID = numPlayers;
        player.gameObject.name = "LobbyPlayer_" + player.playerID;
        lobbyPlayers.Add(player);

        NetworkServer.AddPlayerForConnection(conn, player.gameObject);
    }

    [Server]
    public void StartGame()
    {
        // add ServerChangeScene() to 'if' later
        if (numPlayers == 2)
        { }
        foreach (var player in lobbyPlayers)
        {
            if (!player.connectionToClient.isReady)
                player.connectionToClient.Send(new Mirror.ReadyMessage());
        }
        ServerChangeScene("Test");
    }

    public override void ServerChangeScene(string newSceneName)
    {
        connectionsToReplace.Clear();
        foreach (var player in lobbyPlayers)
        {
            connectionsToReplace.Add(player.connectionToClient);
        }
        base.ServerChangeScene(newSceneName);
    }

    public override async void OnServerSceneChanged(string sceneName)
    {
        if (sceneName == "Test")
        {

            foreach (var conn in connectionsToReplace)
            {
                var gamePlayerInstance = Instantiate(gamePlayerPrefab);
                gamePlayerInstance.name = "Player_" + conn.connectionId;
                // waiting for client to be ready works for now
                await WaitForClientReady(conn);

                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject, ReplacePlayerOptions.KeepAuthority);
                Debug.Log($"Replaced player for connection: {conn.connectionId}, New player instance: {gamePlayerInstance.name}");
            }

            connectionsToReplace.Clear();
            lobbyPlayers.Clear();
        }
    }

    public async Task WaitForClientReady(NetworkConnection conn)
    {
        while (!conn.isReady)
        {
            await Task.Yield();
        }
        Debug.Log("Client is now ready!");
    }
}
