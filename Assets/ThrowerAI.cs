using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThrowerAI : Enemy
{
    public string idleAnimName = "Zombie_Idle";
    public string runAnimName = "Thrower_Running";
    public string attackAnimName = "Thrower_Attack";

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    public float shootRange = 300.0f;
    public float maxShootDistance = 200.0f;
    public float minShootDistance = 100.0f;

    public int damageAmount = 5;
    float attackCounter = 0.0f;
    public float attackSpeed = 2.0f;

    bool isAttacking = false;

    [SerializeField] WaypointCategory category = WaypointCategory.THROWER;
    
    bool waypointReached = false;
    Waypoint waypoint;

    private void Start()
    {
        currentAnimationState = idleAnimName;
        
        base.Start();
        pursueUpdating.AddListener(CheckForTargetChange);
    }
    private void OnDisable()
    {
        pursueUpdating.RemoveListener(CheckForTargetChange);
    }
    protected override void Behavior()
    {
        if (!waypointReached && waypoint == null)
        {
            this.GetNewTarget();
        }
        
        HasReachedWaypoint();
        attackCounter += Time.deltaTime;
        

        if (agent.velocity != Vector3.zero && currentAnimationState != runAnimName && !isAttacking)
        {
            ChangeAnimationState(runAnimName);
        }
        else if (agent.velocity == Vector3.zero && currentAnimationState != idleAnimName && !isAttacking)
        {
            ChangeAnimationState(idleAnimName);
        }

        BehaviorSearch();
        BehaviorAttack();
    }

    void BehaviorSearch()
    {
        if (waypointReached) return;
        agent.isStopped = false;
        agent.SetDestination(goalLocation);
    }

    void BehaviorAttack()
    {
        if (!waypointReached) return;
        agent.isStopped = true;
        if (currentPursue == EnemyPursueGoal.PLAYER)
        {
            goalLocation = gameManager.instance.player.transform.position;
        }
        faceTarget(lookDir);
        if (attackCounter >= attackSpeed && !isAttacking)
        {
            AttemptAttack();
        }
    }

    void CheckForTargetChange(EnemyPursueGoal prevPursue)
    {
        if (prevPursue != currentPursue)
        {
            GetNewTarget();
            waypointReached = false;
        }
    }

    public override void GetNewTarget()
    {
        if (currentPursue == EnemyPursueGoal.PLAYER)
        {
            waypoint = WaypointManager.instance.RetrieveNearestWaypointWithMin(category, gameManager.instance.player.transform.position, minShootDistance, maxShootDistance, "Player");
            goalLocation = waypoint.GetDispersedLocation();
        }
        else if (currentPursue == EnemyPursueGoal.SHRINE)
        {
            waypoint = WaypointManager.instance.RetrieveNearestWaypointWithMin(category, gameManager.instance.GetShrineLocationDispersed(), minShootDistance,maxShootDistance, "Shrine");
            goalLocation = waypoint.GetDispersedLocation();
        }
        Debug.Log("Got waypoint " + waypoint.name);
    }

    void HasReachedWaypoint()
    {
        float dist = Vector3.Distance(transform.position, goalLocation);
        Debug.Log("Distance from waypoint is " + dist + " and agent stop dist is " + agent.stoppingDistance + " and state is " + waypointReached + " and isStopped is " + agent.isStopped);
        if (!waypointReached && dist <= agent.stoppingDistance)
        {
            waypointReached = true;
            Debug.Log("Reached waypoint " + waypoint.name);
            waypoint = null;
            goalLocation = currentPursue == EnemyPursueGoal.PLAYER ? gameManager.instance.player.transform.position : gameManager.instance.GetShrineLocation();
            agent.velocity = Vector3.zero;
            Debug.Log("Set goal location to " + goalLocation);
        } 
    }

    void AttemptAttack()
    {
        RaycastHit hit;
        LayerMask mask = currentPursue == EnemyPursueGoal.PLAYER ? LayerMask.GetMask("Player") : LayerMask.GetMask("Shrine");
        Vector3 rayDir = (goalLocation - transform.position).normalized;
        Debug.Log("Attempting attack on " + currentPursue);
        //Debug.DrawRay(transform.position, rayDir * shootRange, Color.red, 1.0f);
        if (Physics.Raycast(transform.position, rayDir, out hit, shootRange, mask) && Vector3.Distance(transform.position, goalLocation) <= maxShootDistance && !isAttacking)
        {
            Attack();
        } else
        {
            Debug.Log("Didn't raycast " + currentPursue);
        }
    }

    void Attack()
    {
        isAttacking = true;
        ChangeAnimationState(attackAnimName);
    }

    void SpawnProjectile()
    {
        Vector3 dir = (goalLocation - shootPos.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);
        Instantiate(bullet, shootPos.position, rot);
    }

    void EndAttack()
    {
        isAttacking = false;
        attackCounter = 0;
    }

}
