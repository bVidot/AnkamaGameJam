using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShoesAsset : MonoBehaviour {

    [MenuItem("Assets/Create/Shoes")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<Shoes>();
    }
}
