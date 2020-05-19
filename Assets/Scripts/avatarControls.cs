using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avatarControls : MonoBehaviour
{
	private Rigidbody rb;
	
	public float speed;
	public float turnrate;
	private float rayLength;
	private float minX;
	private float maxX;
	private float minZ;
	private float maxZ;
	private float minMiddle;
	private float maxMiddle;
	
	public bool carrying;
	
    void Start()
    {
      speed = 10f;
			rayLength = 3f;
			turnrate = 0.15f;
			
			carrying = false;
			
			rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
      float moveX = -Input.GetAxis("Horizontal");
			float moveZ = -Input.GetAxis("Vertical");
			
			Vector3 movement    = new Vector3(moveX, 0.0f, moveZ);
			
			Vector3 newPosition = transform.position + movement * speed * Time.deltaTime;
			Vector3 looking     = newPosition - transform.position;
			
			if(movement != Vector3.zero)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(looking), turnrate);
			}
			
			if (!carrying)
			{
				minX = -15f;
				maxX = 6f;
				minZ = -9;
				maxZ = -0.8f;				
				minMiddle = -10f;
				maxMiddle = -8f;
			}
			else
			{			
				minX = -14.8f;
				maxX = 5.8f;
				minZ = -8.8f;				
				minMiddle = -10.2f;
				maxMiddle = -7.8f;
			}
			
			if ((newPosition.z < -4f) && ((newPosition.x < maxMiddle) && (newPosition.x > minMiddle)))
			{
				newPosition.x = (newPosition.x > -8.85f) ? maxMiddle : minMiddle;
			}
			
			newPosition.x = (newPosition.x > maxX) ? maxX : newPosition.x;
			newPosition.x = (newPosition.x < minX) ? minX : newPosition.x;
			newPosition.z = (newPosition.z > maxZ) ? maxZ : newPosition.z;
			newPosition.z = (newPosition.z < minZ) ? minZ : newPosition.z;
			newPosition.y = 0;
			
			rb.MovePosition(newPosition);
    }
		
		void Update()
		{			
			Vector3 eyesLevel = new Vector3(0, 3.4f, 0);
			Vector3 hipLevel  = new Vector3(0, 2, 0);
      Debug.DrawRay(transform.position+eyesLevel, transform.forward*rayLength, Color.red);
			Debug.DrawRay(transform.position+hipLevel,  transform.forward*rayLength, Color.green);
			Debug.DrawRay(transform.position,           transform.forward*rayLength, Color.blue);
		}
}
