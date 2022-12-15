using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    public IdleState idleState;
    private NPC npc;
    public bool isIdle;


    public NavMeshAgent agent;
    public float speed;
    public Vector3 destination;
    public float walkRadius;

    private void Awake()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        npc = GetComponentInParent<NPC>();
    }

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void GoChase()
    {
        isIdle = false;
        agent.speed = speed;
        agent.SetDestination(RandomNavmeshLocation(walkRadius));
    }

    public override State RunCurrentState()
    {
        if (isIdle) return idleState;
        if (IsNearDestination() & !IsInvoking(nameof(MoveRandom))) { npc.animator.SetFloat("X", 0); Invoke(nameof(MoveRandom), Random.Range(5, 10)); }

        return this;
    }
    private void MoveRandom() { agent.SetDestination(RandomNavmeshLocation(walkRadius)); }
    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.parent.position;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, 1)) finalPosition = hit.position;

        Vector2 direction = finalPosition - transform.parent.position;
        npc.animator.SetFloat("X", direction.x < 0 ? -1 : 1);

        return finalPosition;
    }
    private bool IsNearDestination() => agent.remainingDistance < .5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Player player)) return;
        idleState.GoIdle();
        isIdle = true;
        npc.animator.SetFloat("X", 0);
    }
}