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
	public float progress;
	public float maxTime;
	
	private bool readyForMission;
	public bool cooldown;
	
	public Text missionDiscardProgress;
	
	private string stationMessage;
	
	public Image progressBar;
	public Image cooldownBar;
	public GameObject discardCompletePrefab;

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
				boxReady = Instantiate(boxReadyPrefab, transform.position, transform.rotation);
			}
			
			if (Input.GetKey(KeyCode.R))
			{
				if (time < maxTime)
				{
					time += Time.deltaTime;
					progress = time/maxTime;
					stationMessage = "Progress: " + Mathf.Round(Mathf.Round(progress*100)/10)*10 + "%";
					progressBar.fillAmount = progress;
				}
				else if (time > maxTime)
				{
					Destroy(player.transform.Find("currentMission").gameObject);					
					stationMessage = "Discard completed!";
					missions.GetComponent<missionSpawner>().missionCount -= 1;
					cooldown = true;
					Destroy(boxReady);
					player.GetComponent<avatarControls>().inMission = false;
					
					showCompleteDiscard();
				}
			}
		}
		else if (cooldown)
		{
			if (boxReady == null)
			{
				progressBar.fillAmount = 0f;
				boxReady = Instantiate(boxCooldownPrefab, transform.position, transform.rotation);
			}
			
			if (time > 0f)
			{
				time -= Time.deltaTime;
				progress = time/maxTime;				
				stationMessage = "Station in cooldown: " + Mathf.Round(Mathf.Round(progress*100)/10)*10 + "%";
				cooldownBar.fillAmount = progress;
			}
			else
			{
				Destroy(boxReady);
				stationMessage = "Station ready!";
				cooldownBar.fillAmount = 0f;
				cooldown = false;
			}
		}
		else if ((!readyForMission) && (!cooldown))
		{
			if (boxReady != null)
			{
				time = 0f;
				Destroy(boxReady);
				stationMessage = "Station ready!";
			}
		}
    Debug.DrawRay(transform.position + offset, transform.forward*rayLength, Color.blue);
	}
	
	void showCompleteDiscard()
	{
		offset.y = 4;
		Instantiate(discardCompletePrefab, transform.position + offset, Quaternion.identity, transform);
		
	}
}
