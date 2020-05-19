using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missionSpawner : MonoBehaviour
{
	public GameObject[] missions;
	private GameObject mission;
	
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
			
	public Text missionCountText;
	
  void Start()
  {
		missionCount = 0;
		allMissions = 0;
		rayLength = 3f;
		time = 0f;
		
		readyParticles = GetComponentInChildren<ParticleSystem>();
		spawnerMessage = "Waiting on new missions!";
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
			spawnerMessage = "No new missions!";
		}
		else
		{
			spawnerMessage = "No more missions!";			
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
				Vector3 spawnPosition = transform.position + transform.forward*2*missionCount;
				mission = Instantiate(missions[missionType], spawnPosition, gameObject.transform.rotation);				
				mission.name = "missionReady" + missionCount.ToString();
			}
			time = 0f;
		}
		
		if (missionCount > 0)
		{
			Vector3 boxPosition = transform.position;
			boxPosition.z += Mathf.Sin(Time.time * speed) * amount;
			transform.position = boxPosition;
		}		
		
		Vector3 offset  = new Vector3(0, 2, 0);
    Debug.DrawRay(transform.position, transform.forward*rayLength, Color.blue);
  }
}
