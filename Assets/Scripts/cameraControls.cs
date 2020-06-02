using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    public GameObject player;
		public int testVar;
    private Vector3 offset;
		private Vector3 cameraPosition;
		private float limitRight;
		private float limitLeft;
		private float limitUp;
		private float limitDown;

    void Start()
    {
      offset = transform.position - player.transform.position;
			limitRight = 2f;
			limitLeft = -10f;
			limitUp = 2f;
			limitDown = 4.5f;
    }
 
    void LateUpdate()
    {
			cameraPosition = player.transform.position + offset;
			
			cameraPosition.x = (cameraPosition.x > limitRight) ? limitRight : cameraPosition.x;
			cameraPosition.x = (cameraPosition.x < limitLeft)  ? limitLeft  : cameraPosition.x;
			cameraPosition.z = (cameraPosition.z < limitUp)    ? limitUp    : cameraPosition.z;
			cameraPosition.z = (cameraPosition.z > limitDown)  ? limitDown  : cameraPosition.z;
			
			transform.position = cameraPosition;
    }
}
