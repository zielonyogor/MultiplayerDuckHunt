using UnityEngine;

public class StateMachine 
{
    public State currentState;

    public void Init(State state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void Change(State newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}