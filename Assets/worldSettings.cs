using UnityEngine;

public class worldSettings : MonoBehaviour
{
    [SerializeField] private Material fogAsset;
    [SerializeField] private Color fogColor, tintColor;
    [SerializeField] private float lightStrength;
    

    void Start(){
        fogAsset.SetFloat("_Main_Light_Intensity", lightStrength);
        fogAsset.SetColor("_Colour", fogColor);
        fogAsset.SetColor("_Tint", tintColor);
    }
}
