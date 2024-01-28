using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerLogic : MonoBehaviour
{
    [HideInInspector] public List<Vector3> myPath = new List<Vector3>();

    [SerializeField] private float maxPointDistance, speed;
    [SerializeField] private coneView seen;

    private Transform target;
    private Rigidbody rb;
    private int on = 0;
    private bool goodPath = false;

    void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    void Start(){
        if(myPath.Count >= 2){
            transform.position = myPath[0];
            goodPath = true;
        } else {
            Debug.LogError("No Path Created for " + gameObject.name);
        }
    }

    void Update(){
        if(seen.inView){
            target = seen.player;
        } else {
            if(goodPath){
                transform.position += calculateDirection(myPath[on +1]) * speed * Time.deltaTime;
            }
        }
    }

    private Vector3 calculateDirection(Vector3 target){
        
        float x = target.x - transform.position.x;
        float z = target.z - transform.position.z;

        return new Vector3(x,0,z);
    }
}
