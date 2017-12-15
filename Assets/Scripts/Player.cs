using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Animator animator;

    public int cash;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTrigger(string trigger, float delay = 0f)
    {
        StartCoroutine(Delay(trigger, delay));
    }

    IEnumerator Delay(string trigger, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(trigger);
    }
    
}
