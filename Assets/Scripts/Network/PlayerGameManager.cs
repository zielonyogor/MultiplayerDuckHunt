using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using System;

public class PlayerGameManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnPlayerIDChange))]
    public int playerID;

    private CustomNetworkManager room;
    private CustomNetworkManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as CustomNetworkManager;
        }
    }

    [Header("Game properties")]
    private PlayerInput playerInput;
    [SerializeField] float shootCooldown = 0.1f;

    private float lastShootTime = 0;
    public PigeonManager pigeonManager;
    public Camera mainCamera;

    private void OnPlayerIDChange(int oldID, int newID)
    {
        playerID = newID;
    }

    public override void OnStopClient()
    {
        Room.gamePlayers.Remove(this);
    }

    public override void OnStartAuthority()
    {
        playerInput = GetComponent<PlayerInput>();
        pigeonManager = GameObject.FindGameObjectWithTag("PigeonManager").GetComponent<PigeonManager>();
        mainCamera = Camera.main;
    }

    [Client]
    public void OnShoot()
    {
        if (!isLocalPlayer) return;
        float shootTime = Time.time;
        if (shootTime - lastShootTime >= shootCooldown)
        {
            pigeonManager.CmdCheckIfShot(playerID, mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
            lastShootTime = shootTime; //needs to be somewhere else or maybe not???
        }
    }
}