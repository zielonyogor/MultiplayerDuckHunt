using UnityEngine;

public class GameState : State
{
    public GameState(PlayerManager player, StateMachine stateMachine) : base(player, stateMachine) {}
    public override void Enter()
    {
        base.Enter();
        player.playerInput.actions["Shoot"].performed += player.OnShoot;
        player.gameContainer = GameObject.FindGameObjectWithTag("UI").GetComponent<GameContainer>();
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
        player.playerInput.actions["Shoot"].performed -= player.OnShoot;
    }
}