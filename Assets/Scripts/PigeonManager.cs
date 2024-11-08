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

    private List<Pigeon> pigeons = new List<Pigeon>();
    private List<Pigeon> rareBirds = new List<Pigeon>();
    private float timeBetweenSpawn = 0.5f;
    private float lastSpawnTime;


    float minSpawnTime = 0.5f;
    float maxSpawnTime = 4f;

    public GameObject shotMarkerPrefab;

    public override void OnStartServer()
    {
        gameContainer = GameObject.FindGameObjectWithTag("UI").GetComponent<GameContainer>();
        layerMask = LayerMask.GetMask("Birds");
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Pigeon pigeonScript = child.GetComponent<Pigeon>();
            if (child.CompareTag("RareBird"))
            {
                rareBirds.Add(pigeonScript);

            }
            else
            {
                pigeons.Add(pigeonScript);
            }
            pigeonScript.pigeonID = i;
        }
    }

    private void Update()
    {
        if (Time.time - lastSpawnTime > timeBetweenSpawn)
        {
            foreach (var pigeon in pigeons) //for now spawning only normal pigeons
            {
                if (pigeon.pigeonState == PigeonState.Standby)
                {
                    int birdDirection = Random.value < 0.5f ? 1 : -1; //50% chance for left or right
                    pigeon.StartFlying(birdDirection);

                    lastSpawnTime = Time.time;
                    timeBetweenSpawn = Random.Range(minSpawnTime, maxSpawnTime);
                    return;
                }
            }
        }
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
