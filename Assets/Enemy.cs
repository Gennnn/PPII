using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 20;
    int maxHealth;
    public float moveSpeed = 20.0f;

    void Start()
    {
        maxHealth = health;
    }

    void Update()
    {
        
    }
}
