using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerLogic : MonoBehaviour
{
    [HideInInspector] public List<Vector3> myPath = new List<Vector3>();

    [SerializeField] private float maxPointDistance, speed;
    [SerializeField] [Range(0.0f, 30.0f)] private float turnSmooth, intelegence;

    [SerializeField] private bool hasPathing;
    [SerializeField] private ViewChecker pFinder;
    [SerializeField] private LayerMask obsticles;

    private Rigidbody rb;
    private int on = 1, pause = 1;
    private bool goodPath = false, seenPlayer;
    private Transform target;
    private GameObject pPlayer;
    private Vector3 lastSeen;
    private float lastY;

    void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    void Start(){
        if(hasPathing){
            myPath = GetComponent<PathEdit>().myPath;
            if(myPath.Count >= 2){
                transform.position = myPath[0];
                goodPath = true;
                lastY = transform.eulerAngles.y;
            } else {
                Debug.LogError("No Path Created for " + gameObject.name);
            }
        }
    }

    void Update(){
        if(seenPlayer){
            if(Physics.Linecast(transform.position, target.position,obsticles)){
                if(intelegence < 10f){
                    seenPlayer = false;
                }else if(intelegence < 20f){
                    pause = 0;
                    //add timer before return
                    seenPlayer = false;
                }else{                 
                    if(MathF.Abs(transform.position.x - lastSeen.x) < maxPointDistance && MathF.Abs(transform.position.z - lastSeen.z) < maxPointDistance){
                        //add timer before return
                    }
                }
                
            } else {
                lastSeen = target.position;
            }


        }else if(hasPathing && goodPath){
            
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
            if(Physics.Linecast(transform.position, target.position,obsticles)){
                if(intelegence >= 20){
                    rotate(lastSeen);
                }
            }else{
                rotate(target.position);
            }
            rb.velocity = new Vector3(0, rb.velocity.y, 0) + speed * transform.forward * pause;
        }else if(hasPathing && goodPath){
            rotate(myPath[on]);
            rb.velocity = new Vector3(0, rb.velocity.y, 0) + speed * transform.forward;
        }
    }

    private void rotate(Vector3 to){
        float turn;
        if(lastY - calculateDirection(to,transform.position) > 180){
            turn = (calculateDirection(to,transform.position) + turnSmooth * (lastY - 360))/(turnSmooth + 1);
        }else{
            turn = (calculateDirection(to,transform.position) + turnSmooth * lastY)/(turnSmooth + 1);
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, turn, transform.eulerAngles.z);
        lastY = transform.eulerAngles.y;   
    }
    private float calculateDirection(Vector3 to, Vector3 from){
        float x = to.x - from.x;
        float z = to.z - from.z;

        float direc = Mathf.Rad2Deg*MathF.Atan2(x,z);

        if (direc < 0f){
            direc = 360 + direc;
        }

        return direc;
    }

    void OnDrawGizmos()
    {   if(pFinder.possiblePlayer != null){
            pPlayer = pFinder.possiblePlayer.gameObject;
        }
        if (target != null)
        {
            if(!Physics.Linecast(transform.position, pPlayer.transform.position,obsticles)){
                    
                Gizmos.color = Color.green;
            } else {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(transform.position, pPlayer.transform.position);
        }
    }
}
