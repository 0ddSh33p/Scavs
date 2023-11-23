using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private float minSize =1, maxSize =1, changeSpeed = 2;
    private Light lit;

    void Awake(){
        lit = gameObject.GetComponent<Light>();
    }

    void Update(){
        if(Input.mouseScrollDelta.y < 0 && lit.innerSpotAngle < maxSize){
            lit.intensity -= changeSpeed/2;
            lit.range -= changeSpeed;
            lit.innerSpotAngle += changeSpeed;
            lit.spotAngle += changeSpeed;
        }
        else if(Input.mouseScrollDelta.y > 0 && lit.innerSpotAngle > minSize){
            lit.intensity += changeSpeed/2;
            lit.range += changeSpeed;
            lit.innerSpotAngle -= changeSpeed;
            lit.spotAngle -= changeSpeed;
        }
    }
}
