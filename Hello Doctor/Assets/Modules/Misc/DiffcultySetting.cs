using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "LD48/DifficultySetting")]
public class DiffcultySetting : ScriptableObject
{
    public int3 mapSize;
    [Range(0f,1f)]
    public float minesPercent;
    [Range(0f, 1f)]
    public float bonusPercent;
    [Range(0f, 1f)]
    public float alreadySolvedPercent;
}