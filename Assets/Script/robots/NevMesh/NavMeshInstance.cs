using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NavMeshInstance : MonoBehaviour
{
    private bool recivedData;
    public MeshPoint[,,] mesh { get{return mesh;} set{ mesh = value; recivedData = true;} }
    public Vector3Int dims;
    public Vector3 origin, end;


    public MeshPoint findPoint(Vector3 pos){
        foreach(MeshPoint m in mesh){
            if(Mathf.Abs(m.globalPos.x - pos.x) < m.density/2&&
            Mathf.Abs(m.globalPos.y - pos.y) < m.density/2&&
            Mathf.Abs(m.globalPos.z - pos.z) < m.density/2){
                return m;
            }
        }
        Debug.LogError("AI not intersecting a Nav Mesh");
        return null;
    }

    public List<MeshPoint> getNeighbors(MeshPoint me){
        List<MeshPoint> neighbors = new List<MeshPoint>();

        for(int x = -1; x<= 1; x++){
            for(int y = -1; y<= 1; y++){
                for(int z = -1; z<= 1; z++){
                    if(x==0 && y ==0 && z ==0){
                        continue;
                    }
                    int cX = me.localPos.x + x;
                    int cY = me.localPos.y + y;
                    int cZ = me.localPos.z + z;

                    if(cX >=0 && cX <= dims.x  &&  cY >=0 && cY <= dims.y  &&  cZ >=0 && cZ <= dims.z){
                        neighbors.Add(mesh[cX,cY,cZ]);
                    }
                }
            }
        }
        
        return neighbors;
    }

    public List<MeshPoint> path;
    void OnDrawGizmosSelected(){
        Gizmos.color = new Color(0.9f,0.25f,1.0f,0.30f);
        if(recivedData){
            foreach(MeshPoint p in mesh){
                if(p.good){
                    if(path != null && path.Contains(p)){
                        Gizmos.color = new Color(1f,0.6f,1f,0.8f);
                    }
                    Gizmos.DrawCube(p.globalPos, new Vector3(p.density,p.density,p.density));   
                }
            }
        }
    }
}
