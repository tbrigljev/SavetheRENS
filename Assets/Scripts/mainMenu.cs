using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
	private GameObject textStartUp;
	private GameObject menuButtons;
	private GameObject playButton;
	
	private float time;
	private float waitTime;
	
	public int selectedButton;
	public int maxButtons;
	
	public Color backgroundDefault;
	public Color textDefault;
	public Color backgroundSelected;
	public Color textSelected;
	
	private bool waitMenu;
	private bool waitButtons;
	
	void Start()
	{
		time = 0f;
		waitTime = 1f;
		selectedButton = 0;
		maxButtons = 3;
		
		waitMenu = false;
		waitButtons = false;
		
		backgroundDefault = Color.black;
		textDefault = Color.white;
		backgroundSelected = new Color32(255,160,65,255);
		textSelected = new Color32(6,61,232,255);
		
		playButton = GameObject.Find("ButtonPlay");
		
		textStartUp = GameObject.Find("TextStartUp");
		textStartUp.SetActive(true);
		menuButtons = GameObject.Find("Buttons");
		menuButtons.SetActive(false);			
	}
	
	void Update()
	{	
		if (time < waitTime)
		{
			time += Time.deltaTime;
		}
		else if (time > waitTime)
		{
			if (selectedButton < 1)
			{				
				if (Input.anyKey)
				{
					selectedButton = 1;
					textStartUp.SetActive(false);
					menuButtons.SetActive(true);
					waitMenu = true;
					time = 0f;
				}
			}
		}
		
		if (waitMenu)
		{
			if (time < waitTime)
			{
				time += Time.deltaTime;
			}
			else
			{
				waitButtons = true;
			}
		}
		
		if (waitButtons)
		{
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				selectedButton += 1;
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				selectedButton -= 1;
			}
			if (selectedButton > maxButtons)
			{
					selectedButton = 1;
			}
			else if (selectedButton < 1)
			{
				selectedButton = 3;
			}
			
			if (Input.GetKeyDown(KeyCode.Return))
			{
				switch(selectedButton)
				{
					case 1:
						Play();
						break;
					case 2:
						Debug.Log("Credits");
						break;
					case 3:
						Debug.Log("Exit game");
						//UnityEditor.EditorApplication.isPlaying = false;
						Application.Quit();
						break;
					default:
						Debug.Log("Nothing selected");
						break;
				}
			}
		}
	}
	
	void Play()
	{
		SceneManager.LoadScene("SimpleLevel1");
	}
}
