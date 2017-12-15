using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    Animator animator;
    public int currentDoorClosing = 0;

    Coroutine moveDoorCoroutine;

    private int desiredPosition = 0;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(desiredPosition < Game.Instance.impatience)
        {
            desiredPosition = Game.Instance.impatience;
            if (moveDoorCoroutine != null)
                StopCoroutine(moveDoorCoroutine);

            moveDoorCoroutine = StartCoroutine(MoveDoor(desiredPosition, false));
        }
        else if(desiredPosition > Game.Instance.impatience)
        {
            desiredPosition = Game.Instance.impatience;
            if (moveDoorCoroutine != null)
                StopCoroutine(moveDoorCoroutine);

            moveDoorCoroutine = StartCoroutine(MoveDoor(desiredPosition, true));
        }
        
    }

    IEnumerator MoveDoor(int desiredPosition, bool open)
    {
  
        while (currentDoorClosing != desiredPosition)
        {
            yield return new WaitForSeconds(0.05f);
            if (open)
                currentDoorClosing = Mathf.Max(0, currentDoorClosing - 1);
            else
                currentDoorClosing = Mathf.Min(currentDoorClosing + 1, Game.Instance.maxImpatience);

            animator.SetFloat("Closing", currentDoorClosing);

        }
        moveDoorCoroutine = null;
        if (currentDoorClosing >= 30)
            Game.Instance.SwitchGuitarHeroToFight();
    }

    public void SetTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }

}
