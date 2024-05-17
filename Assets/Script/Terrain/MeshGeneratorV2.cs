using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGeneratorV2 : MonoBehaviour
{
    Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private Color[] colors;

    [SerializeField] private int xSize, zSize, height;

    [SerializeField] private float bDetail, rDetail, eDetail, randomPercent;

    public Vector2 seed;
    private seedGen mySeed;


    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mySeed = GameObject.FindGameObjectWithTag("WorldSeed").GetComponent<seedGen>();
        if(seed.x == 0&&seed.y==0){
            seed = mySeed.seed;
        }
        CreateNewMap();         
    }
    public void CreateNewMap()
    {
        CreateMeshShape();
        CreateTriangles();
        UpdateMesh();
    }

    private void CreateMeshShape ()
    {
        // Creates seed

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        colors = new Color[(xSize + 1) * (zSize + 1)];
        uvs = new Vector2[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                // Assign and set height of each vertices
                float rand = Mathf.PerlinNoise((x+transform.position.x+seed.x) /rDetail,
                                               (z+transform.position.z+seed.y) /rDetail)        * (randomPercent/10);
                float sinVer = (Mathf.Sin(i/eDetail + rand) + 1) /2;

                float y = (sinVer);//+GenerateNoiseHeight(z,x))/2;

                colors[i] = new Color(y,y*0.6f,y*0.65f);
                vertices[i] = new Vector3(x, y*height, z);
                uvs[i] = new Vector2(x/(float)xSize, z/(float)zSize);
                i++;
            }
        }
    }


    private float GenerateNoiseHeight(int z, int x)
    {
        float noiseHeight = 0;

        // loop over passes
        float mapZ = ((z+ transform.position.z)/bDetail) + seed.y;
        float mapX = ((x+ transform.position.x)/bDetail) + seed.x;

        // Create perlinValues  
        float perlinValue = Mathf.PerlinNoise(mapX , mapZ);
        noiseHeight += perlinValue;

        return noiseHeight;
    }

    private void CreateTriangles() 
    {
        // Need 6 vertices to create a square (2 triangles)
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        // loop through rows
        for (int z = 0; z < xSize; z++)
        {
            // fill all columns in row
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = colors;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}