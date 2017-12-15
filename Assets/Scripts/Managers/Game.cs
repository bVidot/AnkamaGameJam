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

    public int impatience = 0;

    [Header("Guitar Hero")]
    Coroutine saleCoroutine;
    public float delayBetweenNote = 1f;
    public Transform notePrefab;
    public bool canSale = false;

    public float perfectDistance = 0.1f;
    public float goodDistance = 0.3f;
    public float mediumDistance = 0.6f;
    public float badDistance = 1f;
    public Transform slot1;
    public List<Transform> noteSlot1;
    public Transform slot2;
    public List<Transform> noteSlot2;
    public Transform slot3;
    public List<Transform> noteSlot3;
    public Transform slot4;
    public List<Transform> noteSlot4;

    public float noteSpeed = 1f;

    [Header("Fight")]
    Coroutine fightCoroutine;
    public float interruptDuration = 1f;
    public bool canInterruptClose = false;
    public bool interruptClose = false;
    public bool canFight = false;
     
    public int fightJauge = 0;
    public int minimumFightJauge = 15;

	// Use this for initialization
	void Start () {
        StartSale();
    }
	
	// Update is called once per frame
	void Update () {
		if(saleState == SaleState.Fight && canFight)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                fightJauge += 2;
            }
        }else if(saleState == SaleState.Fight && canInterruptClose)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                interruptClose = true;
            }
        }
        else if(saleState == SaleState.Talk && canSale)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                if(noteSlot1.Count < 0 && !noteSlot1[0])
                    impatience += 3;
                else
                {
                    CheckNoteTiming(1, noteSlot1[0].transform.position.y);
                    noteSlot1[0].GetComponent<Note>().DeleteNote();
                }
                    
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (noteSlot2.Count < 0 && !noteSlot2[0])
                    impatience += 3;
                else
                {
                    CheckNoteTiming(2, noteSlot2[0].transform.position.y);
                    noteSlot2[0].GetComponent<Note>().DeleteNote();
                }
                    
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (noteSlot3.Count < 0 && !noteSlot3[0])
                    impatience += 3;
                else
                {
                    CheckNoteTiming(3, noteSlot3[0].transform.position.y);
                    noteSlot3[0].GetComponent<Note>().DeleteNote();
                }
                    
            }
            else if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (noteSlot4.Count < 0 && !noteSlot4[0])
                    impatience += 3;
                else
                {
                    CheckNoteTiming(4, noteSlot4[0].transform.position.y);
                    noteSlot4[0].GetComponent<Note>().DeleteNote();
                }
            }
            impatience = Mathf.Min(impatience, 30);
        }
	}

    public void ResetImpatience()
    {
        impatience = 10;
        fightJauge = 0;
    }

    #region GuitarHero
    public void StartSale()
    {
        canFight = false;
        canInterruptClose = false;

        saleState = SaleState.Talk;
        saleCoroutine = StartCoroutine(InitSale(2f));
    }

    IEnumerator InitSale(float delay)
    {
        float delaySlot1 = Random.Range(0f, 1f);
        float delaySlot2 = Random.Range(0f, 1f);
        float delaySlot3 = Random.Range(0f, 1f);
        float delaySlot4 = Random.Range(0f, 1f);

        yield return new WaitForSeconds(delay);
        canSale = true;

        StartCoroutine(SpawnNote(1, delaySlot1));
        StartCoroutine(SpawnNote(2, delaySlot2));
        StartCoroutine(SpawnNote(3, delaySlot3));
        StartCoroutine(SpawnNote(4, delaySlot4));

    }

    private void CheckNoteTiming(int slot, float posY)
    {
        float distance = 0f;
        switch(slot)
        {
            case 1:
                distance = Mathf.Abs(slot1.transform.position.y - posY);
                break;
            case 2:
                distance = Mathf.Abs(slot2.transform.position.y - posY);
                break;
            case 3:
                distance = Mathf.Abs(slot3.transform.position.y - posY);
                break;
            case 4:
                distance = Mathf.Abs(slot4.transform.position.y - posY);
                break;
        }

        if(distance <= perfectDistance)
        {
            impatience += 0;
        }
        else if(distance <= goodDistance)
        {
            impatience += 1;
        }
        else if (distance <= mediumDistance)
        {
            impatience += 2;
        }
        else
        {
            impatience += 3;
        }

        impatience = Mathf.Min(impatience , 30);

    }

    public void ClearAllNotes()
    {
        StopAllCoroutines();
        foreach(Transform note in noteSlot1)
        {
            if(note)
                note.GetComponent<Note>().DeleteNote(true);
        }
        noteSlot1.Clear();

        foreach (Transform note in noteSlot2)
        {
            if (note)
                note.GetComponent<Note>().DeleteNote(true);
        }
        noteSlot2.Clear();

        foreach (Transform note in noteSlot3)
        {
            if (note)
                note.GetComponent<Note>().DeleteNote(true);
        }
        noteSlot3.Clear();

        foreach (Transform note in noteSlot4)
        {
            if (note)
                note.GetComponent<Note>().DeleteNote(true);
        }
        noteSlot4.Clear();
    }

    public IEnumerator SpawnNote(int slot, float delay)
    {
        yield return new WaitForSeconds(delay);
        Transform note;
        switch (slot)
        {
            case 1:
                note = Instantiate(notePrefab, new Vector3(slot1.position.x, -1, -9), Quaternion.identity) as Transform;
                note.GetComponent<Note>().slot = Note.Slot.Slot1;
                noteSlot1.Add(note);
                break;
            case 2:
                note = Instantiate(notePrefab, new Vector3(slot2.position.x, -1, -9), Quaternion.identity);
                note.GetComponent<Note>().slot = Note.Slot.Slot2;
                noteSlot2.Add(note);
                break;
            case 3:
                note = Instantiate(notePrefab, new Vector3(slot3.position.x, -1, -9), Quaternion.identity);
                note.GetComponent<Note>().slot = Note.Slot.Slot3;
                noteSlot3.Add(note);
                break;
            case 4:
                note = Instantiate(notePrefab, new Vector3(slot4.position.x, -1, -9), Quaternion.identity);
                note.GetComponent<Note>().slot = Note.Slot.Slot4;
                noteSlot4.Add(note);
                break;
        }
    }
#endregion
    public void StartFightGame()
    {
        canSale = false;
        ClearAllNotes();
        saleState = SaleState.Fight;
        fightCoroutine = StartCoroutine(InitFight(2f, 2f));
    }

    IEnumerator InitFight(float delay, float fightDuration)
    {
        canInterruptClose = true;
        yield return new WaitForSeconds(interruptDuration);
        canInterruptClose = false;
        if(!interruptClose)
        {
            //LOSE
            Debug.Log("Lose");
            yield break;
        }
        interruptClose = false;
        yield return new WaitForSeconds(delay - interruptDuration);
        canFight = true;
        yield return new WaitForSeconds(fightDuration);
        canFight = false;
        CheckFightJauge();
    }

    private void CheckFightJauge()
    {
        if(fightJauge >= minimumFightJauge)
        {
            //WIN adn restart saleMode
            ResetImpatience();
            StartSale();
        }
        else
        {
            //LOSE
            Debug.Log("Lose");
        }
    }
}
