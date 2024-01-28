using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class crawl : NetworkBehaviour
{
    [SerializeField] private Transform[] targets, IKs;
    [SerializeField] private float setpSize, stepHeight, stepSmooth, speed, GmaxDist, DownSpeed;
    
    private float maxDist;
    private Rigidbody[] tRB;
    private int leg;
    private Vector3[] goodLocations, oldLocs;

    void Start(){
        goodLocations = new Vector3[targets.Length];
        oldLocs = new Vector3[targets.Length];
        tRB = new Rigidbody[targets.Length];


        for(int i = 0; i < targets.Length; i++){
            goodLocations[i] = targets[i].position;
            tRB[i] = targets[i].gameObject.GetComponent<Rigidbody>();
            oldLocs[i] = targets[i].position;
        }
        StartCoroutine("Step");
    }

    void Update(){

        for(int i = 0; i < targets.Length; i++){

            if(leg != i){
                targets[i].position += new Vector3(oldLocs[i].x - targets[i].position.x, 0 ,oldLocs[i].z - targets[i].position.z);
            }
            else{
                oldLocs[i] = targets[i].position;
            }

            goodLocations[i] = IKs[i].position;
        }
    }

    void FixedUpdate(){
        for(int i = 0; i < tRB.Length; i++){
            tRB[i].velocity -= new Vector3(0,DownSpeed,0);
        }
    }

    public IEnumerator Step(){
        maxDist = GmaxDist;
        leg = -1;

        for(int i = 0; i < targets.Length; i++){
            float curDist = Theorem(targets[i].position.x,targets[i].position.z,goodLocations[i].x,goodLocations[i].z);
            if(curDist > maxDist*maxDist){
                leg = i;
                maxDist = curDist;
            }
        }

        if(leg >= 0){
            Vector3 toGo = new Vector3(goodLocations[leg].x - targets[leg].position.x,0,goodLocations[leg].z - targets[leg].position.z);

            for(int i = 0; i < stepSmooth; i++){

                targets[leg].position += toGo/stepSmooth;
                if(i < stepSmooth/3){
                    targets[leg].position += new Vector3(0,2*stepHeight/stepSmooth,0);
                }
                yield return new WaitForSeconds(0.005f);
            }
        }

        yield return new WaitForSeconds(0.001f);
        StartCoroutine("Step");
    }

    float Theorem(float a1, float b1, float a2, float b2){

        float a = Mathf.Abs(a1-a2);
        float b = Mathf.Abs(b1-b2);

        return Mathf.Sqrt((a*a)+(b*b));
    }
}
