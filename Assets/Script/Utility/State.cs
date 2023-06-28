using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class State
{
    protected readonly FiniteStateMachine stateMachine;

    public State(FiniteStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract IEnumerator Start();

    public abstract void Terminate();
}
