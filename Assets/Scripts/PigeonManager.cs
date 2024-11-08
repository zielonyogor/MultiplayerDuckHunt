using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class PigeonManager : NetworkBehaviour
{
    public static UnityEvent<int, int> OnPigeonShot = new UnityEvent<int, int>();

    private LayerMask layerMask;
    public GameContainer gameContainer;
    [SerializeField] List<Pigeon> pigeons;

    public GameObject shotMarkerPrefab;

    private void OnEnable()
    {
    }

    public override void OnStartServer()
    {
        gameContainer = GameObject.FindGameObjectWithTag("UI").GetComponent<GameContainer>();
        layerMask = LayerMask.GetMask("Birds");
    }

    [Command(requiresAuthority = false)]
    public void CmdCheckIfShot(int playerID, Ray cameraRay)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(cameraRay, Mathf.Infinity, layerMask);

        RpcSpawnShotMarker(hit.point);

        if (hit.collider != null)
        {
            Pigeon shotPigeon = hit.collider.gameObject.GetComponent<Pigeon>();
            if (shotPigeon.pigeonState != PigeonState.Fly) return;

            Debug.Log("Shot a pigeon with ID: " + hit.collider.gameObject.GetComponent<Pigeon>().pigeonID);
            OnPigeonShot.Invoke(playerID, hit.collider.gameObject.GetComponent<Pigeon>().pigeonID);
        }
    }

    [ClientRpc]
    private void RpcSpawnShotMarker(Vector2 position)
    {
        GameObject marker = Instantiate(shotMarkerPrefab, position, Quaternion.identity);
        Destroy(marker, 0.5f);
    }
}
