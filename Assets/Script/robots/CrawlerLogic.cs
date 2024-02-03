using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerLogic : MonoBehaviour
{
    [HideInInspector] public List<Vector3> myPath = new List<Vector3>();

    [SerializeField] private float maxPointDistance, speed;
    [SerializeField] private bool hasPathing;

    private Transform target;
    private Rigidbody rb;
    private int on = 1;
    private bool goodPath = false;

    void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    void Start(){
        if(hasPathing){
            myPath = GetComponent<PathEdit>().myPath;
            if(myPath.Count >= 2){
                transform.position = myPath[0];
                goodPath = true;
            } else {
                Debug.LogError("No Path Created for " + gameObject.name);
            }
        }
    }

    void Update(){

        if(hasPathing && goodPath){
            if(MathF.Abs(transform.position.x - myPath[on].x) < maxPointDistance && MathF.Abs(transform.position.z - myPath[on].z) < maxPointDistance){
                on++;
            }
            if(on >= myPath.Count) on = 0;
        }
    }

    void FixedUpdate(){
        if(hasPathing && goodPath){
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,calculateDirection(myPath[on],transform.position),transform.eulerAngles.z);
            rb.velocity = speed * transform.forward;
        }
    }

    private float calculateDirection(Vector3 to, Vector3 from){
        
        float x = to.x - from.x;
        float z = to.z - from.z;

        return Mathf.Rad2Deg*MathF.Atan2(x,z);
    }
}
