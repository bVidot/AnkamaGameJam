using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyManager : MonoBehaviour {

    public static DontDestroyManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != null)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
