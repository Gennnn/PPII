using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : Enemy
{
    public string idleAnimName = "Zombie_Idle";
    public string runAnimName = "Zombie_Running";
    public string attackAnimName = "Zombie_Attack";

    [SerializeField] BoxCollider attackCollider;

    public int damageAmount = 5;
    float attackCounter = 0.0f;
    public float attackSpeed = 0.7f;

    bool isAttacking = false;

    private void Awake()
    {
        //Debug.Log($"{name}: {this.GetType().Name} Awake called. Enabled? {enabled}");
    }

    private void Start()
    {
        currentAnimationState = idleAnimName;
        base.Start();
    }

    protected override void Behavior()
    {
        if (!isAttacking)
        {
            attackCounter += Time.deltaTime;
        }

        agent.SetDestination(GetTargetPosition());

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
            AttemptAttack();
        }
    }

    void AttemptAttack()
    {
        if ( (attackCollider.bounds.Intersects(gameManager.instance.playerScript.characterController.bounds) || (attackCollider.bounds.Intersects(gameManager.instance.shrine.GetCaptureField().bounds))) && !isAttacking)
        {
            ChangeAnimationState(attackAnimName);
            agent.isStopped = true;
            isAttacking = true;
        }
    }

    void Attack()
    {
        if (attackCollider.bounds.Intersects(gameManager.instance.playerScript.characterController.bounds))
        {
            gameManager.instance.playerScript.takeDamage(damageAmount);
        } else if (attackCollider.bounds.Intersects(gameManager.instance.shrine.GetCaptureField().bounds))
        {
            gameManager.instance.shrine.takeDamage(damageAmount);
        }
        
    }

    void EndAttack()
    {
        attackCounter = 0.0f;
        isAttacking = false;
        agent.isStopped = false;
    }

    


}
