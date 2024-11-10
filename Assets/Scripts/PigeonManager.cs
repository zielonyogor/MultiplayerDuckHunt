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

    private bool gameIsPlaying = false;

    private List<Pigeon> pigeons = new List<Pigeon>();
    private float timeBetweenSpawn = 0.5f;
    private float lastSpawnTime;

    [Header("Basic pigeons spawn range")]
    [SerializeField] float minSpawnTime = 0.1f;
    [SerializeField] float maxSpawnTime = 3f;

    private List<Pigeon> rareBirds = new List<Pigeon>();

    [Header("Time left since rare birds can spawn")]
    [SerializeField] private float rareStartTime = 80f;
    private float timeBetweenRareSpawn = 5f;
    private float lastRareSpawnTime;

    [Header("Rare bird spawn range")]
    [SerializeField] float minRareSpawnTime = 4f;
    [SerializeField] float maxRareSpawnTime = 18f;

    [Header("Shot marker for debug")]
    public GameObject shotMarkerPrefab;

    public override void OnStartServer()
    {
        gameContainer = GameObject.FindGameObjectWithTag("UI").GetComponent<GameContainer>();
        GameCountdown.OnGameCountdownEnd.AddListener(() =>
        {
            gameIsPlaying = true;
        });
        GameTimer.OnTimerEnd.AddListener(() =>
        {
            gameIsPlaying = false;
        });

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
        if (!gameIsPlaying) return;
        if (Time.time - lastSpawnTime > timeBetweenSpawn)
        {
            foreach (var pigeon in pigeons)
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
        if (gameContainer.timer.currentTime > rareStartTime) return;
        if (Time.time - lastRareSpawnTime > timeBetweenRareSpawn)
        {
            foreach (var rareBird in rareBirds)
            {
                if (rareBird.pigeonState == PigeonState.Standby)
                {
                    int birdDirection = Random.value < 0.5f ? 1 : -1; //50% chance for left or right
                    rareBird.StartFlying(birdDirection);

                    lastRareSpawnTime = Time.time;
                    timeBetweenRareSpawn = Random.Range(minRareSpawnTime, maxRareSpawnTime);
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
            gameContainer.UpdateScore(playerID, shotPigeon.score);
        }
    }

    [ClientRpc]
    private void RpcSpawnShotMarker(Vector2 position)
    {
        GameObject marker = Instantiate(shotMarkerPrefab, position, Quaternion.identity);
        Destroy(marker, 0.5f);
    }
}
