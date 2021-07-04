using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

enum EnemyState
{
    Patrolling,
    Attacking,
    Alarmed
}

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float seekRange = 15f;
    [SerializeField] private LayerMask navigationLayer;
    [SerializeField] private EnemyState currentState;
    [SerializeField] private float movementDestinationInterval = 0.5f;

    [SerializeField] private float totalFOV = 70.0f;
    [SerializeField] private float rayRange = 10.0f;

    [SerializeField] private int health = 100;

    private Path path;
    private Seeker seeker;
    private AIPath pathfinder;
    private FieldOfView fieldOfView;

    private Transform playerTransform;

    private AIDestinationSetter destinationSetter;

    private Transform currentMovementTarget = null;

    private float seekTimer = 8f;

    public void Damage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            Die();
        }
    }

    private void Start()
    {
        currentState = EnemyState.Patrolling;

        seeker = GetComponent<Seeker>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        pathfinder = GetComponent<AIPath>();
        fieldOfView = GetComponent<FieldOfView>();

        playerTransform = FindObjectOfType<Player>().GetComponent<Transform>();

        pathfinder.maxSpeed = movementSpeed;
        pathfinder.repathRate = movementDestinationInterval;
    }

    void Die()
    {
        UIManager.instance.EnemiesKilled++;
        Destroy(gameObject);
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patroll();

                break;
            case EnemyState.Attacking:
                Attack();

                break;
            case EnemyState.Alarmed:

                break;
            default:
                break;
        }
    }

    private void Patroll()
    {
        pathfinder.maxSpeed = 2f;
        if (currentMovementTarget == null)
        {
            var neighbors = RoomManager.instance.GetNearestRooms(transform, seekRange);
            currentMovementTarget = neighbors[Random.Range(0, neighbors.Count - 1)];
        }
        destinationSetter.target = currentMovementTarget;

        if (fieldOfView.visibleTargets.Contains(playerTransform))
        {
            Aggro();
        }

        if (Vector2.Distance(transform.position, currentMovementTarget.position) < 0.2f)
        {
            currentMovementTarget = null;
        }
    }

    private void Attack()
    {
        
        seekTimer -= Time.deltaTime;
        if (fieldOfView.visibleTargets.Contains(playerTransform))
        {
            seekTimer = 8f;
            pathfinder.maxSpeed = 3.5f;
        }
        if (seekTimer > 0)
        {
            destinationSetter.target = playerTransform;
        }
        else
        {
            currentState = EnemyState.Patrolling;
        }

        if (Vector3.Distance(playerTransform.position, transform.position) < 1)
        {
            UIManager.instance.LoseGame();
            Player p = playerTransform.GetComponent<Player>();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, seekRange);
    }

    public void Aggro()
    {
        UIManager.instance.EnemiesAlerted++;
        seekTimer = 3f;
        currentState = EnemyState.Attacking;
    }
}
