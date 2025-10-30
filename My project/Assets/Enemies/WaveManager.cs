using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Zone Configuration")]
    public ZoneData[] zones;
    private int currentZoneIndex = 0;

    [Header("Spawning")]
    public float spawnRadius = 15f;
    public Transform player;
    
    private int enemiesRemaining;
    
    void Start()
    {
        StartZone(currentZoneIndex);
    }
    
    void StartZone(int zoneIndex)
    {
        StartCoroutine(SpawnZoneWaves(zones[zoneIndex]));
    }
    
    IEnumerator SpawnZoneWaves(ZoneData zone)
    {
        foreach (WaveData wave in zone.waves)
        {
            yield return StartCoroutine(SpawnWave(wave, zone.zoneNumber));
            
            if (zone.waitForWaveClear)
            {
                yield return new WaitUntil(() => enemiesRemaining <= 0);
            }
            else
            {
                yield return new WaitForSeconds(wave.timeBetweenWaves);
            }
        }
        
        OnZoneComplete();
    }
    
    IEnumerator SpawnWave(WaveData wave, int zoneNumber)
    {
        foreach (EnemySpawnInfo spawnInfo in wave.enemies)
        {
            for (int i = 0; i < spawnInfo.count; i++)
            {
                SpawnEnemy(spawnInfo.enemyData, zoneNumber);
                enemiesRemaining++;
                
                yield return new WaitForSeconds(spawnInfo.spawnDelay);
            }
        }
    }
    
    void SpawnEnemy(EnemyData enemyData, int zoneNumber)
    {
        Vector2 spawnPos = GetRandomSpawnPosition();
        
        GameObject enemy = Instantiate(enemyData.prefab, spawnPos, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript == null)
        {
            Debug.LogError("Enemy script not found");
            return;
        }

        enemyScript.ApplyZoneScaling(zoneNumber);

        enemyScript.OnDeath += () => enemiesRemaining--;
    }

    Vector2 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;
        return (Vector2)player.position + offset;
    }
    
    void OnZoneComplete()
    {
        // TODO: Open merchant
        
        currentZoneIndex++;
        if (currentZoneIndex < zones.Length)
        {
            // Next zone
        }
        else
        {
            // Run completed
        }
    }
}
