
using UnityEngine;

public class LobbyState : State
{
    public LobbyState(PlayerManager player, StateMachine stateMachine) : base(player, stateMachine) {}
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        // needs to have connection with start button
        if(Input.GetKeyDown("space"))
        {
            stateMachine.Change(player.gameState);
        }
    }

    public override void Exit()
    {
    }
}