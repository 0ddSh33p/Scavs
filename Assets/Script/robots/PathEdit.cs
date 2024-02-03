using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PathEdit : MonoBehaviour
{
    public List<Vector3> myPath = new List<Vector3>();
    [SerializeField] private bool ClearAllPoints = false;

    void Update(){
        if(!Application.isPlaying && ClearAllPoints){
            myPath = new List<Vector3>();
            ClearAllPoints = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        if(myPath.Count > 1){
            for(int i = 0; i<myPath.Count - 1; i++){
                Gizmos.DrawLine(myPath[i],myPath[i+1]);
            }
            Gizmos.DrawLine(myPath[myPath.Count-1],myPath[0]);
        }
    }
}
