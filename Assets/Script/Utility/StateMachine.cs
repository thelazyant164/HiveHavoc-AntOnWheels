using UnityEngine;

public abstract class FiniteStateMachine : MonoBehaviour
{
    public State CurrentState { get; private set; }
    private Coroutine running;

    public void SetState(State newState)
    {
        if (running != null) 
        {
            StopCoroutine(running); 
        }
        CurrentState?.Terminate();
        CurrentState = newState;
        running = StartCoroutine(CurrentState.Start());
    }
}
