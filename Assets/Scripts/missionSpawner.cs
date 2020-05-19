using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missionSpawner : MonoBehaviour
{
	public GameObject[] missions;
	private GameObject mission;
	
	private float spawnTime;
	
	private float time;
	public float minTime;
	public float maxTime;
	private float rayLength;
	
	private int missionType;
	
	public int missionCount;
	public int startTime;	
	public int missionMax;
			
	public Text missionCountText;
	
  void Start()
  {
		missionCount = 0;
		rayLength = 3f;
		time = 0f;
  }

  void Update()
  {
    spawnTime = Random.Range(minTime, maxTime);
		
		time += Time.deltaTime;
		if (time > spawnTime)
		{
			if (missionCount < missionMax)
			{
				missionType = Random.Range(0, missions.Length);				
				missionCount = missionCount + 1;
				missionCountText.text = "Missions: " + missionCount.ToString();
				Vector3 spawnPosition = transform.position + transform.forward*2*missionCount;
				mission = Instantiate(missions[missionType], spawnPosition, gameObject.transform.rotation);				
				mission.name = "missionReady" + missionCount.ToString();
			}
			time = 0f;
		}
		
		Vector3 offset  = new Vector3(0, 2, 0);
    Debug.DrawRay(transform.position, transform.forward*rayLength, Color.blue);
  }
}
