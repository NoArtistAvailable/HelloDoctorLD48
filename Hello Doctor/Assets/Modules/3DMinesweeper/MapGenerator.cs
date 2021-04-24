using elZach.Common;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Map map;
    public int3 dimensions;
    [Range(0f,1f)]
    float minesPercent=0.2f;
    [Range(0f, 1f)]
    float boniPercent=0.2f;
    public uint seed;

    public GameObject blockPrefab;
    public float3 spacing = new float3(1, 1, 1);

    [Button("Generate")]
    public void Generate()
    {
        map = new Map(dimensions.x, dimensions.y, dimensions.z, minesPercent, boniPercent, seed);
        for(int x =0; x < map.Width; x++)
            for (int y = 0; y < map.Height; y++)
                for (int z = 0; z < map.Depth; z++)
                {
                    var block = Instantiate(blockPrefab, transform);
                    block.name = map.grid[x, y, z].ToString();
                    block.transform.localPosition = new Vector3(spacing.x * x, spacing.y * y, spacing.z * z);
                }
    }
}
