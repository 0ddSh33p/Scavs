using UnityEngine;

public class TerrainBuilder : MonoBehaviour
{
    [SerializeField] private Mesh[] meshes;
    [SerializeField] private float[] distances;

    private MeshFilter filter;
    private MeshCollider collides;
    [SerializeField] private Transform player;
    private float distance;
        
    void Awake(){
        filter = GetComponent<MeshFilter>();
        collides = GetComponent<MeshCollider>();
    }

    void Update(){
        distance = (Mathf.Abs(player.position.x - transform.position.x) * Mathf.Abs(player.position.x - transform.position.x)) + 
                   (Mathf.Abs(player.position.z - transform.position.z) * Mathf.Abs(player.position.z - transform.position.z));
            
        
        for(int i = 0; i < distances.Length; i++){
            if(distance > distances[i]){
                filter.mesh = meshes[i];
                collides.sharedMesh = meshes[i];
            }
        }

        Mesh mMesh = filter.mesh;
    }
}
