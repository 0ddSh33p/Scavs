using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[Serializable]
public class PathMeshGenerator : EditorWindow
{
    private float density;
    public LayerMask obsticles;
    private Vector3 CornerA, CornerB;
    private MeshPoint[,,] meshGrid;

    
    [MenuItem("Window/NavMesh")]
    public static void ShowWindow(){
        GetWindow<PathMeshGenerator>("Nav Mesh");
    }

    private void buildMesh(){
        float numX, numY, numZ;
        
        numX = (CornerA.x - CornerB.x)/density;
        numY = (CornerA.y - CornerB.y)/density;
        numZ = (CornerA.z - CornerB.z)/density;
        obsticles = LayerMask.GetMask("Default", "Water");

        
        meshGrid = new MeshPoint[(int)(numX+1),(int)(numY+1),(int)(numZ+1)];
        Debug.Log("Created mesh with " + ((int) ((numX+1)*(numY+1)*(numZ+1))) + " points");


        for(int x = 0; x <= (int)numX; x++){
            for(int y = 0; y <= (int)numY; y++){
                for(int z = 0; z <= (int)numZ; z++){

                    
                    meshGrid[x,y,z] = new MeshPoint(new Vector3(x*density+CornerB.x, y*density+CornerB.y, z*density+CornerB.z), new Vector3Int(x,y,z),density);
                        
                    Collider[] temp = Physics.OverlapBox(meshGrid[x,y,z].globalPos, new Vector3(density/2,density/2,density/2));
                    if(temp.Length > 0){
                        foreach(Collider col in temp){
                           if(col.gameObject.layer != 3){
                                if((obsticles & (1 << col.gameObject.layer)) != 0){
                                    meshGrid[x,y,z].good = false;
                                    break;
                                }
                            }else{
                                meshGrid[x,y,z].good = true;
                            }
                        }   
                    }       
                }       
            }
        }          
        
        Debug.Log("Suceeded in creating a mesh");
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Undo.RegisterCreatedObjectUndo (go, "Created go");

        NavMeshInstance meshInstance = Undo.AddComponent<NavMeshInstance>(go);

        Undo.DestroyObjectImmediate (go.GetComponent<MeshRenderer>());
        Undo.DestroyObjectImmediate (go.GetComponent<MeshFilter>());
        Undo.DestroyObjectImmediate (go.GetComponent<BoxCollider>());

        Undo.RecordObject(go, "Updated Name");
        go.name = "Nav Mesh";
        
        meshInstance.dims = new Vector3Int((int)numX, (int)numY, (int)numZ);
        meshInstance.origin = CornerB;
        meshInstance.end = CornerA;
        meshInstance.mesh = meshGrid;
        meshInstance.recivedData = true;

        Debug.Log("Done!");
    }

    void OnGUI(){
        GUILayout.Label("Mesh Bounds:", EditorStyles.boldLabel);
            GUILayout.Label("\nTop Front Right Bound");
            EditorGUILayout.BeginHorizontal();
            Undo.RecordObject(this, "New Pos A");
            float.TryParse(GUILayout.TextField(CornerA.x + ""), out CornerA.x); 
            float.TryParse(GUILayout.TextField(CornerA.y + ""), out CornerA.y); 
            float.TryParse(GUILayout.TextField(CornerA.z + ""), out CornerA.z); 
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Bottom Back Left Bound");
            EditorGUILayout.BeginHorizontal();
            Undo.RecordObject(this, "New Pos B");
            float.TryParse(GUILayout.TextField(CornerB.x + ""), out CornerB.x); 
            float.TryParse(GUILayout.TextField(CornerB.y + ""), out CornerB.y); 
            float.TryParse(GUILayout.TextField(CornerB.z + ""), out CornerB.z); 
        EditorGUILayout.EndHorizontal();


        GUILayout.Label("\n\nMesh Settings:", EditorStyles.boldLabel);

        GUILayout.Label("\nVoxel size");
            GUILayout.Label(density + "");
            Undo.RecordObject(this, "New Density");
            density = GUILayout.HorizontalSlider(density,0f,10f);

        GUILayout.Label("\n\nBuild:", EditorStyles.boldLabel);
        if(GUILayout.Button("Build NavMesh")){
            Debug.Log("Building Mesh...");
            buildMesh();
        }

    }
}
