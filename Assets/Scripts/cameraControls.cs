using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    public GameObject player;		
    private Vector3 offset;
		private Vector3 cameraPosition;
		private float limitRight;
		private float limitLeft;
		private float limitZ;

    void Start()
    {
      offset = transform.position - player.transform.position;
			limitRight = 1.5f;
			limitLeft = -9.6f;
			limitZ = transform.position.z;
    }

 
    void LateUpdate()
    {
			cameraPosition = player.transform.position + offset;
			
			cameraPosition.x = (cameraPosition.x > limitRight) ? limitRight : cameraPosition.x;
			cameraPosition.x = (cameraPosition.x < limitLeft)  ? limitLeft  : cameraPosition.x;
			cameraPosition.z = limitZ;
			
			transform.position = cameraPosition;
    }
}
