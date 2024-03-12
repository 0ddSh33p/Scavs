using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshPoint{
    public Vector3 globalPos{get;}
    public Vector3Int localPos{get;}

    public float density{get;}
    public int gCost;
    public int hCost;
    public int fCost{get{return gCost+hCost;}}
    public bool good;
    public MeshPoint parent;
    public MeshPoint[] neighbors{get;}

    public MeshPoint(Vector3 gPos, Vector3Int lpos, float d){
        globalPos = gPos;
        localPos = lpos;
        density = d;

        good = true;
    }

}
