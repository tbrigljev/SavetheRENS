using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class avatarInputs : MonoBehaviourPun
{
	private GameObject objectGlobalModifiers;

	private bool gameOver;

	public bool MovePlace;
	public bool PickupDrop;
	public bool Work;
	public bool Yeet;

	void Start()
	{
		MovePlace = false;
		PickupDrop = false;
		Work = false;
		Yeet = false;
		gameOver = false;

		objectGlobalModifiers = GameObject.Find("Global");
	}

	void Update()
	{

		//gameOver = objectGlobalModifiers.GetComponent<globalModifiers>().gameOver;
		MovePlace = false;
		PickupDrop = false;
		Yeet = false;


		if (gameOver)
		{
			MovePlace = false;
			PickupDrop = false;
			Work = false;
			Yeet = false;
		}
		else if(photonView.IsMine)
		{
			if ((!MovePlace) && (Input.GetButtonDown("MovePlace")))
			{
				MovePlace = true;
			}
			else if (Input.GetButtonUp("MovePlace"))
			{
				MovePlace = false;
			}

			if (Input.GetButtonDown("PickupDrop"))
			{
				PickupDrop = true;
			}
			else if (Input.GetButtonUp("PickupDrop"))
			{
				PickupDrop = false;
			}

			if (Input.GetButton("Work"))
			{
				Work = true;
			}
			else if (Input.GetButtonUp("Work"))
			{
				Work = false;
			}

			if (Input.GetButtonDown("Yeet"))
			{
				Yeet = true;
			}
			else if (Input.GetButtonUp("Yeet"))
			{
				Yeet = false;
			}
		}
	}
}
