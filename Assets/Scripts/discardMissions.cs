using System.Collections;
using System.Collections.Generic;
using UnityEngine;             
using UnityEngine.UI;

public class discardMissions : MonoBehaviour
{
	private GameObject[] players;
	private GameObject player;
	private GameObject closestPlayer;	
	private GameObject boxReady;
	private GameObject missions;
	
	private Vector3 offset  = new Vector3(0, 2, 0);
	
	public GameObject boxReadyPrefab;
	public GameObject boxCooldownPrefab;
	
	private float rayLength;	
	private float currentDistanceToPlayers;
	private float currentDistanceToClosestPlayer;
	
	private float time;
	public float maxTime;
	
	private bool readyForMission;
	private bool cooldown;
	
	public Text missionDiscardProgress;
	
	private string stationMessage;

	void Start()
	{
		rayLength = 1.5f;
		time = 0f;
		maxTime = 3f;
		
		readyForMission = false;
		cooldown = false;
		
		boxReady = null;
		player = null;
		closestPlayer = null;
		
		missions = GameObject.Find("Missions");
		
		stationMessage = "Station ready!";
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
		//time += Time.deltaTime;
		
		RaycastHit hit;
		Physics.Raycast(transform.position + offset, transform.forward, out hit, rayLength*100);
		// && (hit.distance < rayLength)
		if ((hit.collider.gameObject.name == player.name) && (player.GetComponent<avatarControls>().inMission) && (hit.distance < rayLength))
		{
			readyForMission = true;
		}
		else
		{
			readyForMission = false;
		}
		
		/*f (time > maxTime)
		{
			Debug.Log("Closest player is: " + player.name);
			Debug.Log("Ready for mission is: " + readyForMission);
			time = 0f;
		}*/
	}
  
	void Update()
	{
		missionDiscardProgress.text = stationMessage;
		
		if ((readyForMission) && (!cooldown))
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
			
			if (Input.GetKey(KeyCode.R))
			{
				if (time < maxTime)
				{
					time += Time.deltaTime;
					stationMessage = "Progress: " + Mathf.Round(Mathf.Round(time/maxTime*100)/10)*10 + "%";
				}
				else if (time > maxTime)
				{
					stationMessage = "Discard completed!";
					missions.GetComponent<missionSpawner>().missionCount -= 1;
					cooldown = true;
				}
			}
		}
		else if (cooldown)
		{
			Destroy(boxReady);
			boxReady = Instantiate(boxCooldownPrefab, transform.position, Quaternion.identity);
			if (time > 0f)
			{
				stationMessage = "Station in cooldown: " + Mathf.Round(Mathf.Round(time/maxTime*100)/10)*10 + "%";
				time -= Time.deltaTime*2;
			}
			else
			{
				stationMessage = "Station ready!";
				cooldown = false;
			}
		}
		else if ((!readyForMission) && (!cooldown))
		{
			Destroy(boxReady);
		}
    Debug.DrawRay(transform.position + offset, transform.forward*rayLength, Color.blue);
	}
}
