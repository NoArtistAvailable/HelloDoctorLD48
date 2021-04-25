using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Block : MonoBehaviour
{
    //public enum Axis { X,Y,Z};

    public static UnityEvent<Block> OnBlockClicked = new UnityEvent<Block>();
    public static UnityEvent<Block> OnBlockRevealed = new UnityEvent<Block>();
    public int value;
    [System.NonSerialized]
    public int3 position;

    [System.NonSerialized]
    public Map map;
    //public MeshFilter[] Xs, Ys, Zs;
    public MeshFilter numberFilter;

    public MeshLookup lookup;
    public bool revealed;
    public bool isVisible = true;
    public bool flagged = false;

    //[System.NonSerialized]
    public int specialNeighbours;

    private void OnMouseDown()
    {
        OnBlockClicked.Invoke(this);
        if (value != 0) Debug.LogWarning("We clicked on a " + value, this);
    }

    //private void Start()
    //{
    //    ShowColor();
    //}

    public void Reveal()
    {
        //Debug.Log("RevealState: " + revealed, this);
        if (value == 0)
        {
            //if (revealed) 
            SetInvisible();
            CheckNeighbours();
        }
        else if( value != 0)
        {
            ShowColor();
            if (revealed) PropagateValueToNeighbours(-1, 0.5f);
            revealed = true;
        }
    }

    public void ShowColor()
    {
        Color col = Vector4.zero;
        if (value == 0) col = MineSweeperGame.Instance.colorRegular;
        else if (value < 0) col = MineSweeperGame.Instance.colorMine;
        else if (value > 0) col = MineSweeperGame.Instance.colorBonus;

        var rend = GetComponent<Renderer>();
        var matBlock = new MaterialPropertyBlock();
        rend.GetPropertyBlock(matBlock);
        matBlock.SetColor("_Color", col);
        rend.SetPropertyBlock(matBlock);
    }

    public void SetInvisible()
    {
        isVisible = false;
        var collider = GetComponent<Collider>();
        collider.enabled = false;
        var rend = GetComponent<Renderer>();
        rend.enabled = false;
        NumberVisibility();
        MapGenerator.Instance.DoForeachNeighbour(position, (neighbour) =>
        {
            neighbour.NumberVisibility();
        });
    }

    public void NumberVisibility()
    {
        bool vis = HasVisibleNeighbours();
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(vis);
    }

    public bool HasVisibleNeighbours()
    {
        bool visible = false;
        MapGenerator.Instance.DoForeachNeighbour(position, (neighbour) =>
        {
            visible |= neighbour.isVisible;
        });
        return visible;
    }

    public void SetVisible()
    {
        isVisible = true;
        var collider = GetComponent<Collider>();
        collider.enabled = true;
        var rend = GetComponent<Renderer>();
        rend.enabled = true;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
        GetNeighbourData();
        RefreshNumberMeshes();
    }

    public void ChangeValue(int delta)
    {
        var oldValue = value;
        value += delta;
        value = Mathf.Clamp(value, -1, 1);
        MapGenerator.Instance.map.grid[position.x, position.y, position.z] = value;
        GetNeighbourData();
        if((oldValue == 0 && value != 0) || (oldValue != 0 && value == 0))
        {
            MapGenerator.Instance.DoForeachNeighbour(position, (neighbour) => {
                neighbour.GetNeighbourData();
                if (neighbour.revealed) neighbour.RefreshNumberMeshes();
            });
        }
        if (revealed)
        {
            CheckNeighbours();
            if (value != 0 && !GetComponent<Renderer>().enabled) SetVisible();
            ShowColor();
        }
    }

    public void SetValue(int val)
    {
        var oldValue = value;
        value = val;
        MapGenerator.Instance.map.grid[position.x, position.y, position.z] = value;
        GetNeighbourData();
        if (oldValue != 0 )
        {
            MapGenerator.Instance.DoForeachNeighbour(position, (neighbour) => {
                neighbour.GetNeighbourData();
                if (neighbour.revealed) neighbour.RefreshNumberMeshes();
            });
        }
        Reveal();
    }

    public void SetValueOnSelfAndNeighbours(int val)
    {
        SetValue(val);
        MapGenerator.Instance.DoForeachNeighbour(position, (neigh)=>neigh.SetValue(val));
    }

    public void SetValueAndPropagateToSame(int val)
    {
        if (val == value) return;
        int oldValue = value;
        SetValue(val);
        MapGenerator.Instance.DoForeachNeighbour(position, (neigh) =>
        {
            //Debug.Log("NValue: " + neigh.value + " checkValue: " + oldValue);
            if (neigh.value == oldValue) neigh.SetValueAndPropagateToSame(val);
        });
    }

    public void PropagateValueToNeighbours(int delta, float chance = 1f)
    {
        MapGenerator.Instance.DoForeachNeighbour(position, (neighbour) =>
        {
            if(MapGenerator.Instance.random.NextFloat() <= chance)
                neighbour.ChangeValue(delta);
        });
    }

    public void RefreshNumberMeshes()
    {
        numberFilter.sharedMesh = lookup.GetNumberMesh(specialNeighbours);
        //foreach (var mf in Zs)
        //    mf.sharedMesh = lookup.GetNumberMesh(specialNeighbours.z);

        //foreach (var mf in Ys)
        //    mf.sharedMesh = lookup.GetNumberMesh(specialNeighbours.y);

        //foreach (var mf in Xs)
        //    mf.sharedMesh = lookup.GetNumberMesh(specialNeighbours.x);
    }

    public void CheckNeighbours()
    {
        //Debug.Log(position + " im doing it.", this);
        RefreshNumberMeshes();
        revealed = true;
        SetInvisible();
        if (specialNeighbours.Equals(int3.zero)) SetInvisible();
        OnBlockRevealed.Invoke(this);
    }

    public void GetNeighbourData()
    {
        specialNeighbours = 0;
        MapGenerator.Instance.DoForeachNeighbourData(position, (data) =>
        {
            if (data != 0) specialNeighbours++;
            return data;
        });

        //switch (axis)
        //{
        //    case Axis.Z:
        //        //checking XY neighbours
        //        specialNeighbours.z = 0;
        //        for (int x = -1; x <= 1; x++)
        //            for (int y = -1; y <= 1; y++)
        //            {
        //                int3 pos = new int3(x + position.x, y + position.y, position.z);
        //                if (x == 0 && y == 0) continue;
        //                else if (!map.IsValidPosition(pos)) continue;
        //                if (map.grid[pos.x, pos.y, pos.z] != 0) specialNeighbours.z++;
        //            }
        //        break;
        //    case Axis.Y:
        //        //checking XZ neighbours
        //        specialNeighbours.y = 0;
        //        for (int x = -1; x <= 1; x++)
        //            for (int z = -1; z <= 1; z++)
        //            {
        //                int3 pos = new int3(x + position.x, position.y, z + position.z);
        //                if (x == 0 && z == 0) continue;
        //                else if (!map.IsValidPosition(pos)) continue;
        //                if (map.grid[pos.x, pos.y, pos.z] != 0) specialNeighbours.y++;
        //            }
        //        break;
        //    case Axis.X:
        //        //checking YZ neighbours
        //        specialNeighbours.x = 0;
        //        for (int y = -1; y <= 1; y++)
        //            for (int z = -1; z <= 1; z++)
        //            {
        //                int3 pos = new int3(position.x, y + position.y, z + position.z);
        //                if (y == 0 && z == 0) continue;
        //                else if (!map.IsValidPosition(pos)) continue;
        //                if (map.grid[pos.x, pos.y, pos.z] != 0) specialNeighbours.x++;
        //            }
        //        break;
        //}
    }
}
