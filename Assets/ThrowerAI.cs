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
    public float maxShootDistance = 100.0f;
    public float minShootDistance = 50.0f;

    public int damageAmount = 5;
    float attackCounter = 0.0f;
    public float attackSpeed = 2.0f;

    [SerializeField] float waypointPassDistance = 1.5f;

    bool isAttacking = false;

    [SerializeField] WaypointCategory category = WaypointCategory.THROWER;
    Waypoint waypoint;
    bool reachedWaypoint = false;

    private void Awake()
    {
        //Debug.Log($"{name}: {this.GetType().Name} Awake called. Enabled? {enabled}");
    }

    private void Start()
    {
        currentAnimationState = idleAnimName;
        waypoint = WaypointManager.instance.RetrieveNearestWaypointWithMin(category, gameManager.instance.player.transform.position, minShootDistance);
        base.Start();
    }
    protected override void Behavior()
    {
        UpdateWaypoint();
        HasReachedWaypoint();
        
        if (reachedWaypoint)
        {
            agent.isStopped = true;
        } else
        {
            agent.isStopped = false;
        }
        agent.SetDestination(GetTargetPosition());

        attackCounter += Time.deltaTime;
        faceTarget(lookDir);

        if (agent.velocity != Vector3.zero && currentAnimationState != runAnimName && !isAttacking)
        {
            ChangeAnimationState(runAnimName);
        }
        else if (agent.velocity == Vector3.zero && currentAnimationState != idleAnimName && !isAttacking)
        {
            ChangeAnimationState(idleAnimName);
        }

        if (attackCounter >= attackSpeed && !isAttacking)
        {
            //Debug.Log(name + " is attempting attack");
            AttemptAttack();
        }
    }

    void HasReachedWaypoint()
    {
        if (!waypointReached && Vector3.Distance(transform.position, GetTargetPosition()) <= waypointPassDistance)
        {
            waypointReached = true;
        } else if (waypointReached && Vector3.Distance(transform.position, GetTargetPosition()) > waypointPassDistance)
        {
            waypointReached = false;
        }
    }

    void UpdateWaypoint()
    {
        if (currentPursue == EnemyPursueGoal.PLAYER)
        {
            waypoint = WaypointManager.instance.RetrieveNearestWaypointWithMin(category, gameManager.instance.player.transform.position, minShootDistance, maxShootDistance, LayerMask.GetMask("Player"));
        } else if (currentPursue == EnemyPursueGoal.SHRINE)
        {
            waypoint = WaypointManager.instance.RetrieveNearestWaypointWithMin(category, gameManager.instance.GetShrineLocationDispersed(), minShootDistance, maxShootDistance, LayerMask.GetMask("Shrine"));
        }
    }

    void AttemptAttack()
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Player");
        Vector3 rayDir = (gameManager.instance.player.transform.position - transform.position).normalized;
        //Debug.DrawRay(transform.position, rayDir * shootRange, Color.red, 1.0f);
        if (Physics.Raycast(transform.position, rayDir, out hit, shootRange, mask) && Vector3.Distance(transform.position, gameManager.instance.player.transform.position) <= maxShootDistance && !isAttacking)
        {
            Attack();
        } 
    }

    void Attack()
    {
        isAttacking = true;
        agent.isStopped = true;
        ChangeAnimationState(attackAnimName);
    }

    void SpawnProjectile()
    {
        Vector3 dir = (gameManager.instance.player.transform.position - shootPos.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);
        Instantiate(bullet, shootPos.position, rot);
    }

    void EndAttack()
    {
        isAttacking = false;
        attackCounter = 0;
        agent.isStopped = false;
    }

}
