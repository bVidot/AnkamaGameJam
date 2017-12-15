using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    private static PlayerManager _instance = null;

    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerManager>();

                if (_instance == null)

                {
                    GameObject go = new GameObject();
                    go.name = "PlayerManager";
                    _instance = go.AddComponent<PlayerManager>();
                }

            }

            return _instance;

        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public int playerCash = 0;
    public Shoes currentShoes;
    public House houseSelected;

    // Use this for initialization
    void Start () {
        currentShoes = Resources.Load("Shoes/BasicShoes") as Shoes;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
