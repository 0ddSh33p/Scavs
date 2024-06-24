using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGeneratorV2 : MonoBehaviour
{
    Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private seedGen mySeed;
    [HideInInspector] public Vector2 perlinScale;


    void Start()
    {
        mesh = new Mesh();

        mySeed = GameObject.FindGameObjectWithTag("WorldSeed").GetComponent<seedGen>();
        

        CreateNewMap();         
    }

    public void CreateNewMap()
    {
        perlinScale = mySeed.perlinScale/100;
        CreateMeshShape();
        CreateTriangles();
        UpdateMesh();
    }

    private void CreateMeshShape ()
    {
        float perlinAtPos, macroPerlinPos;
        vertices = new Vector3[(mySeed.size.x + 1) * (mySeed.size.y + 1)];
        uvs = new Vector2[(mySeed.size.x + 1) * (mySeed.size.y + 1)];
        for (int i = 0, z = 0; z <= mySeed.size.y; z++)
        {
            for (int x = 0; x <= mySeed.size.x; x++)
            {
                perlinAtPos = Mathf.PerlinNoise((x+transform.position.x+mySeed.seed.x)*perlinScale.x /mySeed.trueScale.x, 
                                                (z+transform.position.z+mySeed.seed.y)*perlinScale.y /mySeed.trueScale.y);

                macroPerlinPos = Mathf.PerlinNoise((x+transform.position.x+ (mySeed.seed.x/2))/mySeed.macroScale.x,
                                                   (z+transform.position.z+(mySeed.seed.y/2))/mySeed.macroScale.y);

                vertices[i] = new Vector3(x, (Mathf.Sin(perlinAtPos * mySeed.perlinPercent)*mySeed.height) + (macroPerlinPos*mySeed.macroHeight), z);
                uvs[i] = new Vector2(x/(float)mySeed.size.x, z/(float)mySeed.size.y);
                i++;
            }
        }
    }
    private void CreateTriangles() 
    {
        // Need 6 vertices to create a square (2 triangles)
        triangles = new int[mySeed.size.x * mySeed.size.y * 6];
        int vert = 0;
        int tris = 0;

        // loop through rows
        for (int z = 0; z < mySeed.size.y; z++)
        {
            // fill all columns in row
            for (int x = 0; x < mySeed.size.x; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + mySeed.size.x + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + mySeed.size.x + 1;
                triangles[tris + 5] = vert + mySeed.size.x + 2;

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
        mySeed.meshLocations.Add(myloc);
        mySeed.addPOI(transform.position);
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
