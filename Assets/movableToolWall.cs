using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movableToolWall : MonoBehaviour
{
	public GameObject wallWest;
	public GameObject wallEast;
	public GameObject wallBack;
	public GameObject boxFailPrefab;
	
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
	private float currentDistanceToPlayers;
	private float currentDistanceToClosestPlayer;
	
	public float range;
	public float distance;
	public float boxFailOffset;		

	private bool allowed;
	private bool carrying;
	//private bool validPosition;
		
  void Start()
  {
		boxFail = null;
		
    //time = 0f;
		range = 1.5f;
		distance = 1f;		
		//maxTimer = 1f;
		rayLengthForward = 3f;
		rayLengthBackward = 0.5f;
		
		player = null;
		allowed = false;
		carrying = false;
		closestPlayer = null;
		//validPosition = false;
		
		col = GetComponent<Collider>();
  }
	
	GameObject findClosestPlayer()
	{
		players = GameObject.FindGameObjectsWithTag("Player");		
    currentDistanceToPlayers = 200f;
		
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
		bool validAngleBack = Mathf.Abs(angleBack - 180) < 2f;
		bool validAngleWest = Mathf.Abs(angleWest - 90) < 2f;
		bool validAngleEast = Mathf.Abs(angleEast - 90) < 2f;
		
		RaycastHit hit;
		Physics.Raycast(transform.position, -transform.forward, out hit, rayLengthBackward*100);

		if ((validAngleBack && (hit.collider.gameObject.name == "WallBack")     && hit.distance < rayLengthBackward) ||
				(validAngleWest && (hit.collider.gameObject.name == "WallWest")     && hit.distance < rayLengthBackward) ||
				(validAngleEast && (hit.collider.gameObject.name == "WallMiddle02") && hit.distance < rayLengthBackward))
		{
			allowed = true;
		}
		else
		{
			allowed = false;
		}
		
		/*time += Time.deltaTime;
		
		if (time > maxTimer)
		{
			Debug.Log(time);
			allowed = true;
			time = 0f;
		}*/
	}
  
	void Update()
  {
		Debug.DrawRay(transform.position,  transform.forward*rayLengthForward,  Color.blue);
		Debug.DrawRay(transform.position, -transform.forward*rayLengthBackward, Color.red);
		
		player = findClosestPlayer();
		
		/*time += Time.deltaTime;
		if (time > maxTimer)
		{			
			Debug.Log("Closest player is: " + player.name);
			angle = Vector3.Angle(wallWest.transform.forward, transform.forward);
			Debug.Log("Angle with west wall is: " + angle);
			angle = Vector3.Angle(wallEast.transform.forward, transform.forward);
			Debug.Log("Angle with east wall is: " + angle);
			angle = Vector3.Angle(wallBack.transform.forward, transform.forward);
			Debug.Log("Angle with back wall is: " + angle);
		}*/
		
		if (!carrying)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				Debug.Log("Q has been pressed");
				//Debug.Log("Player is at: " + player.transform.position);
				//Debug.Log("Tab is at: " + (transform.position - offset));
				if ((player.transform.position - (transform.position - offset)).sqrMagnitude < range*range)
				{
					Physics.IgnoreCollision(col, player.GetComponent<Collider>());
					col.isTrigger = true;
					carrying = true;
				}
				//Debug.Log("Position is: " + tabPos);
				//Debug.Log("Player is at: " + player.transform.position);
				//Debug.Log("Distance is: " + curD);
				//Debug.Log("Range is: " + range*range);
				//Debug.Log("Carrying must be true: " + carrying);
			}
		}
		else if (carrying)
		{
			player.GetComponent<avatarControls>().carrying = carrying; 
			
			transform.position = player.transform.position + player.transform.TransformDirection(new Vector3(0, 3, distance));;
			trot = player.transform.eulerAngles;
			trot.y += 180;
			transform.rotation = Quaternion.Euler(trot);
			
			if (allowed)
			{
				Destroy(boxFail);
				
				if (Input.GetKeyDown(KeyCode.Q))
				{
					drop();
					carrying = false;
					col.isTrigger = false;				
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
		var currentPos = transform.position;
		transform.position = new Vector3(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y), Mathf.Round(currentPos.z));
		
		var rot = transform.eulerAngles;
    rot.x = Mathf.Round(rot.x / 90) * 90;
    rot.y = Mathf.Round(rot.y / 90) * 90;
    rot.z = Mathf.Round(rot.z / 90) * 90;
		trot = rot;
    transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
	}
	
	void OnTriggerStay(Collider collider)
	{
		allowed = false;
		//time = 0f;
	}
}
