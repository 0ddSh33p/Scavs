using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPoint{
    private Vector3 globalPos;
    private float density, open;
    private int level;

    public MeshPoint(int l, Vector3 gPos, float d){
        level = l;
        globalPos = gPos;
        density = d;
        open = 0f;
    }

    public Vector3 getGlobalPos(){
        return globalPos;
    }
    public float getDensity(){
        return density;
    }
    public int getLevel(){
        return level;
    }

    public void setOpen(float num){
        open = num;
    }
    public float getOpen(){
        return open;
    }

}
