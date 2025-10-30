using UnityEngine;

[CreateAssetMenu(fileName = "playerStats", menuName = "Scriptable Objects/playerStats")]
public class PlayerStats : ScriptableObject
{
    public float maxHp = 100;
    public float shield = 0;
    public float damage = 1;
    public float range = 1;
    public float projectileCount = 0;
    public float criticalChances = 50;
    public float criticalDamage = 2;
    public float attackSpeed = 0;
    public float moveSpeed = 1;
    public float pickupRange = 0;
}
