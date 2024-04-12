using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGeneratorV2 : MonoBehaviour
{
    Mesh mesh;
    [SerializeField] private AnimationCurve heightCurve;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;

    [SerializeField] private int xSize, zSize, height, passes;

    [SerializeField] private float scale, bDetail, sDetail, pointiness;

    [HideInInspector] public Vector2 seed;
    public seedGen mySeed;

    [SerializeField] private ChunkMesh myMesh;



    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        seed = mySeed.seed;
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
        uvs = new Vector2[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                // Assign and set height of each vertices
                float noiseHeightM = GenerateNoiseHeight(
                 (int) (Mathf.Abs(z + pointiness)-Mathf.Abs(z + pointiness)),x);
                float noiseHeightN = GenerateNoiseHeight(
                 z,(int) (Mathf.Abs(x + pointiness)-Mathf.Abs(x + pointiness)));

                float rawNoise = GenerateNoiseHeight(z,x);

                vertices[i] = new Vector3(x, rawNoise*(noiseHeightN/noiseHeightM+1), z);
                uvs[i] = new Vector2(x/(float)xSize, z/(float)zSize);
                i++;
            }
        }
    }


    private float GenerateNoiseHeight(int z, int x)
    {
        float noiseHeight = 0;

        // loop over passes
        for (float pos = sDetail; pos < bDetail; pos += (bDetail - sDetail)/passes)
        {
            float mapZ = (z/pos) + seed.x;
            float mapX = (x/pos) + seed.y;

            // Create perlinValues  
            float perlinValue = Mathf.PerlinNoise(mapZ + transform.position.z/pos, mapX + transform.position.x/pos);
            noiseHeight += heightCurve.Evaluate(perlinValue) * height * (1.00001f - (pos/bDetail)) / passes;
        }
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
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        GetComponent<MeshCollider>().sharedMesh = mesh;

        gameObject.transform.localScale = new Vector3(scale, scale, scale);
        myMesh.CornerB = gameObject.transform.position;
        myMesh.CornerA = gameObject.transform.position + (new Vector3(xSize,height*1.3f,zSize)*scale);
        myMesh.buildMesh();
    }
}