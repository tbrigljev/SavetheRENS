using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TRANSFOMR  : POS / LIMITUP/RIGHT / LIMITDOWN/LEFT
// ORIGINAL X : -7 / 2 / -10
// ORIGINAL Y : 9.5
// ORIGINAL Z : 4 / 2 / 4.5
// ROTATION   : 40 / 180 / 0

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
			//limitRight = 2f;
			limitRight = 6f;
			//limitLeft = -10f;
			limitLeft = -14f;
			//limitUp = 2f;
			//limitUp = 4f;
			//limitDown = 4.5f;
			limitDown = transform.position.z;
    }
 
    void LateUpdate()
    {
			cameraPosition = player.transform.position + offset;
			
			cameraPosition.x = (cameraPosition.x > limitRight) ? limitRight : cameraPosition.x;
			cameraPosition.x = (cameraPosition.x < limitLeft)  ? limitLeft  : cameraPosition.x;
			//cameraPosition.z = (cameraPosition.z < limitUp)    ? limitUp    : cameraPosition.z;
			//cameraPosition.z = (cameraPosition.z > limitDown)  ? limitDown  : cameraPosition.z;
			cameraPosition.z = limitDown;
			
			transform.position = cameraPosition;
    }
}
