using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class coneView : MonoBehaviour
{
    [HideInInspector] public Transform player;
    [HideInInspector] public bool inView;

     private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6){
            inView = true;
            player = other.gameObject.transform;
        }
    }

}
