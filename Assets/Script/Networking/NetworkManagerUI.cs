using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
   
    [SerializeField]private Button hostBtn, clientBtn;
    [SerializeField]private relay Relay;
    [SerializeField]private GameObject sceneCamera;
    [SerializeField]private TMP_InputField code;
    [SerializeField]private TMP_Text joinCode;

    private void Awake(){
        hostBtn.onClick.AddListener(() => {

            Relay.lay();
            code.text = Relay.code;
            joinCode.text = Relay.code;
            sceneCamera.SetActive(false);
            
        });
        clientBtn.onClick.AddListener(() => {
            try{
            Relay.joinLay(code.text);
            joinCode.text = code.text;
            
            }
            catch{
                Debug.Log("bad code");
            }
        });
    }

    void Update(){
        joinCode.text = Relay.code;
    }

}
