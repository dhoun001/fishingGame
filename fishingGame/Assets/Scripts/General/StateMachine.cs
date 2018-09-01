using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine: MonoBehaviour
{
    public State currentState = null;

    protected Dictionary<Type, State> validStates = new Dictionary<Type, State>();

    /// <summary>
    /// Avoid running update on the same frame as OnEnter
    /// </summary>
    private bool runOnUpdate = false;

    /// <summary>
    /// Set state machine to go to another valid State
    /// </summary>
    public void SetState(Type t)
    {
        if (!validStates.ContainsKey(t))
        {
            Debug.LogError("Could not go to state " + t.Name + ", make sure the state is attached on the same gameobject that its state machine is attached to.");
            return;
        }

        State s = validStates[t];

        if (currentState != null)
            currentState.OnExit();

        currentState = s;

        runOnUpdate = false;
        s.OnEnter();

        StartCoroutine(RunOnUpdate());
    }

    /// <summary>
    /// Call current state on exit, then set current state to null
    /// Should be called if switching to another state machine
    /// </summary>
    public void DisableStateMachine()
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = null;
        //runOnUpdate = false;
    }

    protected void AddNewState(State s)
    {
        validStates.Add(s.GetType(), s);
    }

    /// <summary>
    /// Wait for end of frame, to ensure OnEnter occurs first, then Update
    /// </summary>
    private IEnumerator RunOnUpdate()
    {
        yield return new WaitForEndOfFrame();
        runOnUpdate = true;
    }

    private void Update()
    {
        if (!runOnUpdate)
            return;

        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    private void LateUpdate()
    {
        if (!runOnUpdate)
            return;

        if (currentState != null)
        {
            currentState.OnLateUpdate();
        }
    }

}
