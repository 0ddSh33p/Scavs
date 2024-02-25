using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NavMeshInstance : MonoBehaviour
{
    [HideInInspector] public Vector3[][] myVerts, dimes;

    [HideInInspector] public bool recivedData;
    [SerializeField] private int layer = 0, specI = -1;
    [SerializeField] private bool viewAll = false;


    void OnDrawGizmosSelected(){
        if(recivedData){
            for(int i = 0; i < myVerts.Length; i++){
                if(viewAll || layer == i){
                    Gizmos.color = Color.green;
                    for(int j =0; j<myVerts[i].Length; j++){
                        if(specI == j || 0>specI){
                            Gizmos.DrawCube(myVerts[i][j], dimes[i][j]);
                        }
                    }
                }
            }
        }
    }
}
