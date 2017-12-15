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

    public void Interrupt()
    {
        animator.SetTrigger("StartInterrupt");
    }

    public void Fight()
    {
        animator.SetTrigger("StartFight");
    }

    public void Sale()
    {
        animator.SetTrigger("StartIdle");
    }
}
