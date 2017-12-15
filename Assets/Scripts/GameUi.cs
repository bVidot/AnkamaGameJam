using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUi : MonoBehaviour {

    public Image impatienceJauge;
    public Image cashJauge;
    public Image resistanceJauge;

    public CanvasGroup panelGuitarHero;
    public CanvasGroup panelFight;
     

    // Use this for initialization
    void Start () {
        impatienceJauge.fillAmount = 0f;
        cashJauge.fillAmount = 0f;
        resistanceJauge.fillAmount = 0f;
        panelGuitarHero = FindObjectOfType<Canvas>().transform.GetChild(0).GetComponent<CanvasGroup>();
        panelFight = FindObjectOfType<Canvas>().transform.GetChild(1).GetComponent<CanvasGroup>();
        panelGuitarHero.DOFade(1f, 0f);
        panelFight.DOFade(0f, 0f);
    }
	
	// Update is called once per frame
	void Update () {
        impatienceJauge.fillAmount = Mathf.Min((float)Game.Instance.impatience / (float)Game.Instance.maxImpatience, 1f) ;
        cashJauge.fillAmount = Mathf.Min((float)Game.Instance.currentPlayerCash / (float)Game.Instance.clientCash, 1f);
        resistanceJauge.fillAmount = Mathf.Min((float)Game.Instance.fightGame.fightJauge / (float)Game.Instance.fightGame.minimumFightJauge, 1f);
    }

    public void SwitchCanvas(bool fight)
    {
        if(fight)
        {
            panelGuitarHero.DOFade(0f, 0.5f);
            panelFight.DOFade(1f, 0.5f);
        }
        else
        {
            panelGuitarHero.DOFade(1f, 0.5f);
            panelFight.DOFade(0f, 0.5f);
        }
        
    }
}
