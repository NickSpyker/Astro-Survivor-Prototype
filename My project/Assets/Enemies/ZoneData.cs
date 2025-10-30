using UnityEngine;

[CreateAssetMenu(fileName = "ZoneData", menuName = "Scriptable Objects/ZoneData")]
public class ZoneData : ScriptableObject
{
    public int zoneNumber = 1;
    public WaveData[] waves;
    public bool waitForWaveClear = true;
}
