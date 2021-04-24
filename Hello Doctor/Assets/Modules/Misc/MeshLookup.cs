using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshLookup : ScriptableObject
{
    public Mesh[] meshes;
    public MeshLookup lookup;

    public Mesh GetNumberMesh(int number)
    {
        return meshes[number];
    }
}
