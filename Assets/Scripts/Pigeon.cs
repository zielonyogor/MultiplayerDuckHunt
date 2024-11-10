using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum PigeonState
{
    Standby,
    Fly,
    Shot
}

public class Pigeon : NetworkBehaviour
{
    public int pigeonID = 0;
    [Header("Pigeon variables")]
    public float speed = 5f;
    public int score = 10;

    public PigeonState pigeonState;

    private int direction = 1;
    private SpriteRenderer spriteRenderer;

    public override void OnStartServer()
    {
        PigeonManager.OnPigeonShot.AddListener(HandlePigeonShot);

        spriteRenderer = GetComponent<SpriteRenderer>();
        pigeonState = PigeonState.Standby;
    }

    public void StartFlying(int direction)
    {
        float yPosition = Random.Range(-3, 3);
        this.direction = direction;
        if (direction == -1)
        {
            transform.position = new Vector3(10, yPosition, 0);
        }
        else
        {
            transform.position = new Vector3(-10, yPosition, 0);
        }
        spriteRenderer.flipX = this.direction == -1;
        ChangeState(PigeonState.Fly);
    }

    private void ChangeState(PigeonState newPigeonState)
    {
        pigeonState = newPigeonState;
    }

    private void Update()
    {
        switch (pigeonState)
        {
            case PigeonState.Fly:
                UpdateFly();
                break;
            case PigeonState.Shot:
                UpdateShot();
                break;
            default:
                break;
        }
    }

    private void UpdateFly()
    {
        transform.position += new Vector3(1, 0, 0) * (direction * speed * Time.deltaTime);
        if (transform.position.x * direction > 8)
        {
            transform.position = new Vector3(-10, 0, 0);
            ChangeState(PigeonState.Standby);
        }
    }

    private void UpdateShot()
    {
        transform.position += new Vector3(0, -4, 0) * Time.deltaTime;
        if (Mathf.Abs(transform.position.y) > 6)
        {
            transform.position = new Vector3(-10, 0, 0);
            ChangeState(PigeonState.Standby);
        }
    }

    private void HandlePigeonShot(int playerID, int pigeonID)
    {
        if (pigeonID != this.pigeonID) return;
        ChangeState(PigeonState.Shot);
    }
}
