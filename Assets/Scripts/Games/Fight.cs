﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fight : MonoBehaviour {

    [Header("Fight")]
    Coroutine initFightCoroutine;
    Coroutine fightCoroutine;
    public float interruptDuration = 1f;
    public bool canInterruptClose = false;
    public bool interruptClose = false;
    public bool canFight = false;

    public int fightJauge = 0;
    public int minimumFightJauge = 0;
    public int increaseValue = 0;

    private bool FightPlayed = false;
    private bool LosePlayed = false;
    private bool AlarmPlayed = false;

    private float fightDuration = 5f;

    public Image fightTimer;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Game.Instance.saleState == Game.SaleState.Fight && canFight)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fightJauge += increaseValue;
            }
        }
        else if (Game.Instance.saleState == Game.SaleState.Fight && canInterruptClose && !interruptClose)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!FightPlayed)
                {
                    FightPlayed = true;
                    // ADRIEN
                    AudioClip sfx;
                    sfx = Resources.Load("Sounds/Fight") as AudioClip;
                    SoundManager.PlaySFX(sfx);
                }

                Game.Instance.player.SetTrigger("StartInterrupt");
                interruptClose = true;
                StopCoroutine(initFightCoroutine);
                fightCoroutine = StartCoroutine(LaunchFight(fightDuration));
            }
        }
    }

    public void StartFightGame()
    {
        Game.Instance.saleState = Game.SaleState.Fight;
        initFightCoroutine = StartCoroutine(InitFight(2f, 5f));
    }

    IEnumerator InitFight(float delay, float fightDuration)
    {
        canInterruptClose = true;
        Game.Instance.door.SetTrigger("StartWarning");

        // ADRIEN
        if (!Game.Instance.AlarmPlayed && !AlarmPlayed)
        {
            SoundManager.StopSFX(Game.Instance.conv);

            AlarmPlayed = true;
            AudioClip sfx;
            sfx = Resources.Load("Sounds/Alarm") as AudioClip;
            SoundManager.PlaySFX(sfx);
            FightPlayed = false;
            LosePlayed = false;
        }

        yield return new WaitForSeconds(interruptDuration);
        canInterruptClose = false;
        if (!interruptClose)
        {
            //LOSE
            interruptClose = false;
            Game.Instance.PlayerLose();
            yield break;
        }
    }

    IEnumerator Timer(float fightDuration)
    {
        float currentFightDuration = fightDuration;
        while(currentFightDuration > 0f)
        {

            currentFightDuration -= Time.deltaTime;
            fightTimer.fillAmount = Mathf.Max(0f, currentFightDuration / fightDuration);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator LaunchFight(float fightDuration)
    {
        interruptClose = false;
        canFight = true;
        StartCoroutine(Timer(fightDuration));
        Game.Instance.door.SetTrigger("StartFight");
        Game.Instance.player.SetTrigger("StartFight");
        yield return new WaitForSeconds(fightDuration);
        canFight = false;
        CheckFightJauge();

        //ADRIEN
        AlarmPlayed = false;
    }

    private void CheckFightJauge()
    {
        if (fightJauge >= minimumFightJauge)
        {
            //WIN adn restart saleMode
            canFight = false;
            canInterruptClose = false;
            fightJauge = 0;
            Game.Instance.SwitchFightToGuitarHero();
        }
        else
        {
            if (!LosePlayed)
            {
                LosePlayed = true;
                // ADRIEN
                AudioClip sfx;
                sfx = Resources.Load("Sounds/Crac") as AudioClip;
                SoundManager.PlaySFX(sfx);
            }

            //LOSE
            Debug.Log("Lose");
            Game.Instance.PlayerLose();
        }
    }
}
