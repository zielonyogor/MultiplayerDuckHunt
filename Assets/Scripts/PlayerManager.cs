using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using Edgegap;

[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : NetworkBehaviour
{
    public PlayerInput playerInput { get; private set; }
    [SerializeField] float shootCooldown;

    private float lastShootTime = 0;
    private LayerMask layerMask;
    public GameContainer gameContainer;

    // States
    StateMachine stateMachine;

    public GameState gameState;
    public LobbyState lobbyState;

    virtual protected void Start() {
        playerInput = GetComponent<PlayerInput>();

        stateMachine = new StateMachine();
        gameState = new GameState(this, stateMachine);
        lobbyState = new LobbyState(this, stateMachine);
        stateMachine.Init(lobbyState);

        layerMask = LayerMask.GetMask("Birds");
    }

    virtual protected void Update()
    {
        stateMachine.currentState.Update();
    }

    [Client]
    public void OnShoot(InputAction.CallbackContext context){
        float shootTime = Time.time;
        if(shootTime - lastShootTime >= shootCooldown)
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(gameContainer.mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), Mathf.Infinity, layerMask);

            if(hit.collider != null)
            {
                lastShootTime = Time.time;
                CmdShoot();
            }
        }
    }

    [Command(requiresAuthority = false)]
    void CmdShoot()
    {
       Debug.Log(connectionToClient);
       gameContainer.player1Score.text = "AAAAAAAAAA";
    }
}