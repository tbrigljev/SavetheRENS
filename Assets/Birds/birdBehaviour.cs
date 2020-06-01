using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdBehaviour : MonoBehaviour
{
	private GameObject[] birds;
	private GameObject bird;
	
	enum birdBehaviours{
		sing,
		preen,
		ruffle,
		peck,
	}

	float[,] positions = new float[,] {
		{7.1f, -0.2f},
		{7f, -10f},
		{-9.1f, -0.2f},
		{-9f, -10f},
		{-15.1f, -0.2f},
		{-15f, -10f}
	};
	
	public int currentPosition;
	private int choosePosition;
	private List<int> otherPositions = new List<int>();
	
	private Vector3 start;
	private Vector3 destination;
	private Vector3 originalDestination;
	private Vector3 finalLook;
	
	private Quaternion lookRotation;
	
	public AudioClip song1;
	public AudioClip song2;
	public AudioClip flyAway1;
	public AudioClip flyAway2;
	
	Animator anim;
	
	private bool idle = true;
	private bool onGround = true;
	private bool upwards = false;
	private bool downwards = false;
	
	public bool timeToFly = false;
	
	private int idleAnimationHash;
	private int singAnimationHash;
	private int ruffleAnimationHash;
	private int preenAnimationHash;
	private int peckAnimationHash;
	private int flyAnimationHash;
	private int landingAnimationHash;
	
	private int peckBoolHash;
	private int ruffleBoolHash;
	private int preenBoolHash;
	private int singTriggerHash;
	private int flyingBoolHash;
	private int landingBoolHash;
	private int flyingDirectionHash;
	
	private float rand;
	private float step;
	private float verticalOffsetUpwards;
	private float verticalOffsetDownwards;
	private float flyingDistance;
	private float timerToFlap;
	private float timerToFly;
	private float turnrate;
	
	public float speed;
	
  void Start()
  {		
		timeToFly = false;
    anim = gameObject.GetComponent<Animator>();
		idleAnimationHash = Animator.StringToHash("Base Layer.Idle");
		
		peckBoolHash = Animator.StringToHash("peck");
		ruffleBoolHash = Animator.StringToHash("ruffle");
		preenBoolHash = Animator.StringToHash("preen");
		singTriggerHash = Animator.StringToHash ("sing");
		
		flyingBoolHash = Animator.StringToHash("flying");
		landingBoolHash = Animator.StringToHash("landing");
		flyingDirectionHash = Animator.StringToHash("flyingDirection");
		
		speed = 3;
		verticalOffsetUpwards = 6;
		verticalOffsetDownwards = 0.5f;
		flyingDistance = 0;
		
		finalLook = new Vector3(0, 6, 0);
		choosePosition = 0;
		
		timerToFly = Random.Range(10, 20);
		
		switch (gameObject.name)
		{
			case "BlueJay":
				currentPosition = 0;
				break;
			case "Cardinal":
				currentPosition = 1;
				break;
			case "Chickadee":
				currentPosition = 2;
				break;
			case "GoldFinch":
				currentPosition = 3;
				break;
			case "Robin":
				currentPosition = 4;
				break;
			case "Sparrow":
				currentPosition = 5;
				break;
		}
  }
	
	void getOthersPositions()
	{
		otherPositions.Clear();
		birds = GameObject.FindGameObjectsWithTag("Birds");
		foreach (GameObject bird in birds)
		{
			otherPositions.Add(bird.GetComponent<birdBehaviour>().currentPosition);
		}
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
		switch (behaviour)
		{
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
	
	void Flying(Vector3 start, Vector3 destination, float flyingDistance)
	{
		originalDestination = destination;
		idle = false;
		upwards = (flyingDistance/2 < (transform.position - destination).sqrMagnitude);
		downwards = ((transform.position - destination).sqrMagnitude < 5) ? false : true;
		
		if (upwards)
		{
			destination.y += (verticalOffsetUpwards/360)*flyingDistance;
		}
		if (downwards)
		{
			destination.y += (verticalOffsetDownwards/360)*flyingDistance;
		}
		
		step = speed * Time.deltaTime;
		if (Mathf.Abs(transform.position.x - destination.x) < 0.5f && Mathf.Abs((transform.position.z - destination.z)) < 0.05f)
		{
			anim.SetBool(flyingBoolHash, false);
			anim.SetBool(landingBoolHash, true);
			
			lookRotation = Quaternion.LookRotation((finalLook - transform.position).normalized);
			turnrate = 2f;
			
			if (Quaternion.Angle(transform.rotation, lookRotation) > 3f)
			{
				destination.y += (0.3f/360)*flyingDistance;
			}
			else if (transform.position == originalDestination)
			{
				timeToFly = false;
				onGround = true;
				idle = true;
				anim.SetBool(landingBoolHash, false);
			}
		}
		else
		{
			lookRotation = Quaternion.LookRotation((destination - transform.position).normalized);
			turnrate = 2f;
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnrate*Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, destination, step);
		
		timerToFlap -= Time.deltaTime;
		if (timerToFlap < 0)
		{
			timerToFlap = Random.Range(1f, 3f);
			
			if (Random.value < 0.5)
			{
				GetComponent<AudioSource>().PlayOneShot(flyAway1,.8f);
			}
			else
			{
				GetComponent<AudioSource>().PlayOneShot(flyAway2,.8f);
			}
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
		timerToFly -= Time.deltaTime;
		if(timerToFly < 0)
		{
			timeToFly = true;
			timerToFly = Random.Range(15, 30);
		}
		if (timeToFly)
		{
			getOthersPositions();
			
			if (onGround)
			{
				choosePosition = Random.Range(0, 6);
				if (!otherPositions.Contains(choosePosition))
				{
					currentPosition = choosePosition;
					onGround = false;
					destination.x = positions[currentPosition, 0];
					destination.y = transform.position.y;
					destination.z = positions[currentPosition, 1];
					flyingDistance = (transform.position - destination).sqrMagnitude;
					start = transform.position;
					timerToFlap = 0;
				}
			}
			else
			{
				anim.SetBool(flyingBoolHash, true);
				anim.SetBool(landingBoolHash, false);
				Flying(start, destination, flyingDistance);
			}
		}
    if(onGround)
		{
			OnGroundBehaviours();
		}
  }
}
