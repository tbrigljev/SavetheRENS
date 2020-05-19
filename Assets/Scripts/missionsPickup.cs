using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionsPickup : MonoBehaviour
{
	private GameObject[] players;
	private GameObject player;	
	private GameObject closestPlayer;	
	
	private Vector3 offsetDetect = new Vector3(0, 2, 0);
	private Vector3 offsetCarry = new Vector3(0.6f, 1.65f, 0.2f);
	private Vector3 forceDirection = new Vector3(0, 0, 1);
	
	private float currentDistanceToPlayers;
	private float currentDistanceToClosestPlayer;
	private float distance;
	
	private bool carrying;
	
	public float range;	
	public float thrust;
		
  void Start()
  {
    GetComponent<Rigidbody>().useGravity = true;
		range = 1.2f;
		thrust = 2f;
  }
	
	GameObject findClosestPlayer()
	{
		players = GameObject.FindGameObjectsWithTag("Player");		
    currentDistanceToPlayers = 200f;
		
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
		if (carrying == false)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				if (((player.transform.position - transform.position).sqrMagnitude < range*range) ||
						((player.transform.position + offsetDetect - transform.position).sqrMagnitude < range*range))
				{
					pickup();
					carrying = true;
				}
			}
		}
		else if (carrying == true)
		{
			
			player.GetComponent<avatarControls>().inMission = carrying;
			transform.localPosition = offsetCarry;
			if (Input.GetKeyDown(KeyCode.E))
			{
				drop();
				carrying = false;				
				player.GetComponent<avatarControls>().inMission = carrying;
			}
			else if (Input.GetKeyDown(KeyCode.T))
			{
				yeet();
				GetComponent<Rigidbody>().AddForce(player.transform.forward * thrust, ForceMode.Impulse);
				carrying = false;
				player.GetComponent<avatarControls>().inMission = carrying;
			}
		}
	}
	
	void pickup()
	{
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Collider>().enabled = false;

		transform.SetParent(player.transform);
		transform.localRotation = Quaternion.identity;
	}

	void drop()
	{
		transform.SetParent(null);
		
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Collider>().enabled = true;
	}
	
	void yeet()
	{
		transform.SetParent(null);
		
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Collider>().enabled = true;
	}
}
