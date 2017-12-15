using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DoorAsset : MonoBehaviour {

    [MenuItem("Assets/Create/DoorType")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<DoorType>();
    }
}
