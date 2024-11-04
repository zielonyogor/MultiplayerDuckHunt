using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerLobby : NetworkBehaviour
{
    public int playerID;
    private CustomNetworkManager lobby;
    private CustomNetworkManager Lobby
    {
        get
        {
            if (lobby != null) { return lobby; }
            return lobby = NetworkManager.singleton as CustomNetworkManager;
        }
    }

    public override void OnStartClient()
    {
        //Lobby.lobbyPlayers.Add(this);
        if (!isServer)
        {
            Debug.Log("this is only a client, shouldn't have start game button");
        }
    }

    public override void OnStopClient()
    {
        //Lobby.lobbyPlayers.Remove(this);
    }

    public override void OnStartServer()
    {
        if (isLocalPlayer)
        {
            Debug.Log("this is host, should have message about ip address for joining");
        }
    }

}
