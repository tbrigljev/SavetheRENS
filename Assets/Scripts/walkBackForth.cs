using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkBackForth : MonoBehaviour
{
	private Vector3 startPosition;
	private Vector3 endPosition;
	private Vector3 rotationDirection = new Vector3(1, 0, 0);
	
	private Quaternion lookRotation;
	
	private float speed;
	private float turnrate;
	private float time;
	private float waitTime;
	
	private bool backForth;
	
	void Start()
  {
		backForth = true;
		time = 0f;
		waitTime = 2f;
		speed = 4;
		turnrate = 4f;
		
		switch (gameObject.name)
		{
			case "FillWestPol01":
				startPosition = transform.position;
				endPosition = new Vector3(8f, 0f, -9f);
				waitTime = 2f;
				break;
			case "FillWestPol02":
				startPosition = transform.position;
				endPosition = new Vector3(14f, 0f, -1f);
				waitTime = 3f;
				break;
			default:
				startPosition = transform.position;
				endPosition = transform.position;
				break;
		}
  }
	
	void Update()
	{
		time += Time.deltaTime;
		
		if (time > waitTime)
		{
			if (backForth)
			{
				lookRotation = Quaternion.LookRotation((endPosition - transform.position).normalized);
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnrate*Time.deltaTime);
				transform.position = Vector3.MoveTowards(transform.position, endPosition, speed*Time.deltaTime);
				
				if ((transform.position - endPosition).sqrMagnitude < 0.05f)
				{
					backForth = false;
					time = 0f;
				}
			}
			else
			{
				lookRotation = Quaternion.LookRotation((startPosition - transform.position).normalized);
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnrate*Time.deltaTime);
				transform.position = Vector3.MoveTowards(transform.position, startPosition, speed*Time.deltaTime);
				
				if ((transform.position - startPosition).sqrMagnitude < 0.05f)
				{
					backForth = true;
					time = 0f;
				}
			}
		}
	}
}
