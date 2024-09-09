using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class GravityDisruptionUnit : MonoBehaviour 
{
    [SerializeField] private float size, intensity;
    [SerializeField] private AnimationCurve falloff;

    void Start(){
        size = gameObject.GetComponent<SphereCollider>().radius;
    }
    public float getLevel(Vector3 pos){
        float x = Mathf.Abs(Mathf.Abs(pos.x) - Mathf.Abs(transform.position.x));
        float y = Mathf.Abs(Mathf.Abs(pos.y) - Mathf.Abs(transform.position.y));
        float z = Mathf.Abs(Mathf.Abs(pos.z) - Mathf.Abs(transform.position.z));

        return intensity * falloff.Evaluate(((size*size) - (x*x+y*y+z*z))/size);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position,size);
    }
}
