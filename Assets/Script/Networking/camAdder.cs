using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class camAdder : NetworkBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private AudioListener aud;

    public override void OnNetworkSpawn() {
        if(IsOwner){
            cam.enabled = true;
            aud.enabled = true;
        }
    }
}
