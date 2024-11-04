using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

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
    [SerializeField] float shootCooldown = 0.5f;

    private float lastShootTime = 0;
    private LayerMask layerMask;
    public GameContainer gameContainer;

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
        gameContainer = GameObject.FindGameObjectWithTag("UI").GetComponent<GameContainer>();
        layerMask = LayerMask.GetMask("Birds");
    }

    [Client]
    public void OnShoot()
    {
        if (!isLocalPlayer) return;
        Debug.Log(playerID);
        float shootTime = Time.time;
        if (shootTime - lastShootTime >= shootCooldown)
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(gameContainer.mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                Debug.Log("shot");
                lastShootTime = Time.time;
                gameContainer.CmdUpdateScore(playerID);
            }
        }
    }
}