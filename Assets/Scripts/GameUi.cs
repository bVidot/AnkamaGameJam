using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour {

    public Image impatienceJauge;
    public Image cashJauge;

	// Use this for initialization
	void Start () {
        impatienceJauge.fillAmount = 0f;
        cashJauge.fillAmount = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        impatienceJauge.fillAmount = (float)Game.Instance.impatience / (float)Game.Instance.maxImpatience ;
        cashJauge.fillAmount = (float)Game.Instance.currentPlayerCash / (float)Game.Instance.clientCash;
    }
}
