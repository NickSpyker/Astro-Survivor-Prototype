using UnityEngine;

public enum EnemyType
{
    Normal,
    Scout,
    Cruiser,
    Hunter,
    Monolith
}

public enum EnemyBehaviorType
{
    RamPlayer,      // Normal, Scout, Cruiser
    RangedGatling,  // Hunter
    RangedMixed     // Monolith
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public EnemyType type;

    [Header("Stats")]
    public float baseHP;
    public float baseDamage;
    public float baseSpeed = 7.5f;

    [Header("Behavior")]
    public EnemyBehaviorType behaviorType;
    public float attackRange;

    [Header("Visuals")]
    public GameObject prefab;
}
