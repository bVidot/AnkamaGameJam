using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (!PlayerManager.Instance.firstPlay)
            Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            GetComponent<CanvasGroup>().DOFade(0f, 1f).OnComplete(() => DestroyCanvas());
	}

    void DestroyCanvas()
    {
        Destroy(this.gameObject);
    }
}
