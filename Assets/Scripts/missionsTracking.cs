using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionsTracking : MonoBehaviour
{
	private GameObject missionSpawner;
	
	public List<string> missionTags = new List<string>();
	public List<int> missionPoints = new List<int>();
	
	public string currentTag;
	public int currentPoints;
	public int currentPosition;	
	public int totalPoints;
	
	public bool taskDone;
	
  void Start()
	{
		taskDone = false;
		currentPosition = 0;
		totalPoints = 0;
		
		missionSpawner = GameObject.Find("Missions");
		
		for (int i = 0; i < missionSpawner.GetComponent<missionSpawner>().missionTags.Count; i++)
		{
			missionTags.Add(missionSpawner.GetComponent<missionSpawner>().missionTags[i]);
			missionPoints.Add(missionSpawner.GetComponent<missionSpawner>().missionPoints[i]);
		}
  }
	
	void Update()
	{
		currentTag = missionTags[currentPosition];
		currentPoints = missionPoints[currentPosition];
		
		if (taskDone)
		{
			totalPoints += currentPoints;
			currentPosition += 1;			
			taskDone = false;
		}
	}
}
