using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HouseAsset : MonoBehaviour {

    [MenuItem("Assets/Create/House")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<House>();
    }
}
