using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movableToolFurniture : MonoBehaviour
{
	private GameObject[] players;
	private GameObject player;
	private GameObject closestPlayer;
	private GameObject boxFail;
	
	private Vector3 trot = new Vector3(0, 0, 0);
	
	private float distancePlayers;
	private float currentDistanceToPlayers;
	private float currentDistanceToClosestPlayer;	
	private float playersDetection;
	
	public float range;
	public float distance;
	public float boxFailOffset;
	
	private bool allowed;
	private bool carried;
	private bool playerCarrying;
	private bool playerInMission;
	
  void Start()
  {
		/*range = 3f;
		if (gameObject.name == "FileCase")
		{
			distance = 2.5f;
		}
		else
		{
			distance = 2f;
		}*/
		
		player = null;
		carried = false;
		closestPlayer = null;		
		playersDetection = 1000f;
		playerCarrying = false;
		playerInMission = false;
		
		GetComponent<Rigidbody>().isKinematic = true;
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

  void Update()
  {
    player = findClosestPlayer();
		playerCarrying = player.GetComponent<avatarControls>().carrying;
		playerInMission = player.GetComponent<avatarControls>().inMission;
		
		if (!carried && !playerCarrying && !playerInMission)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if ((player.transform.position - transform.position).sqrMagnitude < range*range)
				{
					carried = true;
					player.GetComponent<avatarControls>().carrying = carried;
					GetComponent<Rigidbody>().isKinematic = false;
					transform.position = player.transform.position + player.transform.TransformDirection(new Vector3(0, 0, distance));
				}
			}
		}
		else if (carried)
		{
			//GetComponent<Rigidbody>().AddForce(player.transform.forward * 1/(Mathf.Abs((player.transform.position - transform.position).sqrMagnitude)));
			transform.position = player.transform.position + player.transform.TransformDirection(new Vector3(0, 0, distance));
			trot = player.transform.eulerAngles;
			trot.y += 180;
			transform.rotation = Quaternion.Euler(trot);
			
			if (Input.GetKeyDown(KeyCode.Q))
			{
				drop();
				carried = false;
				player.GetComponent<avatarControls>().carrying = carried;
				GetComponent<Rigidbody>().isKinematic = true;
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
}
