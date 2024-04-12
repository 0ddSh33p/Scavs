using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways][Serializable]
public class NavMeshInstance : MonoBehaviour
{
    public bool recivedData {private get; set;}
    public MeshPoint[,,] mesh;
    public List<MeshPoint> path;
    public Vector3Int dims;
    public Vector3 origin, end;
    


    public MeshPoint findPoint(Vector3 pos){
        MeshPoint nearest = mesh[0,0,0];
        float dist = 1000f;
        foreach(MeshPoint m in mesh){
            if((pos.x-m.globalPos.x)*(pos.x-m.globalPos.x) +
            (pos.y-m.globalPos.y)*(pos.y-m.globalPos.y) +
            (pos.z-m.globalPos.z)*(pos.z-m.globalPos.z) < dist){
                nearest = m;
            }
        }
        return nearest;
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

    void OnDrawGizmosSelected(){
        Gizmos.color = new Color(0.9f,0.25f,1.0f,0.30f);
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
