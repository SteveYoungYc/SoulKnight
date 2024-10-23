using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}


public class StateMachine
{
    public State currentState;

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
}

