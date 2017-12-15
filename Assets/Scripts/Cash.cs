using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cash : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = PlayerManager.Instance.playerCash.ToString();
	}
	
}
