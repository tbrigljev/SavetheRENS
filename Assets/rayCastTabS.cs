using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayCastTabS : MonoBehaviour
{
	private float rayLength;
	
    void Start()
    {
      rayLength = 3f;
    }
    void Update()
    {
			Vector3 offset  = new Vector3(0, 2, 0);
      Debug.DrawRay(transform.position, transform.forward*rayLength, Color.blue);
    }
}