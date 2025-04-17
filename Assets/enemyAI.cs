using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : Enemy, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] BoxCollider attackCollider;

    [SerializeField] int faceTargetSpeed;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    Color colorOrig;

    Vector3 playerDir;

    float attackCounter = 0.0f;
    public float attackSpeed = 0.7f;
    public int damageAmount = 5;

    private void Start()
    {
        model = GetComponent<Renderer>();
        colorOrig = model.material.color;
    }

    void Update()
    {
        attackCounter += Time.deltaTime;
        playerDir = (gameManager.instance.transform.position - transform.position);
        
        agent.SetDestination(gameManager.instance.player.transform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            faceTarget();
        }

        if (attackCollider.bounds.Intersects(gameManager.instance.playerScript.characterController.bounds) && attackCounter >= attackSpeed)
        {
            attackCounter = 0.0f;
            gameManager.instance.playerScript.takeDamage(damageAmount);
        }
        

        /*shootTimer += Time.deltaTime;
        if (shootTimer >= shootRate)
        {
            shoot();

        }*/
    }


    public void takeDamage(int amount)
    {
        health -= amount;
        StartCoroutine(flashRed());

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

    /*void shoot()
    {
        shootTimer = 0;
        Instantiate(bullet, shootPos.position, transform.rotation);
    }*/

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot,Time.deltaTime * faceTargetSpeed);
    }
}
