using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missionSpawner : MonoBehaviour
{
	public GameObject[] missions;
	private GameObject mission;
	private GameObject box;
	
	private ParticleSystem readyParticles;
	
	private float spawnTime;
	
	private float time;
	public float minTime;
	public float maxTime;
	private float rayLength;
	
	private int missionType;
	
	public int missionCount;
	public int missionMax;
	public int allMissions;
	
	public float speed;
	public float amount;
	
	private string spawnerMessage;
	
	private bool hasMissions;
			
	public Text missionCountText;
	
  void Start()
  {
		missionCount = 0;
		missionMax = 3;
		allMissions = 0;
		rayLength = 3f;
		time = 0f;
		minTime = 3f;
		maxTime = 5f;
		speed = 20f;
		amount = 0.01f;
		
		readyParticles = GetComponentInChildren<ParticleSystem>();
		spawnerMessage = "Waiting on new missions!";
		
		box = GameObject.Find("FrontStopper");
		Physics.IgnoreCollision(GetComponent<Collider>(), box.GetComponent<Collider>());
  }

  void Update()
  {
		missionCountText.text = spawnerMessage;
		
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
		if (time > spawnTime)
		{
			if ((missionCount < missionMax) && (allMissions < 4))
			{
				readyParticles.Play();
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
		
		if (hasMissions)
		{
			Vector3 boxPosition = transform.position;
			boxPosition.z += (Mathf.Sin(Time.time * speed) * amount);
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
