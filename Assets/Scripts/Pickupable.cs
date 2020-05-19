using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupable : MonoBehaviour
{
  public GameObject item;
	public GameObject player;
	
	private Rigidbody rb;
	
	private Vector3 offset = new Vector3(0, 2, 0);
	
	private bool carrying;
	
	public int range = 2;
		
  void Start()
  {
    item.GetComponent<Rigidbody>().useGravity = true;
  }

	void Update()
	{
		if (carrying == false)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				player = GameObject.Find("AvatarMain");
				if (((player.transform.position - item.transform.position).sqrMagnitude < range*range) || ((player.transform.position + offset - item.transform.position).sqrMagnitude < range*range))
				{
					pickup();
					carrying = true;
				}
			}
		}
		else if (carrying == true)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				drop();
				carrying = false;
			}
		}
	}
	
	void pickup()
	{
		item.GetComponent<Rigidbody>().useGravity = false;
		item.GetComponent<Rigidbody>().isKinematic = true;
		item.GetComponent<Collider>().enabled = false;

		item.transform.SetParent(player.transform);
		Vector3 position = new Vector3(-1, 2, 0);
		item.transform.localPosition = position;
		item.transform.localRotation = Quaternion.identity;
	}

	void drop()
	{
		item.GetComponent<Rigidbody>().useGravity = true;
		item.GetComponent<Rigidbody>().isKinematic = false;
		item.GetComponent<Collider>().enabled = true;
		
		item.transform.SetParent(null);
	}
}
