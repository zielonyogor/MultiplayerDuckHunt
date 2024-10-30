using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class HostManager : PlayerManager
{
    protected override void Start()
    {
        base.Start();
        Debug.Log("I'm host");
    }

    protected override void Update()
    {
        base.Update();
    }
}
