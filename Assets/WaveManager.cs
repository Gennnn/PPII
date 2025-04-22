using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public Round currentRound;
    gameManager gameManager;

    int currentWaveMobs = 0;

    float initialBuffer = 0.0f;

    [SerializeField] List<Spawn> spawnLocations = new List<Spawn>();

    Dictionary<Wave, Coroutine> pendingWaves = new Dictionary<Wave, Coroutine>();

    [System.NonSerialized] public UnityEvent<int> remainingEnemiesUpdated;
    [System.NonSerialized] public UnityEvent allMobsKilled;

    bool roundActive = false;

    private void Awake()
    {
        instance = this;
        remainingEnemiesUpdated = new UnityEvent<int>();
        allMobsKilled = new UnityEvent();
    }

    private void Start()
    {
        gameManager = gameManager.instance;
    }

    private void Update()
    {
        initialBuffer += Time.deltaTime;
        if (initialBuffer >= 1.0f && currentWaveMobs <= 0 && pendingWaves.Count > 0)
        {
            Wave shortestWave = pendingWaves.Keys.OrderBy(w => w.delay).FirstOrDefault();
            if (shortestWave != null && pendingWaves.TryGetValue(shortestWave, out Coroutine coroutine))
            {
                StopCoroutine(coroutine);
                pendingWaves.Remove(shortestWave);
                SpawnWave(shortestWave);
            }
        } else if (currentWaveMobs <= 0 && pendingWaves.Count <= 0 && roundActive)
        {
            allMobsKilled.Invoke();
            roundActive = false;
        }
    }

    public void StartRound()
    {
        roundActive = true;
        for (int i = 0; i < currentRound.waves.Length; i++)
        {
            if (currentRound.waves[i].delay <= 0)
            {
                SpawnWave(currentRound.waves[i]);
            } else
            {
                Coroutine c = StartCoroutine(SpawnWave(currentRound.waves[i], currentRound.waves[i].delay));
                pendingWaves.Add(currentRound.waves[i], c);
            }
        }
    }

    void SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.GetEnemyCount(); i++)
        {
            StartCoroutine(SpawnMob(wave.GetEnemy(i), wave.spawnLocation, i * wave.delayBetweenEnemies));
            currentWaveMobs++;
            
        }
    }
    IEnumerator SpawnWave(Wave wave, int delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        for (int i = 0; i < wave.GetEnemyCount(); i++)
        {
            StartCoroutine(SpawnMob(wave.GetEnemy(i), wave.spawnLocation, i * wave.delayBetweenEnemies));
            currentWaveMobs++;
        }

    }
    IEnumerator SpawnMob(Enemy enemy, int location, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        CreateMobAtLocation(enemy, GetSpawnLocation(location));
        //Debug.Log("Spawning " + enemy.name);
    }

    Spawn GetSpawnLocation(int id)
    {
        return spawnLocations[id];
    }

    void CreateMobAtLocation(Enemy enemy, Spawn position)
    {
        Instantiate(enemy, position.spawnPosition, Quaternion.identity);
        enemy.SetAgentAreaCost(position.desiredArea, 1);
    }

    public void MobDeath()
    {
        currentWaveMobs--;
        remainingEnemiesUpdated.Invoke(1);
    }
}
