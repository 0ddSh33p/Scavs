using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class PathMeshGenerator : EditorWindow
{
    private float minDensity = 5, maxDensity = 2, levels = 4;
    private Vector3 CornerA = new Vector3(10,10,10), CornerB;
    private List<MeshPoint>[] meshPoints;

    
    [MenuItem("Window/NavMesh")]
    public static void ShowWindow(){
        GetWindow<PathMeshGenerator>("Nav Mesh");
    }

    private void buildMesh(){
        meshPoints = new List<MeshPoint>[(int)levels+1];


        for(int l = 0; l <= (int)levels; l++){
            float d = ((float)l/((int)levels) * minDensity) + (Mathf.Abs((float)l/((int)levels) - 1) * maxDensity), numX, numY, numZ;
            
            numX = (CornerA.x - CornerB.x)/d;
            numY = (CornerA.y - CornerB.y)/d;
            numZ = (CornerA.z - CornerB.z)/d;

            meshPoints[l] = new List<MeshPoint>();

            Debug.Log("Created mesh level " + l + " with " + ((int) ((numX+1)*(numY+1)*(numZ+1))) + " points");
            int c = 0;

            for(int x = 0; x <= numX; x++){
                for(int y = 0; y <= numY; y++){
                    for(int z = 0; z <= numZ; z++){

                        meshPoints[l].Add(new MeshPoint(l,new Vector3(x*d+CornerB.x, y*d+CornerB.y, z*d+CornerB.z),d));

                        if(l==0){
                            Collider[] temp = Physics.OverlapBox(meshPoints[l][^1].getGlobalPos(), new Vector3(d/2,d/2,d/2));
                            if(temp.Length > 0){
                                foreach(Collider col in temp){
                                    if(col.gameObject.layer != 3){
                                        meshPoints[l][^1].setOpen(-1);
                                    } else {
                                        meshPoints[l][^1].setOpen(1);
                                    }
                                }
                                
                            } else {
                                meshPoints[l][meshPoints[l].Count - 1].setOpen(-1);
                            }
                        } else {
                            for(int o = 0; o < meshPoints[0].Count; o++){
                                if(Mathf.Abs(meshPoints[0][o].getGlobalPos().x - meshPoints[l][c].getGlobalPos().x) < d/2 &&
                                Mathf.Abs(meshPoints[0][o].getGlobalPos().y - meshPoints[l][c].getGlobalPos().y) < d/2 &&
                                Mathf.Abs(meshPoints[0][o].getGlobalPos().z - meshPoints[l][c].getGlobalPos().z) < d/2){
                                    meshPoints[l][c].setOpen(meshPoints[l][c].getOpen() + 1);
                                }
                            }
                            if(meshPoints[l][c].getOpen() >= d/meshPoints[0][0].getDensity() -1.5f){
                                meshPoints[l][c].setOpen(1);
                            } else {
                                meshPoints[l][c].setOpen(-1);
                            }
                        }
                        c++;
                    }       
                }
            }
            //clean layer
            for(int i = meshPoints[l].Count -1; i >= 0; i--){
                if(meshPoints[l][i].getOpen() < 0){
                    meshPoints[l].RemoveAt(i);
                }
            }            
        }
        Debug.Log("Suceeded in creating a mesh");
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        NavMeshInstance meshInstance = go.AddComponent<NavMeshInstance>();
        DestroyImmediate(go.GetComponent<MeshRenderer>());
        DestroyImmediate(go.GetComponent<MeshFilter>());
        DestroyImmediate(go.GetComponent<BoxCollider>());

        go.name = "Nav Mesh";
        meshInstance.myVerts = meshPoints;
        meshInstance.recivedData = true;
        meshInstance.updateVals();
        meshInstance = null;

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
