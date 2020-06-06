using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectColorPauseMenu : MonoBehaviour
{
	public GameObject menuPauseButtons;
	public int buttonIndex;
	public int selectedButton;
	
	private GameObject background;
	private GameObject text;
	private int currentScreen;
	
	private bool gameOver;
	
  void Start()
  {		
		menuPauseButtons = GameObject.Find("Global");
		selectedButton = menuPauseButtons.GetComponent<globalModifiers>().selectedButton;
		gameOver = menuPauseButtons.GetComponent<globalModifiers>().gameOver;
		currentScreen = menuPauseButtons.GetComponent<globalModifiers>().currentScreen;
			
		background = transform.Find("Background").gameObject;
		background.GetComponent<Image>().color = menuPauseButtons.GetComponent<globalModifiers>().backgroundDefault;
			
		text = transform.Find("Text").gameObject;
		text.GetComponent<Text>().color = menuPauseButtons.GetComponent<globalModifiers>().textDefault;
			
    switch(gameObject.name)
		{
			case "ButtonResume":
				buttonIndex = 1;
				break;
			case "ButtonEndQuit":
				buttonIndex = 2;
				break;
			case "ButtonVisitGame":
				buttonIndex = 3;
				break;
			case "ButtonPauseQuit":
				buttonIndex = 2;
				break;			
			case "ButtonBackEndScreen":
				buttonIndex = -1;
				break;
			default:
				buttonIndex = 1;
				break;
		}
	}

  void Update()
  {
		if (gameOver && gameObject.name == "ButtonPauseQuit")
		{
			Debug.Log("Hello ?");
			buttonIndex = 3;
		}
		if (gameOver && gameObject.name == "ButtonBackEndScreen")
		{
			buttonIndex = 2;
		}		
		
		selectedButton = menuPauseButtons.GetComponent<globalModifiers>().selectedButton;
		if (selectedButton == buttonIndex)
		{
			background.GetComponent<Image>().color = menuPauseButtons.GetComponent<globalModifiers>().backgroundSelected;
			text.GetComponent<Text>().color = menuPauseButtons.GetComponent<globalModifiers>().textSelected;
		}	
		else
		{
			if (currentScreen == 2)
			{
				background.GetComponent<Image>().color = menuPauseButtons.GetComponent<globalModifiers>().textDefault;
				text.GetComponent<Text>().color = menuPauseButtons.GetComponent<globalModifiers>().backgroundDefault;
			}
			else
			{
				background.GetComponent<Image>().color = menuPauseButtons.GetComponent<globalModifiers>().backgroundDefault;
				text.GetComponent<Text>().color = menuPauseButtons.GetComponent<globalModifiers>().textDefault;
			}
		}
	}
}
