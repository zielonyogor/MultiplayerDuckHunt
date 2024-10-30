using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    [Header("Player Prefabs")]
    [SerializeField] GameObject hostPrefab;
    [SerializeField] GameObject clientPrefab;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player;
        if (numPlayers == 0)
        {
            player = Instantiate(hostPrefab);
        }
        else
        {
            player = Instantiate(clientPrefab);
        }

        NetworkServer.AddPlayerForConnection(conn, player);
    }

    [Server]
    public void StartGame()
    {
        ServerChangeScene("Test");
    }
}
