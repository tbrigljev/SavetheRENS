using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npcController : MonoBehaviour
{
	//public Camera cam;
	
	private NavMeshAgent agent;
	
	private GameObject[] NPCs;
	private GameObject NPC;
	
	float[,] positions = new float[20,4] {
		{ 16.0f,  -2.6f,  16.5f,  -3.1f}, //0
		{ 15.5f,  -5.5f,  15.5f,  -5.0f},
		{ 10.0f,  -6.0f,   9.5f,  -6.0f},
		{  8.5f, -10.7f,   9.0f, -11.2f},
		{ 15.8f, -13.0f,  16.3f, -12.5f}, //WestPol01
		{ 10.0f, -14.5f,   9.5f, -14.0f}, //5
		{ 15.7f, -19.0f,  16.2f, -18.5f}, //WestPCi01
		{ 10.0f, -21.1f,   9.5f, -21.6f},
		{ 13.0f, -22.0f,  13.5f, -22.5f}, //WestPol02
		{  1.0f, -21.5f,   0.5f, -22.0f},
		{ -2.0f, -19.0f,  -2.5f, -18.5f}, //10
		{ -7.0f, -13.0f,  -6.5f, -13.5f},
		{-11.0f, -21.0f, -10.5f, -21.5f},
		{-18.0f, -21.0f, -17.5f, -21.5f},
		{-23.0f, -21.5f, -22.5f, -22.0f}, //EastPCi01
		{-22.0f, -17.0f, -22.5f, -17.5f}, //15
		{-22.5f, -13.0f, -23.0f, -13.0f},
		{-23.5f,  -8.0f, -24.0f,  -7.5f}, //EastPol01
		{-16.0f,  -8.7f, -16.0f,  -9.2f},
		{-24.0f,  -4.0f, -23.5f,  -4.5f}		
	};
	
	private int currentPosition;
	private int choosePosition;
	private List<int> otherPositions = new List<int>();
	
	private Vector3 start;
	private Vector3 destination;
	private Vector3 finalAngle;
	
	private Quaternion lookRotation;
	
	private float time;
	private float waitTime;
	private float timerStuck;
	private float timeStuck;
	private float timerRotation;
	private float timeToRotate;
	private float offsetAngle;
	
	private bool move;
		
	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		
		switch (gameObject.name)
		{
			case "NPCWestPol01":
				currentPosition = 4;
				break;
			case "NPCWestPol02":
				currentPosition = 8;
				break;
			case "NPCEastPol01":
				currentPosition = 17;
				break;
			case "NPCWestPCi01":
				currentPosition = 6;
				break;
			case "NPCEastPCi01":
				currentPosition = 14;
				break;
			default:
				currentPosition = 0;
				break;
		}
		
		start.x = positions[currentPosition, 0];
		start.y = 0f;
		start.z = positions[currentPosition, 1];
		
		transform.position = start;
		start = Vector3.zero;
		
		time = 0f;
		waitTime = Random.Range(2f, 10f);
		timerStuck = 0f;
		timeStuck = 6f;
		timerRotation = 0f;
		timeToRotate = 2f;
		
		move = true;
  }
	
	void getOthersPositions()
	{
		otherPositions.Clear();
		NPCs = GameObject.FindGameObjectsWithTag("NPCs");
		foreach (GameObject NPC in NPCs)
		{
			otherPositions.Add(NPC.GetComponent<npcController>().currentPosition);
		}
	}
	
	void Update()
	{
		time += Time.deltaTime;
		
		if (time > waitTime)
		{			
			if (move)
			{
				getOthersPositions();
				choosePosition = Random.Range(0, 20);
				if (!otherPositions.Contains(choosePosition))
				{
					move = false;
					currentPosition = choosePosition;
					destination.x = positions[currentPosition, 0];
					destination.y = transform.position.y;
					destination.z = positions[currentPosition, 1];
					finalAngle.x  = positions[currentPosition, 2];
					finalAngle.y  = transform.position.y;
					finalAngle.z  = positions[currentPosition, 3];
					
					agent.SetDestination(destination);
				}
			}
			else
			{
				timerStuck += Time.deltaTime;
				
				if ((transform.position - destination).sqrMagnitude < 0.1f)
				{
					timerRotation += Time.deltaTime;
					{
						if (timerRotation < timeToRotate)
						{
							lookRotation = Quaternion.LookRotation((finalAngle - transform.position).normalized);
							transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 3f*Time.deltaTime);
						}
						else
						{
							start = Vector3.zero;
							time = 0f;
							timerStuck = 0f;
							timerRotation = 0f;
							waitTime = Random.Range(2f, 10f);
							move = true;
						}
					}					
				}
				if (timerStuck > timeStuck)
				{
					if ((transform.position - start).sqrMagnitude < 0.05f)
					{
						move = true;
					}
					start = transform.position;
					timerStuck = 0;
				}				
			}
		}
		
		/*if (Input.GetMouseButtonDown(0))
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast(ray, out hit))
			{
				agent.SetDestination(hit.point);
			}
		}*/
	}
}
