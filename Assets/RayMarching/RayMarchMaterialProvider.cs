using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "RayMarchMaterialProvider", menuName = "Game/RayMarchMaterialProvider")]
public class RayMarchMaterialProvider : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static RayMarchMaterialProvider _instance;

    public static RayMarchMaterialProvider Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = UnityEngine.Resources.Load("RayMarchMaterial") as RayMarchMaterialProvider;
            return _instance;
        }
    }
}