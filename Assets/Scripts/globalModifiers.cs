using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalModifiers : MonoBehaviour
{
	public float playerSpeedModifier;
	public float missionsPointsModifier;
	public float completeMissionsModifier;
	public float cooldownMissionsModifier;
	
	public int totalPoints;
	
  void Start()
  {
		playerSpeedModifier = 1f;
		missionsPointsModifier = 1f;
		completeMissionsModifier = 1f;
		cooldownMissionsModifier = 1f;
		
		totalPoints = 0;
  }
}
