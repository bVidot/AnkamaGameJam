using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

    public enum Slot { Slot1 = 1,
                       Slot2 = 2,
                       Slot3 = 3,
                       Slot4 = 4 }
    public Slot slot;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(gameObject != null)
        {
            float posY = transform.position.y;
            posY += Game.Instance.noteSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, posY, -9);

            if (transform.position.y >= 11f)
            {
                Game.Instance.impatience += 3;
                Game.Instance.impatience = Mathf.Min(Game.Instance.impatience, 30);
                DeleteNote();
            }
        }    
	}

    public void DeleteNote(bool clear = false)
    {
        if(!clear)
        {
            if (slot == Slot.Slot1)
                Game.Instance.noteSlot1.Remove(transform);
            else if (slot == Slot.Slot2)
                Game.Instance.noteSlot2.Remove(transform);
            else if (slot == Slot.Slot3)
                Game.Instance.noteSlot3.Remove(transform);
            else if (slot == Slot.Slot4)
                Game.Instance.noteSlot4.Remove(transform);
        }
        
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (Game.Instance.canSale)
            Game.Instance.StartCoroutine(Game.Instance.SpawnNote((int)slot, Game.Instance.delayBetweenNote + Random.Range(-1f, 1f)));
    }
}
