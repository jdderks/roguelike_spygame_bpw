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

    private Path path;
    private Seeker seeker;
    private AIPath pathfinder;
    private FieldOfView fieldOfView;

    private Transform playerTransform;

    private AIDestinationSetter destinationSetter;

    private Transform currentMovementTarget = null;


    public void Damage(int amount)
    {
        throw new System.NotImplementedException();
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


    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrolling();

                break;
            case EnemyState.Attacking:

                break;
            case EnemyState.Alarmed:

                break;
            default:
                break;
        }
    }


    private void Patrolling()
    {
        if (currentMovementTarget == null)
        {
            var neighbors = RoomManager.instance.GetNearestRooms(transform, seekRange);
            currentMovementTarget = neighbors[Random.Range(0, neighbors.Count-1)];
        }
        destinationSetter.target = currentMovementTarget;

        if (fieldOfView.visibleTargets.Contains(playerTransform))
        {
            destinationSetter.target = playerTransform;
        }

        if (Vector2.Distance(transform.position, currentMovementTarget.position) < 0.2f)
        {
            currentMovementTarget = null;
        }
    }

    private void Seek()
    {
        
    }


    private void OnDrawGizmos()
    { 
        Gizmos.DrawWireSphere(transform.position,seekRange);
    }
}
