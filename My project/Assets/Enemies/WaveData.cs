using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public EnemyData enemyData;
    public int count;
    public float spawnDelay;
}

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    public EnemySpawnInfo[] enemies;
    public float timeBetweenWaves;
}
