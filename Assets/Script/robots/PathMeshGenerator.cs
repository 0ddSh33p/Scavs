using UnityEngine;
using UnityEditor;

public class PathMeshGenerator : EditorWindow
{
    private float minDensity = 5, maxDensity = 2, levels = 4;
    private Vector3 CornerA = new Vector3(10,10,10), CornerB;
    private Vector3[][] meshVerts, sizes;
    
    [MenuItem("Window/NavMesh")]
    public static void ShowWindow(){
        GetWindow<PathMeshGenerator>("Nav Mesh");
    }

    private void buildMesh(){
        meshVerts = new Vector3[(int)levels+1][];
        sizes = new Vector3[(int)levels+1][];

        for(int l = 0; l <= (int)levels; l++){
            float d = ((float)l/((int)levels) * minDensity) + (Mathf.Abs((float)l/((int)levels) - 1) * maxDensity), numX, numY, numZ;
            
            numX = (CornerA.x - CornerB.x)/d;
            numY = (CornerA.y - CornerB.y)/d;
            numZ = (CornerA.z - CornerB.z)/d;

            meshVerts[l] = new Vector3[(int) ((numX+1)*(numY+1)*(numZ+1))];
            sizes[l] = new Vector3[(int) ((numX+1)*(numY+1)*(numZ+1))];
            Debug.Log("Created mesh level " + l + " with " + meshVerts[l].Length + " points");
            int c = 0;

            for(int x = 0; x <= numX; x++){
                for(int y = 0; y <= numY; y++){
                    for(int z = 0; z <= numZ; z++){

                        meshVerts[l][c] = new Vector3(x*d+CornerB.x, y*d+CornerB.y, z*d+CornerB.z);
                        sizes[l][c] = new Vector3(d,d,d);
                        c++;
                    }       
                }
            }
            Debug.Log("Suceeded in creating a mesh");

        }
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        NavMeshInstance meshInstance = go.AddComponent<NavMeshInstance>();
        DestroyImmediate(go.GetComponent<MeshRenderer>());
        DestroyImmediate(go.GetComponent<MeshFilter>());
        DestroyImmediate(go.GetComponent<BoxCollider>());

        go.name = "Nav Mesh";
        meshInstance.myVerts = meshVerts;
        meshInstance.dimes = sizes;
        meshInstance.recivedData = true;
        Debug.Log("Done!");
    }

    void OnGUI(){
        GUILayout.Label("Mesh Bounds:", EditorStyles.boldLabel);
            GUILayout.Label("\nTop Front Right Bound");
            EditorGUILayout.BeginHorizontal();
            float.TryParse(GUILayout.TextField(CornerA.x + ""), out CornerA.x); 
            float.TryParse(GUILayout.TextField(CornerA.y + ""), out CornerA.y); 
            float.TryParse(GUILayout.TextField(CornerA.z + ""), out CornerA.z); 
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Bottom Back Left Bound");
            EditorGUILayout.BeginHorizontal();
            float.TryParse(GUILayout.TextField(CornerB.x + ""), out CornerB.x); 
            float.TryParse(GUILayout.TextField(CornerB.y + ""), out CornerB.y); 
            float.TryParse(GUILayout.TextField(CornerB.z + ""), out CornerB.z); 
        EditorGUILayout.EndHorizontal();


        GUILayout.Label("\n\nMesh Settings:", EditorStyles.boldLabel);
        GUILayout.Label("\nMesh Levels " + ((int)levels + 1));
        levels = GUILayout.HorizontalSlider(levels,1f,100f);
        GUILayout.Label("\nSubdivisions Per Unit Minimum to Maximum");

        EditorGUILayout.BeginHorizontal();
            GUILayout.Label((int)minDensity + "");
            GUILayout.Label(maxDensity + "");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
            minDensity = GUILayout.HorizontalSlider(minDensity,1f,200f);
            maxDensity = GUILayout.HorizontalSlider(maxDensity,0.01f,5f);
        EditorGUILayout.EndHorizontal();


        GUILayout.Label("\n\nBuild:", EditorStyles.boldLabel);
        if(GUILayout.Button("Build NavMesh")){
            Debug.Log("Building Mesh...");
            buildMesh();
        }
    }

    

}
