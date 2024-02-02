using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Flashlight : NetworkBehaviour
{
    [SerializeField] private float minSize =1, maxSize =1, changeSpeed = 2;
    [SerializeField] private Light lit;

    void Update(){
        if (!IsOwner) return;
        if(Input.GetMouseButton(1) && Input.mouseScrollDelta.y < 0 && lit.innerSpotAngle < maxSize){
            lit.intensity -= changeSpeed/2;
            lit.range -= changeSpeed;
            lit.innerSpotAngle += changeSpeed;
            lit.spotAngle += changeSpeed;
        }
        else if(Input.GetMouseButton(1) && Input.mouseScrollDelta.y > 0 && lit.innerSpotAngle > minSize){
            lit.intensity += changeSpeed/2;
            lit.range += changeSpeed;
            lit.innerSpotAngle -= changeSpeed;
            lit.spotAngle -= changeSpeed;
        }
    }
}
