using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class LobbyContainer : MonoBehaviour
{
    private CustomNetworkManager networkManager;
    public Button startGameButton;
    [SerializeField] Button startOrJoinButton;
    [SerializeField] TMP_InputField addressInputField;

    private void Start()
    {
        networkManager = NetworkManager.singleton as CustomNetworkManager; ;
        startGameButton.onClick.AddListener(StartGame);
        addressInputField.onEndEdit.AddListener(delegate
        {
            ChangeButtonFunctionality(addressInputField.text);
        });

        startOrJoinButton.onClick.AddListener(StartServer);
    }

    public void StartGame()
    {
        if (NetworkServer.active)
        {
            networkManager.StartGame();
        }
    }

    public void StartServer()
    {
        networkManager.StartHost();
        Debug.Log(networkManager.networkAddress);
    }

    public void JoinGame()
    {
        string ipAddress = addressInputField.text;
        Debug.Log("joining...");
        networkManager.StartClient();
    }

    public void ChangeButtonFunctionality(string inputText)
    {
        Debug.Log(inputText);
        if (inputText == "")
        {
            startOrJoinButton.onClick.RemoveListener(JoinGame);
            startOrJoinButton.onClick.AddListener(StartServer);
            startOrJoinButton.GetComponentInChildren<TextMeshProUGUI>().text = "create server";
        }
        else
        {
            startOrJoinButton.onClick.RemoveListener(StartServer);
            startOrJoinButton.onClick.AddListener(JoinGame);
            startOrJoinButton.GetComponentInChildren<TextMeshProUGUI>().text = "join server";
        }
    }
}
