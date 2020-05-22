using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionsPickup : MonoBehaviour
{
	private GameObject[] players;
	private GameObject player;	
	private GameObject closestPlayer;	
	
	private Vector3 offsetDetect = new Vector3(0, 3, 0);
	private Vector3 offsetCarry = new Vector3(0.6f, 1.65f, 0.2f);
	private Vector3 forceDirection = new Vector3(0, 0, 1);
	private Vector3 latestPosition;
	
	private float currentDistanceToPlayers;
	private float currentDistanceToClosestPlayer;
	private float distance;
	
	private bool carried;
	private bool playerCarrying;
	private bool playerInMission;
	
	public int totalPoints;
	public int currentPoints;
	
	private float rangeEyes;	
	private float rangeFloor;
	public float thrust;
	
	private AudioSource pickupDropMission;
	public AudioClip pickupSound;
	public AudioClip dropSound;
	public AudioClip yeetSound;
	
	private string globalName;
	private string localName;
		
  void Start()
  {
    GetComponent<Rigidbody>().useGravity = true;
		rangeEyes = 2f;
		rangeFloor = 1.2f;
		thrust = 10f;
		
		totalPoints = 10;
		currentPoints = 0;
		
		carried = false;
		playerCarrying = false;
		playerInMission = false;
		
		pickupDropMission = gameObject.GetComponent<AudioSource>();
		
		globalName = gameObject.name;
		localName = "currentMission";
		
		GetComponent<Rigidbody>().isKinematic = true;
  }
	
	GameObject findClosestPlayer()
	{
		players = GameObject.FindGameObjectsWithTag("Player");		
    currentDistanceToPlayers = 400f;
		
    foreach (GameObject player in players)
     {
      currentDistanceToClosestPlayer = (player.transform.position - transform.position).sqrMagnitude;
      if (currentDistanceToClosestPlayer < currentDistanceToPlayers)
      {
        closestPlayer = player;
        currentDistanceToPlayers = currentDistanceToClosestPlayer;
      }
    }
    return closestPlayer;
  }

	void Update()
	{
		player = findClosestPlayer();
		playerCarrying = player.GetComponent<avatarControls>().carrying;
		playerInMission = player.GetComponent<avatarControls>().inMission;
		
		if (!carried && !playerInMission && !playerCarrying)
		{
			if (player.GetComponent<avatarInputs>().actionE)
			{
				if (((player.transform.position - transform.position).sqrMagnitude < rangeFloor*rangeFloor) ||
						((player.transform.position + offsetDetect - transform.position).sqrMagnitude < rangeEyes*rangeEyes))
				{
					pickup();
					carried = true;
					player.GetComponent<avatarControls>().inMission = carried;
				}
			}
		}
		else if (carried)
		{			
			player.GetComponent<avatarControls>().inMission = playerInMission;
			transform.localPosition = offsetCarry;
			if (player.GetComponent<avatarInputs>().actionE)
			{
				drop();
				carried = false;				
				player.GetComponent<avatarControls>().inMission = carried;
			}
			else if (player.GetComponent<avatarInputs>().actionT)
			{
				yeet();
				GetComponent<Rigidbody>().AddForce(player.transform.forward * thrust, ForceMode.Impulse);
				carried = false;
				player.GetComponent<avatarControls>().inMission = carried;
			}
		}
	}
	
	void pickup()
	{		
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Collider>().enabled = false;
		
		gameObject.name = localName;
		
		pickupDropMission.clip = pickupSound;
		pickupDropMission.Play();

		transform.SetParent(player.transform);
		transform.localRotation = Quaternion.identity;
	}

	void drop()
	{
		transform.SetParent(null);
		
		gameObject.name = globalName;
		latestPosition = transform.position + transform.forward*0.3f;
		
		pickupDropMission.clip = dropSound;
		pickupDropMission.Play();
		
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Collider>().enabled = true;
		transform.position = latestPosition;
	}
	
	void yeet()
	{
		transform.SetParent(null);
		
		gameObject.name = globalName;
		
		pickupDropMission.clip = yeetSound;
		pickupDropMission.Play();
		
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Collider>().enabled = true;
	}
}
