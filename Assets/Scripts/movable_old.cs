using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movable_old : MonoBehaviour
{
  public GameObject item;
	public GameObject player;
	private Collider col = null;
	private BoxCollider bcol;
	//public Transform guide;
	
	private Vector3 offset = new Vector3(0, 2, 0);
	private Vector3 trot = new Vector3(0, 0, 0);
	
	private Vector3 castCenter = new Vector3(0, 0, 0);
	private Vector3 castExtents = new Vector3(0, 0, 0);
	private Vector3 castForward = new Vector3(0, 0, 0);
	
	private bool carrying = false;
	private bool col_hit = false;
	
	private RaycastHit hit;
	
	public int range = 1;
	public float distance = -1f;
	public int angle = 180;
	
	public int colx = 1;
	public int coly = 1;
	public int colz = 1;
	
	float m_MaxDistance;

	void Start()
	{
		player = GameObject.Find("AvatarMain");
		col = item.GetComponent<Collider>();
		bcol = item.GetComponent<BoxCollider>();
		//Physics.IgnoreCollision(item.GetComponent<Collider>(), player.GetComponent<Collider>());
	}
	
	void Update()
	{
		castCenter = col.bounds.center;
		castExtents = bcol.size;
		
		if (!carrying)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if (((player.transform.position - item.transform.position).sqrMagnitude < range*range) || ((player.transform.position + offset - item.transform.position).sqrMagnitude < range*range))
				{
					pickup();
					carrying = true;
				}
			}
		}
		else if (carrying)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				col.enabled = true;
				//drop();
				//carrying = false;
				Debug.Log("Size :" + bcol.size);
				
				col_hit = Physics.BoxCast(col.bounds.center, bcol.size, new Vector3(0,0,0), out hit, Quaternion.Euler(trot.x, trot.y, trot.z));			
				//col_hit = Physics.BoxCast(col.bounds.center, bcol.size, new Vector3(0,0,0), out hit);
				Debug.Log("Hit :" + hit);
				
				if (!col_hit || (hit.collider.name == "AvatarMain"))
				{
					Debug.Log("Nothing hit");
					drop();
					carrying = false;
				}
				else
				{
					Debug.Log("Collision detected with " + hit.collider.name);
					Debug.Log("Center " + col.bounds.center);
					Debug.Log("Scale " + bcol.size);
				}
			}
		}
	}
	
	/*
	void pickup()
	{
		col.enabled = false;

		item.transform.SetParent(player.transform);
		Vector3 position = new Vector3(distance, 0, 0);
		item.transform.localPosition = position;
		item.transform.localRotation = Quaternion.Euler(0, angle, 0);
	}*/
	void pickup()
	{
		col.enabled = false;
		item.transform.position = player.transform.position;
	}

/*
	void drop()
	{		
		item.transform.SetParent(null);
		
		var currentPos = item.transform.position;
		item.transform.position = new Vector3(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y), Mathf.Round(currentPos.z));
		
		var rot = transform.eulerAngles;
    rot.x = Mathf.Round(rot.x / 90) * 90;
    rot.y = Mathf.Round(rot.y / 90) * 90;
    rot.z = Mathf.Round(rot.z / 90) * 90;
		trot = rot;
    item.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
	}*/
	void drop()
	{
		var currentPos = item.transform.position;
		item.transform.position = new Vector3(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y), Mathf.Round(currentPos.z));
		
		var rot = transform.eulerAngles;
    rot.x = Mathf.Round(rot.x / 90) * 90;
    rot.y = Mathf.Round(rot.y / 90) * 90;
    rot.z = Mathf.Round(rot.z / 90) * 90;
		trot = rot;
    item.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
	}
	
	void OnDrawGizmos()
	{
		//Check if there has been a hit yet
		if (col_hit)
     {
			 Gizmos.color = Color.red;
			 //Draw a Ray forward from GameObject toward the hit
			 //Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
			 //Draw a cube that extends to where the hit exists
			 Gizmos.DrawWireCube(castCenter + castForward * hit.distance, castExtents);
    }
		//If there hasn't been a hit yet, draw the ray at the maximum distance
		else
    {
			Gizmos.color = Color.blue;
			//Draw a Ray forward from GameObject toward the maximum distance
			//Gizmos.DrawRay(col.bounds.center, transform.forward * m_MaxDistance);
			//Draw a cube at the maximum distance
			Gizmos.DrawWireCube(castCenter + castForward * m_MaxDistance, castExtents);
     }
  }
}