using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, mouse;
    [SerializeField] private GameObject cam;
    [SerializeField] private Rigidbody rb;
    float x,z, mX = 0,mY = 0;

    void FixedUpdate(){
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        mX += Input.GetAxis("Mouse X") * mouse;
        mY += Input.GetAxis("Mouse Y") * mouse;

        
        rb.velocity += transform.forward * z * speed;
        rb.velocity += transform.right * x * speed;



        cam.transform.eulerAngles = new Vector3(-mY, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x,mX,gameObject.transform.eulerAngles.z);
    }
}
