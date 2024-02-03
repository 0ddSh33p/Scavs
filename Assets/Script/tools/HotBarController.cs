using System.Collections;
using Unity.Netcode;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class HotBarController : NetworkBehaviour
{
    [SerializeField] private GameObject[] tools;
    private int onTool = 0;

    void Update(){
        if (!IsOwner) return;
        for(int i = 0; i<tools.Length; i++){
            if(i == onTool){
                tools[i].SetActive(true);
            }
            else{
                tools[i].SetActive(false);
            }
        }
        
        if(!Input.GetMouseButton(1)){
            if(Input.mouseScrollDelta.y > 0.1f){
                onTool ++;
            } else if(Input.mouseScrollDelta.y < -0.1f){
                onTool --;
            }
            if(onTool >= tools.Length){
                onTool = 0;
            } else if( onTool < 0){
                onTool = tools.Length -1;
            }
        }
    }
}
