using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using System;

public class LobbyContainer : MonoBehaviour
{
    private CustomNetworkManager networkManager;
    public Button startGameButton;
    [SerializeField] Button startServerButton;
    [SerializeField] Button joinServerButton;
    [SerializeField] TMP_InputField addressInputField;

    private void Start()
    {
        networkManager = NetworkManager.singleton as CustomNetworkManager;

        startGameButton.onClick.AddListener(StartGame);
        startGameButton.interactable = false;

        startServerButton.onClick.AddListener(StartServer);
        joinServerButton.onClick.AddListener(JoinGame);

        addressInputField.text = "localhost";
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
        string ipAddress = addressInputField.text;
        if (String.IsNullOrEmpty(ipAddress))
        {
            Debug.Log("you have to enter ip address");
        }
        else
        {
            networkManager.networkAddress = ipAddress;
            networkManager.StartHost();
            Debug.Log(networkManager.networkAddress);
        }
    }

    public void JoinGame()
    {
        string ipAddress = addressInputField.text;
        if (String.IsNullOrEmpty(ipAddress))
        {
            Debug.Log("you have to enter ip address");
        }
        else
        {
            networkManager.networkAddress = ipAddress;
            startGameButton.interactable = true;
            networkManager.StartClient();
        }
    }
}
