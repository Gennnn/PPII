using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    enum damageType { moving, stationary, DOT, homing}
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int amount;
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    bool isDamaging;
    void Start()
    {
        if (type == damageType.moving || type  == damageType.homing)
        {
            Destroy(gameObject, destroyTime);

            if(type == damageType.moving)
            {
                rb.velocity = transform.forward * speed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == damageType.homing)
        {
            rb.velocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && (type == damageType.stationary || type == damageType.moving || type == damageType.homing)) {
            dmg.takeDamage(amount);
        }

        if (type == damageType.moving || type == damageType.homing)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && type == damageType.DOT)
        {
            if (!isDamaging)
            {
                StartCoroutine(damageOther(dmg));
            }
        }
    }

    IEnumerator damageOther(IDamage d)
    {
        isDamaging = true;
        d.takeDamage(amount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }
}
