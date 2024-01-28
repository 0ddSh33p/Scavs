using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private Vector2 size, ofeset;
    [SerializeField] private GameObject chunk;

    void Start(){
        for(int x = 0; x<size.x; x++){
            for(int y = 0; y<size.y; y++){
                Instantiate(chunk, new Vector3(x*ofeset.x,0,y*ofeset.y), Quaternion.identity);
            }
        }
    }
}
