using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avatarInputs : MonoBehaviour
{
	private GameObject objectGlobalModifiers;
	
	private bool gameOver;
	
  public bool actionQ;
	public bool actionE;
	public bool actionR;
	public bool actionT;
		
  void Start()
  {
    actionQ = false;
		actionE = false;
		actionR = false;
		actionT = false;
		gameOver = false;
		
		objectGlobalModifiers = GameObject.Find("Global");
  }
		
  void Update()
  {
		gameOver = objectGlobalModifiers.GetComponent<globalModifiers>().gameOver;
		
		if (gameOver)
		{
			actionQ = false;
			actionE = false;
			actionR = false;
			actionT = false;
		}
		
		else
		{
			actionQ = false;
			actionE = false;
			actionT = false;
			
			if ((!actionQ) && (Input.GetKeyDown(KeyCode.Q)))
			{
				actionQ = true;
			}
			
			if (Input.GetKeyDown(KeyCode.E))
			{
				actionE = true;
			}

			if (Input.GetKey(KeyCode.R))
			{
				actionR = true;
			}
			else if (Input.GetKeyUp(KeyCode.R))
			{
				actionR = false;
			}

			if (Input.GetKeyDown(KeyCode.T))
			{
				actionT = true;
			}
		}
  }
}
