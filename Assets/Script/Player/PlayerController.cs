using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float walkSpeed = 4, crouchSpeed = 2, runSpeed = 7, jumpH = 12, maxGround = 0.2f, grav = 0.2f, sensitivity = 0.5f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject cam, flashLight;

    private Rigidbody rb;
    private Vector3 vel;
    private float speed;
    private int gear;
    private bool grounded, lIO;
    
 
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gear = 2;

        GameObject tempObj = GameObject.FindWithTag("PrimarySpawn");
        if( tempObj != null){
            transform.position = tempObj.transform.position;
        }

        /*tempObj = GameObject.FindWithTag("WorldSeed");
        if(tempObj.GetComponent<seedGen>() != null){
            tempObj.GetComponent<seedGen>(). players.Add(gameObject);
        }*/

    }

    /*void Start(){
        GameObject.FindGameObjectWithTag("WorldSeed").GetComponent<seedGen>().players.Add(gameObject);
    }*/


    void FixedUpdate()
    {
        if (!IsOwner) return;

        float horz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        

        switch (gear){
            case 1:
                speed = crouchSpeed;
                break;
            case 2:
                speed = walkSpeed;
                break;
            case 3:
                speed = runSpeed;
                break;
            default:
                speed = walkSpeed;
                break;
        }

        if(!grounded) speed /= 1.3f;
        if(horz != 0 && vert != 0){
            speed /= 1.3f;
        }

        vel = new Vector3(0,rb.velocity.y - grav,0);
        
        vel += transform.forward * vert * speed;
        vel += transform.right * horz * speed;

        rb.velocity = vel;
    }

    void Update(){
        if (!IsOwner) return;
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            if(gear!=3){
                gear = 3;
            }
            else{
                gear = 2;
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            if(gear!=1){
                gear = 1;                
            }
            else{
                gear = 2;
            }
        }

        if(Input.GetKeyDown("f")){
            if(lIO){
                flashLight.SetActive(false);
                lIO = false;
            }
            else{
                flashLight.SetActive(true);
                lIO = true;
            }
        }

        if(Input.GetAxis("Mouse Y") != 0){
            cam.transform.eulerAngles += new Vector3(-sensitivity * Input.GetAxis("Mouse Y"),0,0);
        }

        if(Input.GetAxis("Mouse X") != 0){
            transform.eulerAngles += new Vector3(0,sensitivity * Input.GetAxis("Mouse X"),0);
        }

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, maxGround, layerMask)){
            grounded = true;
        }
        else{
            grounded = false;
        }

        if (grounded && Input.GetKeyDown("space"))
        {
            rb.AddForce(Vector3.down * -jumpH * 10);
        }
    }
}
