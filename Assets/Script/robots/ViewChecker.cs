using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ViewChecker : MonoBehaviour
{
    [HideInInspector] public GameObject possiblePlayer;
    [SerializeField] private MeshRenderer visible;

    void Awake(){
        visible.enabled = false;
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            possiblePlayer = other.gameObject;
        }
    }
    
    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            possiblePlayer = null;
        }
    }
}
