using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class characterCustomizer : MonoBehaviour
{
    [SerializeField] private GameObject me;
    private Rigidbody rb;

    [SerializeField] private Mesh[] BaseMeshes;
    [SerializeField] private MeshFilter myBaseMesh;
    [SerializeField] private TMP_Dropdown physique;
    [SerializeField] private Material skinColor;


    void Awake(){
        rb = me.GetComponent<Rigidbody>();
    }

    [SerializeField] private float speedS, speedD, screenDiv;
    public void updateModel(){
        myBaseMesh.mesh = BaseMeshes[physique.value];
    }

    void Update(){
        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).z < screenDiv){
            if(Input.GetMouseButton(0)){
                rb.angularVelocity = new Vector3(0,0,0);
                me.transform.Rotate(0,-Input.GetAxis("Mouse X") * Time.deltaTime * speedS, 0);
            }
            if(Input.GetMouseButtonUp(0)){
                rb.angularVelocity = new Vector3(0, -Input.GetAxis("Mouse X") * speedD, 0);
            }
        } 
    }
}
