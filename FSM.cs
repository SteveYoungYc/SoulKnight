using UnityEngine;

public abstract class State
{
    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Update() {}
}

public class IdleState : State
{
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

