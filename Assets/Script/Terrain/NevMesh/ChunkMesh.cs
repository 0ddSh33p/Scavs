using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMesh : MonoBehaviour
{
    [SerializeField] private float density;
    public LayerMask obsticles;
    [HideInInspector] public Vector3 CornerA, CornerB;
    private MeshPoint[,,] meshGrid;


    public void buildMesh(){
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

        NavMeshInstance meshInstance = gameObject.AddComponent<NavMeshInstance>();
        
        meshInstance.dims = new Vector3Int((int)numX, (int)numY, (int)numZ);
        meshInstance.origin = CornerB;
        meshInstance.end = CornerA;
        meshInstance.mesh = meshGrid;
        meshInstance.recivedData = true;

        Debug.Log("Done!");
    }
}
