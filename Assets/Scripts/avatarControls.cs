using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avatarControls : MonoBehaviour
{
	private GameObject globalModifiers;
	private GameObject camera;
	
	private Rigidbody rb;
	
	public float speed;
	private float speedModifier;
	private float stepsTime;
	private float turnrate;
	
	private float minX;
	private float maxX;
	private float minZ;
	private float maxZ;
	private float time;
	private float minMiddle;
	private float maxMiddle;	
	private float rayLength;
	
	private AudioSource footSteps;
	public AudioClip[] footStepsSounds;
	
	public bool carrying;
	public bool inMission;
	
    void Start()
    {			
			time = 0f;
      speed = 10f;
			rayLength = 3f;			
			stepsTime = 0.4f;
			turnrate = 0.15f;
			
			carrying = false;
			inMission = false;
			
			rb = GetComponent<Rigidbody>();
			
			footSteps = gameObject.GetComponent<AudioSource>();
			
			globalModifiers = GameObject.Find("Global");
			speedModifier = globalModifiers.GetComponent<globalModifiers>().playerSpeedModifier;
			
			camera = GameObject.Find("MainCamera");
			camera.GetComponent<cameraControls>().player = gameObject;
    }

    void FixedUpdate()
    {
      float moveX = -Input.GetAxis("Horizontal");
			float moveZ = -Input.GetAxis("Vertical");
			
			Vector3 movement    = new Vector3(moveX, 0.0f, moveZ);
			
			Vector3 newPosition = transform.position + movement * speed * Time.deltaTime;
			Vector3 looking     = newPosition - transform.position;
			
			if ((movement != Vector3.zero) && !((Input.GetKey("space"))))
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
			
			time += Time.deltaTime;
			if (time > stepsTime && (Mathf.Abs(moveX) > 0 || Mathf.Abs(moveZ) > 0))
			{
				time = 0f;
				int index = Random.Range(0, footStepsSounds.Length);
				footSteps.clip = footStepsSounds[index];
        footSteps.Play ();
			}
			
			if (movement != Vector3.zero)
			{
				rb.MovePosition(newPosition);
			}
			else
			{
				rb.velocity = Vector3.zero;
			}
		}
		
	void Update()
	{
		if (speedModifier != globalModifiers.GetComponent<globalModifiers>().playerSpeedModifier)
		{
			speedModifier = globalModifiers.GetComponent<globalModifiers>().playerSpeedModifier;
			speed *= speedModifier;
			stepsTime /= speedModifier;
		}
		
		Vector3 eyesLevel = new Vector3(0, 3.4f, 0);
		Vector3 hipLevel  = new Vector3(0, 2, 0);
    Debug.DrawRay(transform.position+eyesLevel, transform.forward*rayLength, Color.red);
		Debug.DrawRay(transform.position+hipLevel,  transform.forward*rayLength, Color.green);
		Debug.DrawRay(transform.position,           transform.forward*rayLength, Color.blue);
	}
}
