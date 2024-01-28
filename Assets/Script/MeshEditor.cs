using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MeshEditor : MonoBehaviour
{
    Mesh m;
    [SerializeField] private Transform[] p;
    [SerializeField] private Vector3 maxDists;

    void Start()
    {
        m = new Mesh
        {
            vertices = GetComponent<MeshFilter>().mesh.vertices,
            triangles = GetComponent<MeshFilter>().mesh.triangles,
            uv = GetComponent<MeshFilter>().mesh.uv,
            normals = GetComponent<MeshFilter>().mesh.normals,
        };
        m.MarkDynamic();
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int j = 0; j < p.Length; j++){
            for(int i = 0; i < m.vertices.Length; i++){
                Vector3 space = new Vector3(Mathf.Abs(p[j].position.x - m.vertices[i].x),Mathf.Abs(p[j].position.y - m.vertices[i].y),Mathf.Abs(p[j].position.z - m.vertices[i].z));
                if(space.x < maxDists.x && space.y < maxDists.y && space.z < maxDists.z){
                    m.vertices[i] -= new Vector3(0,5,0);
                }
            }
        }
    }
}
