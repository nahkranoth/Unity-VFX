using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "MyBlitMaterials", menuName = "Game/MyBlitMaterials")]
public class MyBlitMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static MyBlitMaterials _instance;

    public static MyBlitMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = UnityEngine.Resources.Load("MyBlitMaterials") as MyBlitMaterials;
            return _instance;
        }
    }
}