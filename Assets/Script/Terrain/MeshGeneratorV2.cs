using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGeneratorV2 : MonoBehaviour
{
    Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private Color[] colors;

    [SerializeField] private int xSize, zSize, height, passes;

    [SerializeField] private float scale, bDetail, sDetail, rDetail, eDetail, randomPercent;

    [HideInInspector] public Vector2 seed;
    private seedGen mySeed;

    [SerializeField] private ChunkMesh myMesh;
    [SerializeField] private ProceduralAddition myAdder;




    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mySeed = GameObject.FindGameObjectWithTag("WorldSeed").GetComponent<seedGen>();

        seed = mySeed.seed;

        myAdder.center = gameObject.transform.position + (new Vector3(xSize * scale,0,zSize * scale) / 2);
        myAdder.dims = new Vector3(xSize * scale,0,zSize * scale);
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
                float rand = Mathf.PerlinNoise((x+seed.x) /rDetail,(z+seed.y) /rDetail) * (randomPercent/10);
                float sinVer = (Mathf.Sin(i/eDetail + rand) + 1) /2;
                sinVer *= sinVer;
                float y = (sinVer+GenerateNoiseHeight(z,x))/2;

                colors[i] = new Color(y,y,y);
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
        for (float pos = sDetail; pos < bDetail; pos += (bDetail - sDetail)/passes)
        {
            float mapZ = (z/pos) + seed.x;
            float mapX = (x/pos) + seed.y;

            // Create perlinValues  
            float perlinValue = Mathf.PerlinNoise(mapZ + transform.position.z/pos, mapX + transform.position.x/pos);
            noiseHeight += perlinValue * (1f - (pos/bDetail));
        }
        return noiseHeight / passes;
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

        gameObject.transform.localScale = new Vector3(scale, scale, scale);
        myMesh.CornerB = gameObject.transform.position;
        myMesh.CornerA = gameObject.transform.position + (new Vector3(xSize,height,zSize)*scale);
        myMesh.buildMesh();
    }
}