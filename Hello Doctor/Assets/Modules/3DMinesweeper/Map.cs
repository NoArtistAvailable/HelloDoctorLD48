using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

[System.Serializable]
public class Map
{
    public int Width, Height, Depth;
    public int[,,] grid;

    public Map(int width, int height, int depth, float minesPercent, float bonusPercent, ref Random rand)
    {
        Width = width;
        Height = height;
        Depth = depth;

        grid = new int[Width, Height, Depth];
        int totalCount = Width * Height * Depth;

        int mines = Mathf.FloorToInt(totalCount * minesPercent);
        int boni = Mathf.FloorToInt(totalCount * bonusPercent);

        for(int i = 0; i < mines; i++)
        {
            int x = rand.NextInt(Width);
            int y = rand.NextInt(Height);
            int z = rand.NextInt(Depth);
            grid[x, y, z] += -1;
        }

        for (int i = 0; i < boni; i++)
        {
            int x = rand.NextInt(Width);
            int y = rand.NextInt(Height);
            int z = rand.NextInt(Depth);
            grid[x, y, z] += 1;
        }
    }

    public bool IsValidPosition(int3 pos)
    {
        return pos.x >= 0 && pos.x < Width
            && pos.y >= 0 && pos.y < Height
            && pos.z >= 0 && pos.z < Depth;
    }
}
