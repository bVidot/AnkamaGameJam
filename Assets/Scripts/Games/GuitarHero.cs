using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarHero : MonoBehaviour {

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

    public Sprite spriteSlot1;
    public Sprite spriteSlot2;
    public Sprite spriteSlot3;
    public Sprite spriteSlot4;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (Game.Instance.saleState == Game.SaleState.Talk && canSale)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (noteSlot1.Count < 0 && !noteSlot1[0])
                    Game.Instance.impatience += 3;
                else
                {
                    CheckNoteTiming(1, noteSlot1[0].transform.position.y);
                    noteSlot1[0].GetComponent<Note>().DeleteNote();
                }

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (noteSlot2.Count < 0 && !noteSlot2[0])
                    Game.Instance.impatience += 3;
                else
                {
                    CheckNoteTiming(2, noteSlot2[0].transform.position.y);
                    noteSlot2[0].GetComponent<Note>().DeleteNote();
                }

            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (noteSlot3.Count < 0 && !noteSlot3[0])
                    Game.Instance.impatience += 3;
                else
                {
                    CheckNoteTiming(3, noteSlot3[0].transform.position.y);
                    noteSlot3[0].GetComponent<Note>().DeleteNote();
                }

            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (noteSlot4.Count < 0 && !noteSlot4[0])
                    Game.Instance.impatience += 3;
                else
                {
                    CheckNoteTiming(4, noteSlot4[0].transform.position.y);
                    noteSlot4[0].GetComponent<Note>().DeleteNote();
                }
            }
            Game.Instance.impatience = Mathf.Min(Game.Instance.impatience, Game.Instance.maxImpatience);
        }

    }
    
    public void StartSale()
    {
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
        switch (slot)
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

        AudioClip sfx;

        if (distance <= perfectDistance)
        {
            Game.Instance.impatience += 0;
            sfx = Resources.Load("Sounds/Notes/Parfait0" + slot) as AudioClip;
            SoundManager.PlaySFXRandomized(sfx);
            Game.Instance.IncreaseCash(3);
        }
        else if (distance <= goodDistance)
        {
            Game.Instance.impatience += 1;
            sfx = Resources.Load("Sounds/Notes/Bien0" + slot) as AudioClip;
            SoundManager.PlaySFXRandomized(sfx);
            Game.Instance.IncreaseCash(2);
        }
        else if (distance <= mediumDistance)
        {
            Game.Instance.impatience += 2;
            sfx = Resources.Load("Sounds/Notes/OK0" + slot) as AudioClip;
            SoundManager.PlaySFXRandomized(sfx);
            Game.Instance.IncreaseCash(1);
        }
        else
        {
            Game.Instance.impatience += 3;
            sfx = Resources.Load("Sounds/Notes/Rate0" + slot) as AudioClip;
            SoundManager.PlaySFXRandomized(sfx);
        }

        Game.Instance.impatience = Mathf.Min(Game.Instance.impatience, Game.Instance.maxImpatience);

    }

    public void ClearAllNotes()
    {
        canSale = false;
        StopAllCoroutines();
        foreach (Transform note in noteSlot1)
        {
            if (note)
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

        if (!canSale || Game.Instance.saleState != Game.SaleState.Talk)
            yield break;

        Transform note;
        switch (slot)
        {
            case 1:
                note = Instantiate(notePrefab, new Vector3(slot1.position.x, -3, 0), Quaternion.identity) as Transform;
                note.GetComponent<Note>().slot = Note.Slot.Slot1;
                note.GetComponent<SpriteRenderer>().sprite = spriteSlot1;
                noteSlot1.Add(note);
                break;
            case 2:
                note = Instantiate(notePrefab, new Vector3(slot2.position.x, -3, 0), Quaternion.identity);
                note.GetComponent<Note>().slot = Note.Slot.Slot2;
                note.GetComponent<SpriteRenderer>().sprite = spriteSlot2;
                noteSlot2.Add(note);
                break;
            case 3:
                note = Instantiate(notePrefab, new Vector3(slot3.position.x, -3, 0), Quaternion.identity);
                note.GetComponent<Note>().slot = Note.Slot.Slot3;
                note.GetComponent<SpriteRenderer>().sprite = spriteSlot3;
                noteSlot3.Add(note);
                break;
            case 4:
                note = Instantiate(notePrefab, new Vector3(slot4.position.x, -3, 0), Quaternion.identity);
                note.GetComponent<Note>().slot = Note.Slot.Slot4;
                note.GetComponent<SpriteRenderer>().sprite = spriteSlot4;
                noteSlot4.Add(note);
                break;
        }
    }
}
