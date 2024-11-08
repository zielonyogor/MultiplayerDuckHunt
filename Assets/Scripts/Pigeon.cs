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
    public PigeonState pigeonState = PigeonState.Fly;

    public override void OnStartServer()
    {
        PigeonManager.OnPigeonShot.AddListener(HandlePigeonShot);
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
        transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
        if (Mathf.Abs(transform.position.x) > 8)
        {
            transform.position = new Vector3(-10, 0, 0);
            ChangeState(PigeonState.Standby);
        }
    }

    private void UpdateShot()
    {
        transform.position += new Vector3(0, -2, 0) * Time.deltaTime;
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
