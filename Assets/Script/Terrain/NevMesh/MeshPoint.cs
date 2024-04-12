using System;
using UnityEngine;

[Serializable]
public class MeshPoint{
    public Vector3 globalPos;
    public Vector3Int localPos;

    public float density;
    public int gCost;
    public int hCost;
    public bool good;
    [NonSerialized] public MeshPoint parent;
    [NonSerialized] public MeshPoint[] neighbors;

    public MeshPoint(Vector3 gPos, Vector3Int lpos, float d){
        globalPos = gPos;
        localPos = lpos;
        density = d;

        good = false;
    }

    public int getF(){
        return hCost + gCost;
    }

}
