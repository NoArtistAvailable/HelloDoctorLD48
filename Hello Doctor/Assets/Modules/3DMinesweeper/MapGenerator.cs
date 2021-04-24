using elZach.Common;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Map map;
    public Block[,,] blocks;
    public Vector3 completeSize = Vector3.one;
    public int3 dimensions;
    [Range(0f,1f)]
    public float minesPercent=0.2f;
    [Range(0f, 1f)]
    public float boniPercent=0.2f;
    public uint seed;

    public Block blockPrefab;
    float3 blockSize = new float3(1, 1, 1);

    public Color mineColor, bonusColor;

    public int revealRandomAtStart = 3;

    private void Start()
    {
        if (seed == 0) seed = (uint)(UnityEngine.Random.Range(int.MinValue, int.MaxValue) + int.MaxValue);
        var rand = new Unity.Mathematics.Random(seed);
        Generate();
        Block.OnBlockRevealed.AddListener(RevealNeighbours);
        for (int i = 0; i < revealRandomAtStart; i++)
            blocks[rand.NextInt(map.Width), rand.NextInt(map.Height), rand.NextInt(map.Depth)].CheckNeighbours();
    }

    [Button("Generate")]
    public void Generate()
    {
        blockSize = new float3(completeSize.x / (float)dimensions.x, completeSize.y / (float)dimensions.y, completeSize.z / (float)dimensions.z);
        
        map = new Map(dimensions.x, dimensions.y, dimensions.z, minesPercent, boniPercent, seed);
        blocks = new Block[map.Width, map.Height, map.Depth];
        Vector3 positionOffset = (new Vector3(map.Width*blockSize.x, map.Height*blockSize.y, map.Depth*blockSize.z) - (Vector3) blockSize ) * -0.5f;
        for(int x =0; x < map.Width; x++)
            for (int y = 0; y < map.Height; y++)
                for (int z = 0; z < map.Depth; z++)
                {
                    var block = Instantiate(blockPrefab, transform);
                    block.name = x+":"+y+":"+z + "["+ map.grid[x, y, z] + "]";
                    block.transform.localPosition = new Vector3(blockSize.x * x, blockSize.y * y, blockSize.z * z) + positionOffset;
                    block.transform.localScale = blockSize;

                    block.value = map.grid[x, y, z];
                    block.position = new int3(x, y, z);
                    block.map = map;
                    block.GetNeighbourData();

                    //var rend = block.GetComponent<Renderer>();
                    //if (map.grid[x, y, z] != 0)
                    //{
                    //    MaterialPropertyBlock renderBlock = new MaterialPropertyBlock();
                    //    rend.GetPropertyBlock(renderBlock);
                    //    renderBlock.SetColor("_Color", map.grid[x, y, z] < 0 ? mineColor : bonusColor);
                    //    rend.SetPropertyBlock(renderBlock);
                    //}

                    blocks[x, y, z] = block;
                }
    }

    private void RevealNeighbours(Block block)
    {
        var position = block.position;
        if (block.specialNeighbours.x != 0 || block.specialNeighbours.y != 0 || block.specialNeighbours.z != 0) return;

        //List<Block> checkNext = new List<Block>();
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (Mathf.Abs(x) == Mathf.Abs(y) && Mathf.Abs(x) == Mathf.Abs(z)) continue;
                    int3 pos = new int3(x + position.x, y + position.y, z + position.z);
                    if (pos.Equals(position) || !map.IsValidPosition(pos)) continue;
                    if (!blocks[pos.x, pos.y, pos.z].revealed) {
                        //Debug.Log(position + " is checking " + pos + " he has " + block.specialNeighbours + " and target is " + blocks[pos.x, pos.y, pos.z].value, block);
                        blocks[pos.x, pos.y, pos.z].CheckNeighbours();
                    }
                }

    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, completeSize);
    }
}
