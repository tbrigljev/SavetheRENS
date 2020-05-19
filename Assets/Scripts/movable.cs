using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movable : MonoBehaviour
{	
	public GameObject player;
	private Rigidbody rb;
	private Collider col = null;
	
	private Vector3 offset = new Vector3(0, 2, 0);
	private Vector3 trot = new Vector3(0, 0, 0);
	
	private bool carrying = false;
	private bool allowed = true;
	private bool letdown = false;
	
	private float collisionTimer = 0f;
	public float maxTimer = 1f;
	
	private string colName = "";
	
	public int range = 1;
	public float distance = 1f;
	public int angle = 180;
	
	void Start()
	{
		player = GameObject.Find("AvatarMain");
		rb = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
	}
	
	void OnTriggerStay(Collider collider)
	{
		colName = collider.name;
		Debug.Log("TriggerEnterCollider: " + collider.name);
		allowed = false;
		//Debug.Log("++++++++++++OnTriggerEnter: " + allowed);
		collisionTimer = 0f;
	}
	
	void FixedUpdate()
	{
		collisionTimer += Time.deltaTime;
		if (collisionTimer > maxTimer)
		{
			Debug.Log(collisionTimer);
			allowed = true;
			collisionTimer = 0f;
		}
	}
	
	void Update()
	{
		if (!carrying)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if (((player.transform.position - transform.position).sqrMagnitude < range*range) || ((player.transform.position + offset - transform.position).sqrMagnitude < range*range))
				{
					Physics.IgnoreCollision(col, player.GetComponent<Collider>());					
					col.isTrigger = true;
					carrying = true;
				}
			}
		}
		else if (carrying)
		{
			transform.position = player.transform.position + player.transform.TransformDirection(new Vector3(distance, 0, 0));;
			trot = player.transform.eulerAngles;
			trot.y += 180;
			transform.rotation = Quaternion.Euler(trot);
			
			if (Input.GetKeyDown(KeyCode.Q))
			{	
				Debug.Log("Allowed: " + allowed);
				if (allowed)
				{
					Debug.Log("No blocking collision detected");
					carrying = false;
					drop();
					col.isTrigger = false;
					Physics.IgnoreCollision(col, player.GetComponent<Collider>(), false);
				}
				else
				{
					Debug.Log("Collision with: " + colName);
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
	
	void onTriggerExit(Collider collider)
	{
		Debug.Log("TriggerExitAllowed: " + allowed);
		allowed = true;
	}
}