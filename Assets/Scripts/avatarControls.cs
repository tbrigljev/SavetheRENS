using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avatarControls : MonoBehaviour
{
	private Rigidbody rb;
	
	public float speed;
	public float turnrate;
	private float rayLength;
	
    void Start()
    {
      rb = GetComponent<Rigidbody>();
			speed = 10f;
			turnrate = 0.15f;
			rayLength = 3f;
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
			
			newPosition.x = (newPosition.x > 9)     ? 9     : newPosition.x;
			newPosition.x = (newPosition.x < -14)   ? -14   : newPosition.x;
			newPosition.z = (newPosition.z > -0.7f) ? -0.7f : newPosition.z;
			newPosition.z = (newPosition.z < -9)    ? -9    : newPosition.z;
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
