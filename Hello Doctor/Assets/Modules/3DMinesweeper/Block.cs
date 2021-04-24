using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Block : MonoBehaviour
{
    public enum Axis { X,Y,Z};

    public static UnityEvent<Block> OnBlockClicked = new UnityEvent<Block>();
    public static UnityEvent<Block> OnBlockRevealed = new UnityEvent<Block>();
    public int value;
    [System.NonSerialized]
    public int3 position;

    [System.NonSerialized]
    public Map map;
    public MeshFilter[] Xs, Ys, Zs;

    public MeshLookup lookup;
    public bool revealed;

    //[System.NonSerialized]
    public int3 specialNeighbours;

    private void OnMouseDown()
    {
        OnBlockClicked.Invoke(this);
        if (value != 0) Debug.LogWarning("We clicked on a " + value, this);
    }

    public void Reveal()
    {
        if (revealed) SetInvisible();
        CheckNeighbours();
    }

    public void ChangeColor()
    {
        var rend = GetComponent<Renderer>();
        var block = new MaterialPropertyBlock();
        rend.GetPropertyBlock(block);
        block.SetColor("_Color", Random.ColorHSV());
        rend.SetPropertyBlock(block);
    }

    public void SetInvisible()
    {
        var collider = GetComponent<Collider>();
        collider.enabled = false;
        var rend = GetComponent<Renderer>();
        rend.enabled = false;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    public void GetNeighbourData()
    {
        GetNeighbourData(Axis.Z);
        GetNeighbourData(Axis.Y);
        GetNeighbourData(Axis.X);
    }

    public void CheckNeighbours()
    {
        //Debug.Log(position + " im doing it.", this);
        
        foreach (var mf in Zs)
            mf.sharedMesh = lookup.GetNumberMesh(specialNeighbours.z);
        
        foreach (var mf in Ys)
            mf.sharedMesh = lookup.GetNumberMesh(specialNeighbours.y);
        
        foreach (var mf in Xs)
            mf.sharedMesh = lookup.GetNumberMesh(specialNeighbours.x);
        revealed = true;
        if (specialNeighbours.Equals(int3.zero)) SetInvisible();
        OnBlockRevealed.Invoke(this);
    }

    private void GetNeighbourData(Axis axis)
    {
        switch (axis)
        {
            case Axis.Z:
                //checking XY neighbours
                specialNeighbours.z = 0;
                for (int x = -1; x <= 1; x++)
                    for (int y = -1; y <= 1; y++)
                    {
                        int3 pos = new int3(x + position.x, y + position.y, position.z);
                        if (x == 0 && y == 0) continue;
                        else if (!map.IsValidPosition(pos)) continue;
                        if (map.grid[pos.x, pos.y, pos.z] != 0) specialNeighbours.z++;
                    }
                break;
            case Axis.Y:
                //checking XZ neighbours
                specialNeighbours.y = 0;
                for (int x = -1; x <= 1; x++)
                    for (int z = -1; z <= 1; z++)
                    {
                        int3 pos = new int3(x + position.x, position.y, z + position.z);
                        if (x == 0 && z == 0) continue;
                        else if (!map.IsValidPosition(pos)) continue;
                        if (map.grid[pos.x, pos.y, pos.z] != 0) specialNeighbours.y++;
                    }
                break;
            case Axis.X:
                //checking YZ neighbours
                specialNeighbours.x = 0;
                for (int y = -1; y <= 1; y++)
                    for (int z = -1; z <= 1; z++)
                    {
                        int3 pos = new int3(position.x, y + position.y, z + position.z);
                        if (y == 0 && z == 0) continue;
                        else if (!map.IsValidPosition(pos)) continue;
                        if (map.grid[pos.x, pos.y, pos.z] != 0) specialNeighbours.x++;
                    }
                break;
        }
    }
}
