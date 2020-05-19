using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missionSpawner : MonoBehaviour
{
	public GameObject[] missions;
	private GameObject mission;
	
	private float spawnTime;
	
	public float minTime;
	public float maxTime;
	
	private int missionType;
	
	public int missionCount;
	public int startTime;	
	public int missionMax;
			
	public Text missionCountText;
	
  void Start()
  {
		missionCount = 0;
		StartCoroutine(Spawner());
  }

  void Update()
  {
    spawnTime = Random.Range(minTime, maxTime);
  }
	
	IEnumerator Spawner()
	{
		yield return new WaitForSeconds(startTime);
		
		while(missionCount < missionMax)
		{
			missionType = Random.Range(0,3);
			missionCount = missionCount + 1;
			missionCountText.text = "Missions: " + missionCount.ToString();
			
			Vector3 spawnPosition = transform.position + transform.forward*2*missionCount;
			//Debug.Log(spawnPosition);
						
			mission = Instantiate(missions[missionType], spawnPosition, gameObject.transform.rotation);
			mission.name = "missionReady" + missionCount.ToString();
			
			yield return new WaitForSeconds(spawnTime);
		}
	}	
}
