using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdBehaviour : MonoBehaviour
{
	enum birdBehaviours{
		sing,
		preen,
		ruffle,
		peck,
	}
	
	public AudioClip song1;
	public AudioClip song2;
	public AudioClip flyAway1;
	public AudioClip flyAway2;
	
	Animator anim;
	
	private bool idle = true;
	private bool onGround = true;
	
	private int idleAnimationHash;
	private int singAnimationHash;
	private int ruffleAnimationHash;
	private int preenAnimationHash;
	private int peckAnimationHash;
	
	private int peckBoolHash;
	private int ruffleBoolHash;
	private int preenBoolHash;
	private int singTriggerHash;
	
	private float rand;
	
  void Start()
  {
    anim = gameObject.GetComponent<Animator>();
		idleAnimationHash = Animator.StringToHash("Base Layer.Idle");
		
		peckBoolHash = Animator.StringToHash("peck");
		ruffleBoolHash = Animator.StringToHash("ruffle");
		preenBoolHash = Animator.StringToHash("preen");
		singTriggerHash = Animator.StringToHash ("sing");
  }
	
	void OnGroundBehaviours()
	{
		idle = anim.GetCurrentAnimatorStateInfo(0).nameHash == idleAnimationHash;
		if(idle)
		{
			if (Random.value < Time.deltaTime*.33)
			{
				rand = Random.value;
				if (rand < .25)
				{
					DisplayBehaviour(birdBehaviours.sing);
				}
				else if (rand < .5)
				{
					DisplayBehaviour(birdBehaviours.preen);	
				}
				else if (rand<.75)
				{
					DisplayBehaviour(birdBehaviours.ruffle);	
				}
				else
				{
					DisplayBehaviour(birdBehaviours.peck);	
				}
			}
		}
	}
	
	void DisplayBehaviour(birdBehaviours behaviour)
	{
		idle = false;
		switch (behaviour){
		case birdBehaviours.sing:
			anim.SetTrigger(singTriggerHash);			
			break;
		case birdBehaviours.ruffle:
			anim.SetTrigger(ruffleBoolHash);
			break;
		case birdBehaviours.preen:
			anim.SetTrigger(preenBoolHash);			
			break;
		case birdBehaviours.peck:
			anim.SetTrigger(peckBoolHash);			
			break;
		}
	}
	
	void PlaySong()
	{
		if(Random.value < .5)
		{
			GetComponent<AudioSource>().PlayOneShot(song1, 1);
		}
		else
		{
			GetComponent<AudioSource>().PlayOneShot(song2, 1);
		}
	}

  void Update()
  {
    if(onGround)
		{
			OnGroundBehaviours();
		}
  }
}
