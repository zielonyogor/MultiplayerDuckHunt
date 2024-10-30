using UnityEngine;

public class State
{
    protected StateMachine stateMachine;
    protected PlayerManager player;

    public State(PlayerManager player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        Debug.Log("Entering state: " + this.ToString());
    }

    public virtual void Update() { }

    public virtual void Exit() { }
}