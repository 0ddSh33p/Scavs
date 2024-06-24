using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGeneratorV2 : MonoBehaviour
{
    Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private seedGen mySeed;
    private AnimationCurve slope;

    [HideInInspector] public float height, macroHeight, pPecent;
    [HideInInspector] public Vector2 seed, perlinScale, macroScale, trueScale;
    [HideInInspector] public Vector2Int size;


    void Start()
    {
        mesh = new Mesh();

        mySeed = GameObject.FindGameObjectWithTag("WorldSeed").GetComponent<seedGen>();
        

        CreateNewMap();         
    }

    public void CreateNewMap()
    {
        seed = mySeed.seed;
        size = mySeed.size;
        height = mySeed.height;
        pPecent = mySeed.perlinPercent;
        macroHeight = mySeed.macroHeight;
        macroScale = mySeed.macroScale;
        trueScale = mySeed.trueScale;
        perlinScale = mySeed.perlinScale/100;
        CreateMeshShape();
        CreateTriangles();
        UpdateMesh();
    }

    private void CreateMeshShape ()
    {
        float perlinAtPos, macroPerlinPos;
        vertices = new Vector3[(size.x + 1) * (size.y + 1)];
        uvs = new Vector2[(size.x + 1) * (size.y + 1)];
        for (int i = 0, z = 0; z <= size.y; z++)
        {
            for (int x = 0; x <= size.x; x++)
            {
                perlinAtPos = Mathf.PerlinNoise((x+transform.position.x+seed.x)*perlinScale.x /trueScale.x, 
                                                (z+transform.position.z+seed.y)*perlinScale.y /trueScale.y);
                macroPerlinPos = Mathf.PerlinNoise((x+transform.position.x+ (seed.x/2))/macroScale.x, (z+transform.position.z+(seed.y/2))/macroScale.y);


                vertices[i] = new Vector3(x, (Mathf.Sin(perlinAtPos * pPecent)*height) + (macroPerlinPos*macroHeight), z);
                uvs[i] = new Vector2(x/(float)size.x, z/(float)size.y);
                i++;
            }
        }
    }
    private void CreateTriangles() 
    {
        // Need 6 vertices to create a square (2 triangles)
        triangles = new int[size.x * size.y * 6];
        int vert = 0;
        int tris = 0;

        // loop through rows
        for (int z = 0; z < size.y; z++)
        {
            // fill all columns in row
            for (int x = 0; x < size.x; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + size.x + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + size.x + 1;
                triangles[tris + 5] = vert + size.x + 2;

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

        mesh.Optimize();
        
        Vector2Int myloc = new Vector2Int(((int)(transform.position.x + mySeed.transform.position.x))/mySeed.size.x,
                                          ((int)(transform.position.z + mySeed.transform.position.z))/mySeed.size.y);
        mySeed.addNewChunk(myloc);
        mySeed.meshLocations.Add(myloc);
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
