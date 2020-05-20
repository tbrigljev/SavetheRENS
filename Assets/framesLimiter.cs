using UnityEngine;
using System.Collections;

public class framesLimiter : MonoBehaviour
{	
	void Awake()
	{
		Application.targetFrameRate = 30;
  }
}
