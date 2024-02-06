using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerLogic : MonoBehaviour
{
    [HideInInspector] public List<Vector3> myPath = new List<Vector3>();

    [SerializeField] private float maxPointDistance, speed;
    [SerializeField] [Range(0.0f, 10.0f)] private float turnSmooth, intelegence;

    [SerializeField] private bool hasPathing;
    [SerializeField] private ViewChecker pFinder;
    [SerializeField] private LayerMask obsticles;

    private Rigidbody rb;
    private int on = 1, pause = 1;
    private bool goodPath = false, seenPlayer;
    private Transform target;
    private Vector3 lastSeen;

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
        if(seenPlayer){
            if(Physics.Linecast(transform.position, target.position,obsticles)){
                if(intelegence < 3.34f){
                    seenPlayer = false;
                }else if(intelegence < 6.67f){
                    pause = 0;
                    //add timer before return
                }else{
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x,calculateDirection(lastSeen,transform.position),transform.eulerAngles.z);
                    if(MathF.Abs(transform.position.x - lastSeen.x) < maxPointDistance && MathF.Abs(transform.position.z - lastSeen.z) < maxPointDistance){
                        //add timer before return
                    }
                }

            } else {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x,calculateDirection(target.position,transform.position),transform.eulerAngles.z);
                lastSeen = target.position;
            }


        }else if(hasPathing && goodPath){
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,calculateDirection(myPath[on],transform.position),transform.eulerAngles.z);
            if(MathF.Abs(transform.position.x - myPath[on].x) < maxPointDistance && MathF.Abs(transform.position.z - myPath[on].z) < maxPointDistance){
                on++;
            }
            if(on >= myPath.Count) on = 0;

            if(pFinder.possiblePlayer != null){
                if(!Physics.Linecast(transform.position, pFinder.possiblePlayer.transform.position,obsticles)){
                    target = pFinder.possiblePlayer.transform;
                    seenPlayer = true;
                }
            }
        } else {
            //TODO, add a roam mode
        }
    }

    void FixedUpdate(){
        if(seenPlayer){
            rb.velocity = new Vector3(0, rb.velocity.y, 0) + speed * transform.forward * pause;
        }else if(hasPathing && goodPath){
            
            rb.velocity = new Vector3(0, rb.velocity.y, 0) + speed * transform.forward;
        }
    }

    private float calculateDirection(Vector3 to, Vector3 from){
        float x = to.x - from.x;
        float z = to.z - from.z;

        return Mathf.Rad2Deg*MathF.Atan2(x,z);
    }

    void OnDrawGizmos()
    {
        if (target != null)
        {
            if(!Physics.Linecast(transform.position, pFinder.possiblePlayer.transform.position,obsticles)){
                    
                Gizmos.color = Color.green;
            } else {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(transform.position, pFinder.possiblePlayer.transform.position);
        }
    }
}
