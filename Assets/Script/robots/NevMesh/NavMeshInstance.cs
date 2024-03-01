using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NavMeshInstance : MonoBehaviour
{
    [HideInInspector] public List<MeshPoint>[] myVerts;
    [HideInInspector] public bool recivedData;
    [SerializeField] private int layer = 0, specI = -1;
    [SerializeField] private bool viewAll = false;

    public void updateVals(){
        Array.Copy(myVerts,myVerts,myVerts.Length);
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = new Color(0.9f,0.25f,1.0f,0.30f);
        if(recivedData){
            for(int i = 0; i < myVerts.Length; i++){
                if(viewAll || layer == i){

                    for(int j =0; j<myVerts[i].Count; j++){
                        if(specI == j || 0>specI){
                            Gizmos.DrawCube(myVerts[i][j].getGlobalPos(), new Vector3(myVerts[i][j].getDensity(),myVerts[i][j].getDensity(),myVerts[i][j].getDensity()));
                        }
                    }
                }
            }
        }
    }
}
