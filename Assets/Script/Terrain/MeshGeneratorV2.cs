using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class MeshGeneratorV2 : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private seedGen mySeed;
    [HideInInspector] public Vector2 perlinScale;

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    void Start()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mySeed = GameObject.FindGameObjectWithTag("WorldSeed").GetComponent<seedGen>();

        CreateNewMap();
    }

    public void CreateNewMap()
    {
        perlinScale = mySeed.perlinScale / 100;
        CreateMeshShape();
        CreateTriangles();
        UpdateMesh();
    }

    private void CreateMeshShape()
    {
        int width = mySeed.size.x;
        int height = mySeed.size.y;
        int vertexCount = (width + 1) * (height + 1);

        vertices = new Vector3[vertexCount];
        uvs = new Vector2[vertexCount];

        float posX = transform.position.x;
        float posZ = transform.position.z;
        float seedX = mySeed.seed.x;
        float seedY = mySeed.seed.y;
        float perlinPercent = mySeed.perlinPercent;
        float heightScale = mySeed.height;
        float macroHeight = mySeed.macroHeight;
        Vector2 trueScale = mySeed.trueScale;
        Vector2 macroScale = mySeed.macroScale;

        for (int i = 0, z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                float perlinAtPos = Mathf.PerlinNoise((x + posX + seedX) * perlinScale.x / trueScale.x,
                                                      (z + posZ + seedY) * perlinScale.y / trueScale.y);
                float macroPerlinPos = Mathf.PerlinNoise((x + posX + (seedX / 2)) / macroScale.x,
                                                         (z + posZ + (seedY / 2)) / macroScale.y);

                vertices[i] = new Vector3(x, Mathf.Sin(perlinAtPos * perlinPercent) * heightScale + macroPerlinPos * macroHeight, z);
                uvs[i] = new Vector2(x / (float)width, z / (float)height);
            }
        }
    }

    private void CreateTriangles()
    {
        int width = mySeed.size.x;
        int height = mySeed.size.y;
        int triangleCount = width * height * 6;

        triangles = new int[triangleCount];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++, vert++, tris += 6)
            {
                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + width + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + width + 1;
                triangles[tris + 5] = vert + width + 2;
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

        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;

        Vector2Int myloc = new Vector2Int((int)(transform.position.x + mySeed.transform.position.x) / mySeed.size.x,
                                          (int)(transform.position.z + mySeed.transform.position.z) / mySeed.size.y);
        mySeed.PlaceVector2Int(myloc);
        mySeed.addPOI(transform.position);
    }
}