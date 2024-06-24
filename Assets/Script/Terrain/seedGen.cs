using System.Collections.Generic;
using UnityEngine;

public class seedGen : MonoBehaviour
{
    public Vector2 seed, perlinScale, macroScale, trueScale;
    public Vector2Int size;
    public List<GameObject> players;
    public float height, perlinPercent, macroHeight;
    [SerializeField] private int renderDistance;

    [SerializeField] private GameObject Chunk;
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
}