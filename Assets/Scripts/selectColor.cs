using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectColor : MonoBehaviour
{
	public GameObject menuButtons;
	public int buttonIndex;
	public int selectedButton;
	
	private GameObject background;
	private GameObject text;
	
	private int count;
	
    void Start()
    {
			count = 0;
			
			menuButtons = GameObject.Find("MainMenuButtons");
			selectedButton = menuButtons.GetComponent<mainMenu>().selectedButton;			
			
			background = transform.Find("Background").gameObject;
			background.GetComponent<Image>().color = menuButtons.GetComponent<mainMenu>().backgroundDefault;
			
			text = transform.Find("Text").gameObject;
			text.GetComponent<Text>().color = menuButtons.GetComponent<mainMenu>().textDefault;
			
      switch(gameObject.name)
			{
				case "ButtonPlay":
					buttonIndex = 1;
					break;
				case "ButtonCredits":
					buttonIndex = 2;
					break;
				case "ButtonExit":
					buttonIndex = 3;
					break;
				default:
					buttonIndex = 1;
					break;
			}
		}

    void Update()
    {
			if (selectedButton != menuButtons.GetComponent<mainMenu>().selectedButton)
			{
				count += 1;
				selectedButton = menuButtons.GetComponent<mainMenu>().selectedButton;
				if (selectedButton == buttonIndex)
				{
					background.GetComponent<Image>().color = menuButtons.GetComponent<mainMenu>().backgroundSelected;
					text.GetComponent<Text>().color = menuButtons.GetComponent<mainMenu>().textSelected;
				}
				else
				{
					background.GetComponent<Image>().color = menuButtons.GetComponent<mainMenu>().backgroundDefault;
					text.GetComponent<Text>().color = menuButtons.GetComponent<mainMenu>().textDefault;
				}
			}
    }
}
