using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class archerAI : Enemy, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    protected Animator animator;
    protected string currentAnimationState;

    public string idleAnimName = "Zombie_Idle";
    public string runAnimName = "Zombie_Running";
    public string attackAnimName = "Zombie_Attack";

    [SerializeField] int faceTargetSpeed;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    public float shootRange = 300.0f;
    [SerializeField] float shootRate;

    [SerializeField] ParticleSystem hitParticles;

    Color colorOrig;

    Vector3 playerDir;

    float attackCounter = 0.0f;
    public float attackSpeed = 2.0f;
    public int damageAmount = 5;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        colorOrig = model.material.color;
        currentAnimationState = idleAnimName;
    }

    void Update()
    {
        attackCounter += Time.deltaTime;
        playerDir = (gameManager.instance.transform.position - transform.position);
        
        agent.SetDestination(gameManager.instance.player.transform.position);

        //faceTarget();

        if (agent.velocity != Vector3.zero && currentAnimationState != runAnimName && currentAnimationState != attackAnimName)
        {
            ChangeAnimationState(runAnimName);
        } else if (agent.velocity == Vector3.zero && currentAnimationState != idleAnimName && currentAnimationState != attackAnimName)
        {
            ChangeAnimationState(idleAnimName);
        }
        

        attackCounter += Time.deltaTime;
        if (attackCounter >= attackSpeed)
        {
            AttemptAttack();
        }
    }

    void AttemptAttack()
    {
        RaycastHit hit;
        LayerMask mask = (1 << 3);
        if (Physics.Raycast(transform.position, transform.forward, out hit, shootRange, mask))
        {
            attackCounter = 0;
            shoot();
        }
    }


    public void takeDamage(int amount)
    {
        health -= amount;
        StartCoroutine(flashRed());
        ParticleSystem particles = Instantiate(hitParticles, model.transform);
        particles.Play();
        if (health <= 0 )
        {
            WaveManager.instance.MobDeath();
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    void shoot()
    {
        attackCounter = 0;
        Instantiate(bullet, shootPos.transform.position, transform.rotation);
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot,Time.deltaTime * faceTargetSpeed);
    }

    void ChangeAnimationState(string newState)
    {
        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.1f);
    }
}
