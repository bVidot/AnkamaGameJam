using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Game.Instance.player.SetTrigger("StartInterrupt");
                interruptClose = true;
                StopCoroutine(initFightCoroutine);
                fightCoroutine = StartCoroutine(LaunchFight(5f));
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

    IEnumerator LaunchFight(float fightDuration)
    {
        interruptClose = false;
        canFight = true;
        Game.Instance.door.SetTrigger("StartFight");
        Game.Instance.player.SetTrigger("StartFight");
        yield return new WaitForSeconds(fightDuration);
        canFight = false;
        CheckFightJauge();
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
            //LOSE
            Debug.Log("Lose");
            Game.Instance.PlayerLose();
        }
    }
}
