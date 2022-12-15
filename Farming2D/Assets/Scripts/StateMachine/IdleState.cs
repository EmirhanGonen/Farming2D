using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chaseState;
    public bool isChasing;

    public override State RunCurrentState()
    {
        if (isChasing) return chaseState;
        return this;
    }
    public void GoIdle()
    {
        chaseState.agent.speed = 0f;
        isChasing = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Player player)) return;
        chaseState.GoChase();
        isChasing = true;
    }
}