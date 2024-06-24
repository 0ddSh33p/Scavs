using System.Collections.Generic;
using UnityEngine;

public class seedGen : MonoBehaviour
{
    public Vector2 seed, perlinScale, macroScale, trueScale;
    public Vector2Int size;
    public List<GameObject> players;
    public float height, perlinPercent, macroHeight;

    [SerializeField] private int renderDistance;
    [SerializeField] private float POIsPer100Units, maxTilt;
    [SerializeField] private GameObject Chunk;
    [SerializeField] private List<GameObject> structures;

    [SerializeField] private LayerMask layerMask;

    [HideInInspector] public List<Vector2Int> meshLocations = new List<Vector2Int>();

    private Vector2 posOfPlayer;
    private bool[,] field = new bool[1,1];
    private bool offPos;
    
    void Awake()
    {
        seed = new Vector2(Random.Range(-99999,99999),Random.Range(-99999, 99999));
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
    }

    void Update(){
        foreach(GameObject player in players){
            for(int r = -renderDistance/2; r<=renderDistance/2; r++){
                for(int c = -renderDistance/2; c<=renderDistance/2; c++){
                    offPos = true;
                    posOfPlayer = new Vector2(Mathf.FloorToInt((player.transform.position.x + transform.position.x)/size.x) + r,
                                            Mathf.FloorToInt((player.transform.position.z + transform.position.z)/size.y) + c);

                    for(int i = 0; i < meshLocations.Count; i++){
                        
                        if(posOfPlayer == meshLocations[i]){
                            offPos = false;
                            break;
                        }
                    }
                    if(offPos){
                        Instantiate(Chunk, new Vector3(posOfPlayer.x * size.x, 0 , posOfPlayer.y * size.y), Quaternion.identity);
                    }
                }
            }
       
        }
    }

    public void addPOI(Vector3 loc){
        if(Random.Range(0f,1f) < POIsPer100Units){
            loc += new Vector3(Random.Range(-50,50), loc.y+height+macroHeight+1 ,Random.Range(-50,50));
            RaycastHit hit;
            if(Physics.Raycast(loc, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask)){
                Instantiate(structures[Random.Range(0,structures.Count)], hit.point, Quaternion.Euler(Random.Range(-maxTilt,maxTilt), Random.Range(0,360), Random.Range(-maxTilt,maxTilt)));
            }
        }
    }
}