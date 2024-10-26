using UnityEngine;
using Mirror;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] CustomNetworkManager networkManager;
    public void StartGame(){
        if(NetworkServer.active){
            networkManager.StartGame();
        }
    }
}
