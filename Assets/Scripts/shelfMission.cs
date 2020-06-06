using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shelfMission : MonoBehaviour
{
	private GameObject[] players;
	private GameObject player;
	private GameObject closestPlayer;	
	private GameObject boxReady;
	private GameObject globalModifiers;
	//--//
	private GameObject canvas;
	private Vector3 lookAtPoint;
	
	private string identifier = "shelf";
	
	private Vector3 raycastOffset = new Vector3(0, 2, 0);
	private Quaternion canvasAngle;
	
	public GameObject boxReadyPrefab;
	public GameObject boxCooldownPrefab;
	
	private float rayLength;	
	private float currentDistanceToPlayers;
	private float currentDistanceToClosestPlayer;
	
	private float completeMissionsModifier;
	private float cooldownMissionsModifier;
	
	private float time;
	private float progress;
	public float maxTimeComplete;
	public float maxTimeCooldown;
	
	private bool readyForMission;
	private bool cooldown;
	
	//private AudioSource shelfMissionSound;
	
	public Image progressBar;
	public Image cooldownBar;
	public GameObject shelfCompletePrefab;	
	
	private bool taskDone;
	
  void Start()
	{
		//--//
		lookAtPoint.y = transform.position.y + 10;
		switch (transform.eulerAngles.y % 180)
		{
			case 0:
				lookAtPoint.x = transform.position.x;
				lookAtPoint.z = transform.position.z + 4;
				break;
			default:
				lookAtPoint.x = (transform.position.x > 0) ? transform.position.x - 4 : transform.position.x + 4;	
				lookAtPoint.z = transform.position.z;
				break;			
		}
		canvas = transform.Find("Canvas").gameObject;
		canvas.transform.LookAt(lookAtPoint);
		canvasAngle = canvas.transform.rotation;
		
		rayLength = 3f;
		time = 0f;
		maxTimeComplete = 2f;
		maxTimeCooldown = 1f;
		
		readyForMission = false;
		cooldown = false;
		
		boxReady = null;
		player = null;
		closestPlayer = null;
		
		globalModifiers = GameObject.Find("Global");
		completeMissionsModifier = globalModifiers.GetComponent<globalModifiers>().completeMissionsModifier;
		cooldownMissionsModifier = globalModifiers.GetComponent<globalModifiers>().cooldownMissionsModifier;
		
		//shelfMissionSound = gameObject.GetComponent<AudioSource>();
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
	
	void FixedUpdate()
	{
		player = findClosestPlayer();
		
		RaycastHit hit;
		Physics.Raycast(transform.position + raycastOffset, transform.forward, out hit, rayLength*100);
		
		if ((hit.collider.gameObject.name == player.name) && (player.GetComponent<avatarControls>().inMission) && (hit.distance < rayLength) && (player.GetComponent<avatarMissions>().currentTag == identifier))
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
		//--//
		canvas.transform.LookAt(lookAtPoint);
		
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

		if ((readyForMission) && (!cooldown))
		{
			if (boxReady == null)
			{
				boxReady = Instantiate(boxReadyPrefab, transform.position, transform.rotation, transform);
			}
			
			if (player.GetComponent<avatarInputs>().actionR)
			{
				if (time < maxTimeComplete)
				{
					time += Time.deltaTime;
					progress = time/maxTimeComplete;
					progressBar.fillAmount = progress;
				}
				else if (time > maxTimeComplete)
				{
					player.GetComponent<avatarMissions>().taskDone = true;
					cooldown = true;
					Destroy(boxReady);
					
					showCompleteShelf();
					//shelfMissionSound.Play();
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
				progress = (time - (maxTimeComplete - maxTimeCooldown))/maxTimeCooldown;
				cooldownBar.fillAmount = progress;
			}
			else
			{
				Destroy(boxReady);
				cooldown = false;
			}
		}
		else if ((!readyForMission) && (!cooldown))
		{
			if (boxReady != null)
			{
				time = 0f;
				Destroy(boxReady);
				progressBar.fillAmount = 0f;
			}
		}
    Debug.DrawRay(transform.position + raycastOffset, transform.forward*rayLength, Color.blue);
	}
	
	void showCompleteShelf()
	{
		//var rot = transform.eulerAngles + 180f * Vector3.up;
		Instantiate(shelfCompletePrefab, transform.position, canvasAngle, transform);		
	}
}
