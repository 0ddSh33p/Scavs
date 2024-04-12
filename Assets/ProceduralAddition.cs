using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProceduralAddition : MonoBehaviour
{
    [SerializeField] private List<GameObject> players;
    [SerializeField] private float spawnRadius;
    [HideInInspector] public bool[] sides = {true,true,true,true};
    [HideInInspector] public Vector3 center;


    void Update(){
        foreach (GameObject player in players){

        }
    } 
}
