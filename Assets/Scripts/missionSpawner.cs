using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missionSpawner : MonoBehaviour
{
	public GameObject[] missions;
	private GameObject mission;
	private GameObject box;
	private GameObject missionCountText;
	private GameObject globalModifiers;
	
	private ParticleSystem readyParticles;
	
	private float time;
	private float spawnTime;
	private float rayLength;

	public float minTime;
	public float maxTime;
	public float shakeSpeed;
	public float shakeAmount;	
	
	private int missionType;
	
	public int missionCount;
	public int missionMax;
	public int allMissions;
	
	private string spawnerMessage;
	
	private AudioSource newMissionSound;
	
	private bool hasMissions;
			
	//private Text missionCountText;
	
  void Start()
  {
		missionCount = 0;
		missionMax = 3;
		allMissions = 0;
		rayLength = 3f;
		time = 0f;
		minTime = 3f;
		maxTime = 5f;
		shakeSpeed = 20f;
		shakeAmount = 0.01f;
		
		readyParticles = GetComponentInChildren<ParticleSystem>();
		spawnerMessage = "Waiting on new missions!";
		
		newMissionSound = gameObject.GetComponent<AudioSource>();
		
		box = GameObject.Find("FrontStopper");
		Physics.IgnoreCollision(GetComponent<Collider>(), box.GetComponent<Collider>());
		
		globalModifiers = GameObject.Find("Global");
		
		
		missionCountText = GameObject.Find("GlobalCounting");
  }

  void Update()
  {
		missionCountText.GetComponent<Text>().text = spawnerMessage;
		
		if (missionCount > 0)
		{
			spawnerMessage = "Missions: " + missionCount.ToString();
		}
		else if (allMissions >= 4)
		{
			spawnerMessage = "No more missions!";
		}
		else
		{
			spawnerMessage = "Waiting on new missions!";			
		}
		
    spawnTime = Random.Range(minTime, maxTime);
		
		time += Time.deltaTime;
		if ((time > spawnTime) && !(globalModifiers.GetComponent<globalModifiers>().gameOver))
		{
			if ((missionCount < missionMax) && (allMissions < 4))
			{
				readyParticles.Play();
				newMissionSound.Play();
				missionType = Random.Range(0, missions.Length);				
				missionCount += 1;
				allMissions += 1;
				Vector3 spawnPosition = transform.position;
				spawnPosition.x += 0.2f;
				spawnPosition.y += 0.7f - (0.1f*missionCount);
				if (missionCount < 10)
				{
					spawnPosition.z -=0.2f;
				}
				else if (missionCount < 20)
				{
					spawnPosition.z -=0.0f;
				}
				else if (missionCount < 30)
				{
					spawnPosition.z +=0.2f;
				}
				mission = Instantiate(missions[missionType], spawnPosition, gameObject.transform.rotation);				
				mission.name = "missionReady" + missionCount.ToString();
			}
			time = 0f;
		}
		
		if (hasMissions && !(globalModifiers.GetComponent<globalModifiers>().gameOver))
		{
			Vector3 boxPosition = transform.position;
			boxPosition.z += (Mathf.Sin(Time.time * shakeSpeed) * shakeAmount);
			transform.position = boxPosition;
			hasMissions = false;
		}
		
    Debug.DrawRay(transform.position, transform.forward*rayLength, Color.blue);
  }
	
	void OnTriggerStay(Collider collider)
	{
		hasMissions = true;
	}
}
