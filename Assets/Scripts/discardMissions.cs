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
	private GameObject progressText;	
	private GameObject globalModifiers;
	
	private Vector3 raycastOffset  = new Vector3(0, 2, 0);
	
	public GameObject boxReadyPrefab;
	public GameObject boxCooldownPrefab;
	
	private float rayLength;	
	private float currentDistanceToPlayers;
	private float currentDistanceToClosestPlayer;
	
	private float completeMissionsModifier;
	private float cooldownMissionsModifier;
	
	private float time;
	public float progress;
	public float maxTimeComplete;
	public float maxTimeCooldown;
	
	private bool readyForMission;
	public bool cooldown;
	
	private Text missionDiscardProgress;
	
	private string stationMessage;
	
	private AudioSource discardMissionSound;
	
	public Image progressBar;
	public Image cooldownBar;
	public GameObject discardCompletePrefab;

	void Start()
	{
		rayLength = 1.5f;
		time = 0f;
		maxTimeComplete = 3f;
		maxTimeCooldown = 2f;
		
		readyForMission = false;
		cooldown = false;
		
		boxReady = null;
		player = null;
		closestPlayer = null;
		
		missions = GameObject.Find("Missions");
		
		globalModifiers = GameObject.Find("Global");
		completeMissionsModifier = globalModifiers.GetComponent<globalModifiers>().completeMissionsModifier;
		cooldownMissionsModifier = globalModifiers.GetComponent<globalModifiers>().cooldownMissionsModifier;
		
		discardMissionSound = gameObject.GetComponent<AudioSource>();
		
		//stationMessage = "Station ready!";
		
		//progressText = GameObject.Find("GlobalInteract");
		//missionDiscardProgress = progressText.GetComponent<Text>();
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
		
		RaycastHit hit;
		Physics.Raycast(transform.position + raycastOffset, transform.forward, out hit, rayLength*100);
		if ((hit.collider.gameObject.name == player.name) && (player.GetComponent<avatarControls>().inMission) && (hit.distance < rayLength))
		{
			readyForMission = true;
		}
		else
		{
			readyForMission = false;
		}
	}
  
	void Update()
	{
		if (completeMissionsModifier != globalModifiers.GetComponent<globalModifiers>().completeMissionsModifier)
		{
			completeMissionsModifier = globalModifiers.GetComponent<globalModifiers>().completeMissionsModifier;
			maxTimeComplete /= completeMissionsModifier;
		}
		if (cooldownMissionsModifier != globalModifiers.GetComponent<globalModifiers>().cooldownMissionsModifier)
		{
			cooldownMissionsModifier = globalModifiers.GetComponent<globalModifiers>().cooldownMissionsModifier;
			maxTimeCooldown /= completeMissionsModifier;
		}
		
		//missionDiscardProgress.text = stationMessage;
		
		if ((readyForMission) && (!cooldown))
		{
			if (boxReady == null)
			{
				boxReady = Instantiate(boxReadyPrefab, transform.position, transform.rotation);
			}
			
			if (player.GetComponent<avatarInputs>().actionR)
			{
				if (time < maxTimeComplete)
				{
					time += Time.deltaTime;
					progress = time/maxTimeComplete;
					//stationMessage = "Progress: " + Mathf.Round(Mathf.Round(progress*100)/10)*10 + "%";
					progressBar.fillAmount = progress;
				}
				else if (time > maxTimeComplete)
				{
					Destroy(player.transform.Find("currentMission").gameObject);					
					//stationMessage = "Discard completed!";
					missions.GetComponent<missionSpawner>().missionCount -= 1;
					cooldown = true;
					Destroy(boxReady);
					player.GetComponent<avatarControls>().inMission = false;
					
					showCompleteDiscard();
					discardMissionSound.Play();
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
				progress = time/maxTimeCooldown;				
				//stationMessage = "Station in cooldown: " + Mathf.Round(Mathf.Round(progress*100)/10)*10 + "%";
				cooldownBar.fillAmount = progress;
			}
			else
			{
				Destroy(boxReady);
				//stationMessage = "Station ready!";
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
				//stationMessage = "Station ready!";
			}
		}
    Debug.DrawRay(transform.position + raycastOffset, transform.forward*rayLength, Color.blue);
	}
	
	void showCompleteDiscard()
	{
		var rot = transform.eulerAngles + 180f * Vector3.up;
		Instantiate(discardCompletePrefab, transform.position, Quaternion.Euler(rot), transform);		
	}
}
