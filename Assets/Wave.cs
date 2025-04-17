using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/Wave")]
public class Wave : ScriptableObject
{
    [SerializeField] Enemy[] enemies;
    public int delay = 0;
    int currentEnemy = 0;
    public float delayBetweenEnemies = 0.5f;
    public int spawnLocation = 0;

    public Enemy GetNextEnemy()
    {
        if (currentEnemy >= enemies.Length)
        {
            return null;
        }
        Enemy retEnemy = enemies[currentEnemy];
        currentEnemy++;
        return retEnemy;
    }

    public int GetEnemyCount()
    {
        return enemies.Length;
    }
}
