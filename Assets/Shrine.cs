using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour, IDamage
{
    [SerializeField] int health = 250;
    int maxHealth;
    [SerializeField] int regenAmount = 10;
    [SerializeField] float regenRate = 2.5f;
    Collider captureField;
    void Start()
    {
        maxHealth = health;
        captureField = GetComponent<Collider>();
        StartCoroutine(Regen());
    }

    IEnumerator Regen()
    {
        yield return new WaitForSeconds(regenRate);
        health += regenAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
        StartCoroutine(Regen());
    }

    public void takeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    public Collider GetCaptureField()
    {
        return captureField;
    }
}
