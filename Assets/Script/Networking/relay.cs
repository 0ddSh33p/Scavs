using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

public class relay : MonoBehaviour
{
    [HideInInspector] public string code = "";
    [SerializeField] private int MaxPlayers;
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void lay(){
        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(MaxPlayers);

            code = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData data = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data);
            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e) {
            Debug.LogWarning(e);
        }
    }

    public async void joinLay(string Mcode){
        try{
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(Mcode);

            RelayServerData data = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data);
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e) {
            Debug.LogWarning(e);
        }
    }

}
