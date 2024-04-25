using System.Collections.Generic;
using UnityEngine;

public class ProceduralAddition : MonoBehaviour
{
    
    [SerializeField] private GameObject chunk;

    [SerializeField] private float minSpawnRadius, maxSpawnRadius;
    [HideInInspector] public Vector2[] sides = new Vector2[]
                                               {new Vector2(0,1) ,new Vector2(1,1) , new Vector2(1,0),
                                                new Vector2(1,-1), new Vector2(0,-1), new Vector2(-1,-1),
                                                new Vector2(-1,0), new Vector2(-1,1)};
    [HideInInspector] public Vector3 center, dims;
    private seedGen terrainMakerData;


    void Start(){
       terrainMakerData = GameObject.FindGameObjectWithTag("WorldSeed").GetComponent<seedGen>();
    }

    void Update(){
        foreach (GameObject player in terrainMakerData.players){
            float dist = Mathf.Pow(Mathf.Abs(player.transform.position.x - center.x),2) + Mathf.Pow(Mathf.Abs(player.transform.position.z - center.z),2);
            if(dist > (minSpawnRadius*minSpawnRadius) && dist < (maxSpawnRadius*maxSpawnRadius)){
                foreach (Vector2 side in sides){
                    if(side.x != 2 && side.y != 2){
                        Instantiate(chunk, gameObject.transform.position + new Vector3(dims.x * side.x,0,dims.z * side.y), Quaternion.identity);
                    }
                }
            }
        }
    } 
}
