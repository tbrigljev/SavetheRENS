using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class discardMissions : MonoBehaviour
{
	private GameObject[] players;
	private GameObject player;
	private GameObject closestPlayer;	
	private GameObject boxReady;
	
	private Vector3 offset  = new Vector3(0, 2, 0);
	
	public GameObject boxReadyPrefab;
	
	private float rayLength;	
	private float currentDistanceToPlayers;
	private float currentDistanceToClosestPlayer;
	
	private float time;
	private float maxTime;
	
	private bool readyForMission;

	void Start()
	{
		rayLength = 1.5f;
		time = 0f;
		maxTime = 3f;
		
		readyForMission = false;
		
		boxReady = null;
		player = null;
		closestPlayer = null;
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
	
	void FixedUpdate()
	{
		player = findClosestPlayer();
		time += Time.deltaTime;

		
		RaycastHit hit;
		Physics.Raycast(transform.position + offset, transform.forward, out hit, rayLength*100);
		// && (hit.distance < rayLength)
		if ((hit.collider.gameObject.name == player.name) && (player.GetComponent<avatarControls>().inMission) && (hit.distance < rayLength))
		{
			Debug.Log("Hit detected");
			readyForMission = true;
		}
		else
		{
			readyForMission = false;
		}
		
		if (time > maxTime)
		{
			Debug.Log("Closest player is: " + player.name);
			Debug.Log("Ready for mission is: " + readyForMission);
			time = 0f;
		}
	}
  
	void Update()
	{
		
		if (readyForMission)
		{
			if (boxReady == null)
			{
				boxReady = Instantiate(boxReadyPrefab, transform.position, Quaternion.identity);
			}
			else if (boxReady != null)
			{
				boxReady.transform.position = transform.position;
				boxReady.transform.rotation = transform.rotation;
			}
		}
		else if (!readyForMission)
		{
			Destroy(boxReady);
		}
    Debug.DrawRay(transform.position + offset, transform.forward*rayLength, Color.blue);
	}
}
