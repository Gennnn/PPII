using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{
    public int damage = 5;
    public float swingRange = 3.0f;
    public float swingSpeed = 0.25f;
    float swingCooldown = 0f;
    public string swingAnimationName = "Sword_Swing";
    public string idleAnimationName = "Sword_Idle";
    Coroutine idleCoroutine;
    void Start()
    {
        currentAnimationState = idleAnimationName;
    }

    void Update()
    {
        swingCooldown += Time.deltaTime;
    }
    public override void Primary()
    {
        if (swingCooldown >= swingSpeed)
        {
            swingCooldown = 0.0f;
            ChangeAnimationState(swingAnimationName);
            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = StartCoroutine(ReturnToIdle());
            }
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, swingRange, ~gameManager.instance.playerScript.ignoreLayer))
            {
                Debug.Log(hit.collider.name);
                IDamage dmg = hit.collider.GetComponent<IDamage>();
                
                if (dmg != null)
                {
                    dmg.takeDamage(damage);

                }
            }
        }
        
        
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(swingSpeed);
        ChangeAnimationState(idleAnimationName);
        idleCoroutine = null;
    }

    public override void Secondary()
    {
        
    }
}
