using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerLogic : MonoBehaviour
{
    public Vector3 toPos;
    [HideInInspector] public List<Vector3> myPath = new List<Vector3>();
    private List<Vector3> ledPath = new List<Vector3>();

    [SerializeField] private float maxPointDistance, speed;
    [SerializeField] private bool hasPathing;
    [SerializeField] private LayerMask obsticles;

    [SerializeField] [Range(0f, 30f)] private float turnSmooth, intelegence;
    [SerializeField] [Range(0f, 10f)] private float suspitiousness;
    [SerializeField] private ViewChecker pFinder;
    [SerializeField] private NavMeshInstance myMesh;




    private Rigidbody rb;
    private int on = 1, pause = 1;
    private bool goodPath = false, seenPlayer, waiting;
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
                ledPath = myPath;
                goodPath = true;
            } else {
                Debug.LogError("No Path Created for " + gameObject.name);
            }
        }
        lastY = transform.eulerAngles.y;
    }

    void Update(){
        if(seenPlayer){
            if(Physics.Linecast(transform.position, pPlayer.transform.position,obsticles)){
                if(intelegence < 10f){
                    seenPlayer = false;
                }else if(intelegence < 20f){
                    if(!waiting){
                        StartCoroutine(PauseAndWait());
                    }
                }else{                 
                    if(onPoint(lastSeen)){
                        if(!waiting){
                            StartCoroutine(PauseAndWait());
                        }
                    }
                }
                
            } else {
                if(waiting){
                    StopCoroutine(PauseAndWait());
                    pause = 1;
                    waiting = false;
                }
                pPlayer = pFinder.possiblePlayer.gameObject;
                lastSeen =  pPlayer.transform.position;
            }

        }else if(goodPath){
            if(onPoint(ledPath[on])){
                on++;
            }
            if(on >= ledPath.Count) on = 0;

            if(pFinder.possiblePlayer != null){
                pPlayer = pFinder.possiblePlayer.gameObject;
                if(!Physics.Linecast(transform.position, pPlayer.transform.position,obsticles)){
                    seenPlayer = true;
                }
            } else {
                pPlayer = null;
            }
        } else {
            findPath(transform.position, toPos);
        }
    }

    
    void findPath(Vector3 startPos, Vector3 endPos){

        MeshPoint onPoint = myMesh.findPoint(startPos);
        MeshPoint toPoint = myMesh.findPoint(endPos);
        List<MeshPoint> openSet = new List<MeshPoint>();
        HashSet<MeshPoint> closedSet = new HashSet<MeshPoint>();

        openSet.Add(onPoint);

        while(openSet.Count > 0){
            MeshPoint currentPoint = openSet[0];

            for(int i = 1; i < openSet.Count; i++){
                if(openSet[i].fCost < currentPoint.fCost || (openSet[i].fCost == currentPoint.fCost && openSet[i].hCost < currentPoint.hCost)){
                    currentPoint = openSet[i];
                }
            }

            openSet.Remove(currentPoint);
            closedSet.Add(currentPoint);

            if (currentPoint == toPoint){
                retracePath(onPoint,toPoint);
                return;
            }

            foreach(MeshPoint neighbor in myMesh.getNeighbors(currentPoint)){
                if(!neighbor.good || closedSet.Contains(neighbor)){
                    continue;
                }
                
                int newCostToNeighbor = currentPoint.gCost + getDistance(currentPoint, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)){
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = getDistance(neighbor, toPoint);
                    neighbor.parent = currentPoint;

                    if(!openSet.Contains(neighbor)){
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    void retracePath(MeshPoint start, MeshPoint end){
        MeshPoint onPoint = end;
        myMesh.path = null;
        while(onPoint != start){
            ledPath.Add(onPoint.globalPos);
            myMesh.path.Add(onPoint);
            onPoint = end.parent;
        }
        ledPath.Reverse();
        goodPath = true;
    }

    int getDistance(MeshPoint pointA, MeshPoint pointB){
        int dstX = Mathf.Abs(pointA.localPos.x - pointB.localPos.x);
        int dstY = Mathf.Abs(pointA.localPos.y - pointB.localPos.y);
        if(dstX>dstY){
            return 14* dstY+10*(dstX-dstY);
        } 
        return 14* dstX+10*(dstY-dstX);
    }

    void FixedUpdate(){
        if(seenPlayer){
            if(Physics.Linecast(transform.position, pPlayer.transform.position,obsticles)){
                if(intelegence >= 20){
                    if(!waiting){
                        rotate(lastSeen);
                    }
                }
            }else{
                rotate(pPlayer.transform.position);
            }
            
        }else if(goodPath){
            rotate(ledPath[on]);
        }
        moveForward();
    }

    IEnumerator PauseAndWait(){
        waiting = true;
        pause = 0;
        yield return new WaitForSeconds(suspitiousness);
        seenPlayer = false;
        pause = 1;
        waiting = false;
    }

    private void moveForward(){
        rb.velocity = new Vector3(0, rb.velocity.y, 0) + speed * transform.forward * pause;
    }
    private bool onPoint(Vector3 point){
        return MathF.Abs(transform.position.x - point.x) < maxPointDistance && MathF.Abs(transform.position.z - point.z) < maxPointDistance;
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
    {   if(pPlayer != null){

            if(!Physics.Linecast(transform.position, pPlayer.transform.position,obsticles)){
                    
                Gizmos.color = Color.green;
            } else {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(transform.position, pPlayer.transform.position);
        }
    }
}
