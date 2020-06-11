using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class missionsTracking : MonoBehaviourPun
{
	private GameObject missionSpawner;

	public List<string> missionTags = new List<string>();
	public List<int> missionPoints = new List<int>();
	public List<string> missionTagsClient = new List<string>();
	public List<int> missionPointsClient = new List<int>();

	public string currentTag;
	public int currentPoints;
	public int currentPosition;
	public int totalPoints;

	private float bonus;

	public bool taskDone;
	//public bool missionDone;
	public bool first; 

	void Start()
	{
		taskDone = false;
		//missionDone = false;
		first = true; 
		currentPosition = 0;
		totalPoints = 0;
		bonus = 1;

		missionSpawner = GameObject.Find("Missions");
		/*if (true || PhotonNetwork.IsMasterClient)
		{
			for (int i = 0; i < missionSpawner.GetComponent<missionSpawner>().missionTags.Count; i++)
			{
				missionTags.Add(missionSpawner.GetComponent<missionSpawner>().missionTags[i]);
				missionPoints.Add(missionSpawner.GetComponent<missionSpawner>().missionPoints[i]);

				
				if (PhotonNetwork.IsMasterClient)
				{
					missionTags.Add(missionSpawner.GetComponent<missionSpawner>().missionTags[i]);
					missionPoints.Add(missionSpawner.GetComponent<missionSpawner>().missionPoints[i]);
				}
				else
				{
					missionTagsClient.Add(missionSpawner.GetComponent<missionSpawner>().missionTags[i]);
					missionPointsClient.Add(missionSpawner.GetComponent<missionSpawner>().missionPoints[i]);
				}
			}
			currentTag = missionTags[currentPosition];
			currentPoints = missionPoints[currentPosition];

			
			if (PhotonNetwork.IsMasterClient && (missionTags.Count > 0))
			{
				currentTag = missionTags[currentPosition];
				currentPoints = missionPoints[currentPosition];
			}
			else if (!PhotonNetwork.IsMasterClient && (missionTagsClient.Count > 0))
			{
				currentTag = missionTagsClient[currentPosition];
				currentPoints = missionPointsClient[currentPosition];
			}

			
			if (PhotonNetwork.IsMasterClient)
			{
				currentTag = missionTags[currentPosition];
				currentPoints = missionPoints[currentPosition];
			}
			else
			{
				currentTag = missionTagsClient[currentPosition];
				currentPoints = missionPointsClient[currentPosition];
			}
		}*/
	}

	void Update()
	{
		if (first)
		{
			first = false;
			currentTag = missionTags[currentPosition];
			currentPoints = missionPoints[currentPosition];
		}
		if (taskDone)
		{
			totalPoints += currentPoints;
			currentPosition += 1;
			currentTag = missionTags[currentPosition];
			currentPoints = missionPoints[currentPosition];
			if ((currentTag == "file") && (missionTags.Count > 1))
			{
				switch (missionTags.Count)
				{
					case 3:
						bonus = 1.5f;
						break;
					case 4:
						bonus = 1.8f;
						break;
					default:
						bonus = 1f;
						break;
				}
				totalPoints = (int)Mathf.Round(totalPoints * bonus);
			}
			taskDone = false;
		}

	}
}
