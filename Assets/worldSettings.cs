using MirzaBeig.VolumetricFogLite;
using UnityEngine;

public class worldSettings : MonoBehaviour
{
    [SerializeField] private Material sceneFogAsset;
    [SerializeField] private VolumetricFogRendererFeatureLite visualAssignment;

    void Awake(){
        visualAssignment.settings.fogMaterial = sceneFogAsset;
    }
}
