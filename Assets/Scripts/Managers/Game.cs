using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum SaleState { Talk, Fight};
    public SaleState saleState;

    public House currentHouse;
    public DoorType doorType;
    public Door door;
    public Player player;

    public GuitarHero guitarHeroGame;
    public Fight fightGame;

    public int maxImpatience = 0;
    public int impatience = 0;

    public int clientCash = 100;
    public int currentPlayerCash = 0;

	// Use this for initialization
	void Start () {
        currentHouse = PlayerManager.Instance.houseSelected;
        door = FindObjectOfType<Door>();
        player = FindObjectOfType<Player>();
        guitarHeroGame =  GetComponent<GuitarHero>();
        fightGame = GetComponent<Fight>();

        currentPlayerCash = 0;
        maxImpatience = currentHouse.impatience;
        impatience = 0;

        switch(currentHouse.doorType)
        {
            case House.DoorType.Level1:
                doorType = Resources.Load("Doors/PoorDoor") as DoorType;
                break;
            case House.DoorType.Level2:
                break;
            case House.DoorType.Level3:
                break;
        }
        fightGame.minimumFightJauge = doorType.strenght;
        fightGame.increaseValue = PlayerManager.Instance.currentShoes.resistance;

        guitarHeroGame.StartSale();
    }

    public void SwitchGuitarHeroToFight()
    {
        guitarHeroGame.ClearAllNotes();
        fightGame.StartFightGame();
    }

    public void SwitchFightToGuitarHero()
    {
        player.SetTrigger("StartIdle");
        door.SetTrigger("EndFight");
        saleState = Game.SaleState.Talk;
        ResetImpatience();
        guitarHeroGame.StartSale();
    }

    public void ResetImpatience()
    {
        impatience = 10;
    }

    public void PlayerWin()
    {
        PlayerManager.Instance.playerCash += currentPlayerCash;
        currentPlayerCash = 0;
    }

    public void PlayerLose()
    {
        door.SetTrigger("DoorWin");
        player.SetTrigger("StartIdle");
        PlayerManager.Instance.playerCash += currentPlayerCash;
        currentPlayerCash = 0;
        currentHouse.nbOfFail++;
    }
}
