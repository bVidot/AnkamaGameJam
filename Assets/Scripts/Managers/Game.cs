﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    private static Game _instance = null;

    public static Game Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Game>();

                if (_instance == null)

                {
                    GameObject go = new GameObject();
                    go.name = "GameManager";
                    _instance = go.AddComponent<Game>();
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

    public enum SaleState { Talk, Fight, End};
    public SaleState saleState;

    public House currentHouse;
    public DoorType doorType;
    public Door door;
    public Player player;

    public Animator contractAnimator;

    public GuitarHero guitarHeroGame;
    public Fight fightGame;

    public GameUi gameUi;
    
    public int maxImpatience = 0;
    public int impatience = 0;
    public int impatienceLimit = 0;

    public int clientCash = 100;
    public int currentPlayerCash = 0;
   
    public AudioClip conv;

    // Use this for initialization
    void Start () {
        currentHouse = PlayerManager.Instance.houseSelected;
        door = FindObjectOfType<Door>();
        player = FindObjectOfType<Player>();
        guitarHeroGame =  GetComponent<GuitarHero>();
        fightGame = GetComponent<Fight>();

        gameUi = FindObjectOfType<GameUi>();

        currentPlayerCash = 0;
        maxImpatience = currentHouse.impatience;
        impatience = 0;

        //ADRIEN
        conv = Resources.Load("Sounds/Conversation" + Random.Range(1, 3)) as AudioClip;

        switch (currentHouse.doorType)
        {
            case House.DoorType.Level1:
                doorType = Resources.Load("Doors/PoorDoor") as DoorType;
                break;
            case House.DoorType.Level2:
                doorType = Resources.Load("Doors/BasicDoor") as DoorType;
                break;
            case House.DoorType.Level3:
                doorType = Resources.Load("Doors/RichDoor") as DoorType;
                break;
        }
        clientCash = currentHouse.budget;
        fightGame.minimumFightJauge = doorType.strenght;
        fightGame.increaseValue = PlayerManager.Instance.currentShoes.resistance;

        saleState = SaleState.Talk;
        door.SetTrigger("DoorLose");
        guitarHeroGame.StartSale();
    }

    public void SwitchGuitarHeroToFight()
    {
        guitarHeroGame.ClearAllNotes();
        gameUi.SwitchCanvas(true);
        fightGame.StartFightGame();
    }

    public void SwitchFightToGuitarHero()
    {
        door.SetTrigger("EndFight");
        player.SetTrigger("Win");
        saleState = Game.SaleState.Talk;
        ResetImpatience();
        gameUi.SwitchCanvas(false);
        guitarHeroGame.StartSale();
    }

    public void ResetImpatience()
    {
        impatienceLimit += maxImpatience * 33 /100;

        if (impatienceLimit >= maxImpatience)
            PlayerLose();

        impatience = impatienceLimit;
    }

    public void IncreaseCash(int amount)
    {
        currentPlayerCash += amount;
        if (currentPlayerCash >= clientCash)
            PlayerWin();
    }

    public void PlayerWin()
    {
        StopAllCoroutines();
        guitarHeroGame.ClearAllNotes();
        gameUi.Hide();
        gameUi.Victory();
        saleState = SaleState.End;
        StopAllCoroutines();
        PlayerManager.Instance.playerCash += currentPlayerCash;
        currentPlayerCash = 0;
        // ADRIEN
        SoundManager.StopSFX(conv);
        AudioClip sfx;
        sfx = Resources.Load("Sounds/Victory") as AudioClip;
        SoundManager.PlaySFX(sfx);
        door.SetTrigger("DoorLose");
        Invoke("Contract", 1f);
        UiManager.Instance.LoadTargetScene(1, 3f);
    }

    //ADRIEN
    public bool LosePlayed = false;
    public bool AlarmPlayed = false;

    public void PlayerLose()
    {
        // ADRIEN
        if (!LosePlayed)
        {
           SoundManager.StopSFX(conv);
            LosePlayed = true;
            AlarmPlayed = true;
            AudioClip sfx;
            sfx = Resources.Load("Sounds/DoorClosed") as AudioClip;
            SoundManager.PlaySFX(sfx);
        }
        
        StopAllCoroutines();
        gameUi.Hide();
        gameUi.Defeat();
        door.SetTrigger("DoorWin");
        player.SetTrigger("Lose", 0.3f);
        PlayerManager.Instance.playerCash += currentPlayerCash;
        currentPlayerCash = 0;
        currentHouse.nbOfFail++;
        UiManager.Instance.LoadTargetScene(1, 3f);
    }

    void Contract()
    {
        contractAnimator.SetFloat("Speed", 1f);
    }

}
