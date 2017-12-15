using System.Collections;
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

    public enum SaleState { Talk, Fight};
    public SaleState saleState;

    public House currentHouse;
    public DoorType doorType;
    public Door door;
    public Player player;

    public GuitarHero guitarHeroGame;
    public Fight fightGame;

    public GameUi gameUi;
    
    public int maxImpatience = 0;
    public int impatience = 0;
    public int impatienceLimit = 0;

    public int clientCash = 100;
    public int currentPlayerCash = 0;
   

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

        if (impatienceLimit >= 30)
            PlayerLose();

        impatience = impatienceLimit;
    }

    public void PlayerWin()
    {
        PlayerManager.Instance.playerCash += currentPlayerCash;
        currentPlayerCash = 0;
    }

    public void PlayerLose()
    {
        door.SetTrigger("DoorWin");
        player.SetTrigger("Lose", 0.3f);
        PlayerManager.Instance.playerCash += currentPlayerCash;
        currentPlayerCash = 0;
        currentHouse.nbOfFail++;
    }
}
