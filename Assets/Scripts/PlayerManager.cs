using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : NetworkBehaviour
{
    [SerializeField] float shootCooldown;

    private float lastShootTime;
    private LayerMask layerMask;
    private Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
        layerMask = LayerMask.GetMask("Birds");
    }

    [Client]
    public void OnShoot(){
        float shootTime = Time.time;
        if(shootTime - lastShootTime >= shootCooldown)
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), Mathf.Infinity, layerMask);

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
       Global.Instance.player1Score.text = "AAAAAAAAAA";
    }
}