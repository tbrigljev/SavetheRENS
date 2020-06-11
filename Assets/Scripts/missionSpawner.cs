using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class missionSpawner : MonoBehaviourPun
{
	public GameObject[] missions;
	private GameObject mission;
	private GameObject box;
	private GameObject missionCountText;
	private GameObject globalModifiers;
	//PhotonView thisPhotonView; 

	private int numberOfTasks;
	private string[] missionAllTags = new string[] {
		"map",
		"tab",
		"shelf"
	};
	private string lastTag = "file";
	public List<string> missionTags = new List<string>();
	//public List<string> missionTagsMission = new List<string>();

	private int[] missionAllPoints = new int[] {
		2,
		5,
		8,
		10
	};
	public List<int> missionPoints = new List<int>();
	//public List<int> missionPointsMission = new List<int>();

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
	public int maxTasks;

	private string spawnerMessage;

	private AudioSource newMissionSound;

	private bool hasMissions;
	private string missionName;
	private bool firstThree; 

	//private Text missionCountText;

	void Start()
	{
		numberOfTasks = 0;
		missionCount = 0;
		missionMax = 3;
		allMissions = 0;
		rayLength = 3f;
		time = 0f;
		minTime = 5f;
		maxTime = 7f;
		shakeSpeed = 20f;
		shakeAmount = 0.01f;
		maxTasks = 4;
		firstThree = true; 

		readyParticles = GetComponentInChildren<ParticleSystem>();
		//spawnerMessage = "Waiting on new missions!";

		newMissionSound = gameObject.GetComponent<AudioSource>();

		box = GameObject.Find("FrontStopper");
		Physics.IgnoreCollision(GetComponent<Collider>(), box.GetComponent<Collider>());

		globalModifiers = GameObject.Find("Global");


		missionCountText = GameObject.Find("GlobalCounting");
	}

	void Update()
	{
		/*
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
		*/

		spawnTime = (firstThree) ? 0 : Random.Range(minTime, maxTime);

		time += Time.deltaTime;

		if ((time > spawnTime) && !(globalModifiers.GetComponent<globalModifiers>().gameOver))
		{
			if ((missionCount < missionMax) && (allMissions < 20))    //&& PhotonNetwork.IsMasterClient
			{
				if(!firstThree)
                {
					readyParticles.Play();
					newMissionSound.Play();
				}

				missionType = Random.Range(0, missions.Length);
				Debug.Log("mission length" + missions.Length); 
				missionCount += 1;
				allMissions += 1;
				Vector3 spawnPosition = transform.position;
				spawnPosition.x += 0.2f;
				spawnPosition.y += 0.7f - (0.1f * missionCount);
				if (missionCount < 10)
				{
					spawnPosition.z -= 0.2f;
				}
				else if (missionCount < 20)
				{
					spawnPosition.z -= 0.0f;
				}
				else if (missionCount < 30)
				{
					spawnPosition.z += 0.2f;
				}
				numberOfTasks = Random.Range(1, maxTasks);
				Debug.Log("numberTasks" + numberOfTasks);
				missionTags.Clear();
				missionPoints.Clear();

				for (int i = 0; i < numberOfTasks; i++)
				{
					missionTags.Add(missionAllTags[Random.Range(0, missionAllTags.Length)]);
					missionPoints.Add(missionAllPoints[Random.Range(0, missionAllPoints.Length)]);
				}
				missionTags.Add(lastTag);
				missionPoints.Add(1);

				/*if (!PhotonNetwork.IsMasterClient && missionTags.Count > 0)
				{
					for (int i = 0; i < numberOfTasks + 1; i++)
					{
						missionTagsClient.Add(missionTags[i]);
						missionPointsClient.Add(missionPoints[i]);
					}
				}*/
				//missionTagsClient = missionTags;
				//missionPointsClient = missionPoints;

				if (true)
				{
					missionName = missions[missionType].name;
					Debug.Log("MISSION NAME " + missionName);
					Debug.Log("MISSION COUNT" + missionCount); 
					mission = PhotonNetwork.Instantiate(missionName, spawnPosition, gameObject.transform.rotation, 0);

					//mission = Instantiate(missions[missionType], spawnPosition, gameObject.transform.rotation);
					mission.name = "missionReady" + missionCount.ToString();
					//missionTagsMission = mission.GetComponent<missionsTracking>().missionTags; 
					//missionPointsMission = mission.GetComponent<missionsTracking>().missionPoints;

					for (int i = 0; i < missionTags.Count; i++)
					{
						//missionTags.Add(missionSpawner.GetComponent<missionSpawner>().missionTags[i]);
						//missionPoints.Add(missionSpawner.GetComponent<missionSpawner>().missionPoints[i]);

						mission.GetComponent<missionsTracking>().missionTags.Add(missionTags[i]);
						mission.GetComponent<missionsTracking>().missionPoints.Add(missionPoints[i]);

					}
					if(missionCount < 4 && firstThree)
                    {
					
						if (missionCount == 3)
						{
							missionCount = 0;
							allMissions = 0; 
							firstThree = false; 
						}
						PhotonNetwork.Destroy(mission); 
                    }
				}
				time = 0f;
			}
		}

		if (hasMissions && !(globalModifiers.GetComponent<globalModifiers>().gameOver))
		{
			Vector3 boxPosition = transform.position;
			boxPosition.z += (Mathf.Sin(Time.time * shakeSpeed) * shakeAmount);
			transform.position = boxPosition;
			hasMissions = false;
		}

		Debug.DrawRay(transform.position, transform.forward * rayLength, Color.blue);
	}

	void OnTriggerStay(Collider collider)
	{
		hasMissions = true;
	}
}
