using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movableToolWall : MonoBehaviour
{
	public GameObject boxFailPrefab;
	
	private GameObject wallWest;
	private GameObject wallEast;
	private GameObject wallBack;
	
	private GameObject[] players;
	private GameObject player;
	private GameObject closestPlayer;
	private GameObject boxFail;
	
	private Collider col;
	
	private Vector3 trot = new Vector3(0, 0, 0);
	private Vector3 offset = new Vector3(0, 3, 0);	
	
  //private float time;
	//private float maxTimer;
	private float distancePlayers;
	private float rayLengthForward;
	private float rayLengthBackward;
	private float playersDetection;
	private float wallPlaceOffsetX;
	private float wallPlaceOffsetZ;
	private float currentDistanceToPlayers;
	private float currentDistanceToClosestPlayer;
	
	public float range;
	public float distance;
	public float boxFailOffset;
	
	private bool validAngleBack;
	private bool validAngleWest;
	private bool validAngleEast;

	private bool allowed;
	private bool carried;
	private bool playerCarrying;
	private bool playerInMission;
	//private bool validPosition;
	private bool hittt;
		
  void Start()
  {
		boxFail = null;
		
		wallWest = GameObject.Find("MainRoomWallWest");
		wallEast = GameObject.Find("MainRoomWallMiddle02");
		wallBack = GameObject.Find("MainRoomWallBack");
		
    //time = 0f;
		//range = 1.5f;
		//distance = 1f;
		//maxTimer = 1f;
		rayLengthForward = 1f;
		rayLengthBackward = 0.5f;
		playersDetection = 1000f;
		
		player = null;
		allowed = false;
		carried = false;
		closestPlayer = null;
		playerCarrying = false;
		playerInMission = false;
		//validPosition = false;
		
		col = GetComponent<Collider>();
  }
	
	GameObject findClosestPlayer()
	{
		players = GameObject.FindGameObjectsWithTag("Player");		
    currentDistanceToPlayers = playersDetection;
		
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
		float angleBack = Vector3.Angle(wallBack.transform.forward, transform.forward);
		float angleWest = Vector3.Angle(wallWest.transform.forward, transform.forward);
		float angleEast = Vector3.Angle(wallEast.transform.forward, transform.forward);			
		validAngleBack = Mathf.Abs(angleBack - 180) < 2f;
		validAngleWest = Mathf.Abs(angleWest - 90) < 2f;
		validAngleEast = Mathf.Abs(angleEast - 90) < 2f;
		
		RaycastHit hit;
		hittt = Physics.Raycast(transform.position, -transform.forward, out hit, rayLengthBackward*100);
		
		/*if ((gameObject.name == "TabB") && (hittt))
		{
			Debug.Log("Collision detected with: " + hit.collider.gameObject.name);
			Debug.Log("Collision distance is: " + hit.distance);
			Debug.Log("Angle back is: " + validAngleBack);
		}*/

		if ((validAngleBack && (hit.collider.gameObject.name == "MainRoomWallBack")     && hit.distance < rayLengthBackward) ||
				(validAngleWest && (hit.collider.gameObject.name == "MainRoomWallWest")     && hit.distance < rayLengthBackward) ||
				(validAngleEast && (hit.collider.gameObject.name == "MainRoomWallMiddle02") && hit.distance < rayLengthBackward))
		{
			allowed = true;
		}
		else
		{
			allowed = false;
		}
	}
  
	void Update()
  {
		wallPlaceOffsetX = 0f;
		wallPlaceOffsetZ = 0f;
		
		Debug.DrawRay(transform.position,  transform.forward*rayLengthForward,  Color.blue);
		Debug.DrawRay(transform.position, -transform.forward*rayLengthBackward, Color.red);
		
		player = findClosestPlayer();
		playerCarrying = player.GetComponent<avatarControls>().carrying;
		playerInMission = player.GetComponent<avatarControls>().inMission;
		
		if (!carried && !playerCarrying && !playerInMission)
		{
			if (player.GetComponent<avatarInputs>().actionQ)
			{
				if ((player.transform.position - (transform.position - offset)).sqrMagnitude < range*range)
				{
					Physics.IgnoreCollision(col, player.GetComponent<Collider>());
					carried = true;
					player.GetComponent<avatarControls>().carrying = carried;
				}
			}
		}
		else if (carried)
		{
			transform.position = player.transform.position + player.transform.TransformDirection(new Vector3(0, 3, distance));;
			trot = player.transform.eulerAngles;
			trot.y += 180;
			transform.rotation = Quaternion.Euler(trot);
			
			if (allowed)
			{
				Destroy(boxFail);
				
				if (player.GetComponent<avatarInputs>().actionQ)
				{
					drop();
					carried = false;
					player.GetComponent<avatarControls>().carrying = carried;
					//col.isTrigger = false;				
					Physics.IgnoreCollision(col, player.GetComponent<Collider>(), false);
				}
			}
			else if (!allowed)
			{
				if (boxFail == null)
				{
					boxFail = Instantiate(boxFailPrefab, transform.position, Quaternion.identity);
				}
				else if (boxFail != null)
				{
					var boxFailPosition = transform.position;
					boxFailPosition.y += boxFailOffset;
					boxFail.transform.position = boxFailPosition;
					boxFail.transform.rotation = transform.rotation;
				}
			}
		}
  }
		
	void drop()
	{
		if (gameObject.name == "TabS")
		{
			if (validAngleBack)
			{
				wallPlaceOffsetX = ((transform.position.x % 1) < 0.5f) ? -0.5f : 0.5f;
			}
			else if (validAngleEast || validAngleWest)
			{
				wallPlaceOffsetZ = ((transform.position.x % 1) < 0.5f) ? -0.5f : 0.5f;
			}
		}
		
		var currentPos = transform.position;
		transform.position = new Vector3(Mathf.Round(currentPos.x) + wallPlaceOffsetX, Mathf.Round(currentPos.y), Mathf.Round(currentPos.z) + wallPlaceOffsetZ);
		
		var rot = transform.eulerAngles;
    rot.x = Mathf.Round(rot.x / 90) * 90;
    rot.y = Mathf.Round(rot.y / 90) * 90;
    rot.z = Mathf.Round(rot.z / 90) * 90;
		trot = rot;
    transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
	}
	
	void OnCollisionStay(Collision collision)
	{
		allowed = false;
	}
	
	/*void OnTriggerStay(Collider collider)
	{
		allowed = false;
		//time = 0f;
	}*/
}
