using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class CustomNetworkManager : NetworkManager
{
    [Header("Player Prefabs")]
    [SerializeField] private PlayerLobby lobbyPlayerPrefab;
    [SerializeField] private PlayerGameManager gamePlayerPrefab;

    [HideInInspector]
    public List<PlayerLobby> lobbyPlayers = new List<PlayerLobby>();
    [HideInInspector]
    public List<PlayerGameManager> gamePlayers = new List<PlayerGameManager>();
    private List<Tuple<NetworkConnectionToClient, int>> connectionsToReplace = new List<Tuple<NetworkConnectionToClient, int>>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
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
        ServerChangeScene("Test");
    }

    public override void ServerChangeScene(string newSceneName)
    {
        connectionsToReplace.Clear();
        foreach (var player in lobbyPlayers)
        {
            connectionsToReplace.Add(new Tuple<NetworkConnectionToClient, int>(player.connectionToClient, player.playerID));
        }
        base.ServerChangeScene(newSceneName);
    }

    public override async void OnServerSceneChanged(string sceneName)
    {
        if (sceneName == "Test")
        {

            foreach (var (conn, id) in connectionsToReplace)
            {
                var gamePlayerInstance = Instantiate(gamePlayerPrefab);
                gamePlayerInstance.name = "Player_" + id;
                gamePlayerInstance.playerID = id;
                gamePlayers.Add(gamePlayerInstance);

                // waiting for client to be ready works for now
                await WaitForClientReady(conn);

                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject, ReplacePlayerOptions.KeepAuthority);
                Debug.Log($"Replaced player for connection: {conn.connectionId}, New player instance: {gamePlayerInstance.name}");
            }
            connectionsToReplace.Clear();
            foreach (var player in lobbyPlayers)
            {
                Destroy(player.gameObject);
            }
            lobbyPlayers.Clear();
        }
    }

    public async Task WaitForClientReady(NetworkConnection conn)
    {
        while (!conn.isReady)
        {
            await Task.Yield();
        }
    }
}
