using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ClientManager : PlayerManager
{
    protected override void Start()
    {
        base.Start();
        Debug.Log("I'm client");
    }

    protected override void Update()
    {
        base.Update();
    }
}
