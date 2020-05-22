using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class globalModifiers : MonoBehaviour
{
	private GameObject objectTotalPoints;
	private GameObject objectCurrentTime;
	private GameObject objectPauseMenu;
	private GameObject objectEndScreen;
	private GameObject backEndButton;
	private GameObject pauseQuitButton;
	private GameObject resumeButton;
	private GameObject pausedGame;
	
	private GameObject objectEndScreenTime;
	private GameObject objectEndScreenPoints;
	private GameObject objectEndScreenMissions;
	
	public float playerSpeedModifier;
	public float missionsPointsModifier;
	public float completeMissionsModifier;
	public float cooldownMissionsModifier;
	
	public int totalPoints;
	public int newPoints;
	public int filedMissions;
	public int pointsTarget;
	
	private float gameTimer;
	private int gameTime;
	private int gameTimeSeconds;
	private int gameTimeMinutes;
	private int maxButtons;
	private int minButtons;
	private int quitButton;
	private int endButton;
	
	public int selectedButton;
	public int currentScreen;
	
	private string textTotalPoints;
	private string textCurrentTime;
	
	public bool gameOver;
	
	public Color backgroundDefault;
	public Color textDefault;
	public Color backgroundSelected;
	public Color textSelected;
	
	private AudioSource backgroundMusic;
	public AudioClip musicLevel;
	public AudioClip musicEndScreen;
	
	private bool paused;
	
  void Start()
  {
		playerSpeedModifier = 1f;
		missionsPointsModifier = 1f;
		completeMissionsModifier = 1f;
		cooldownMissionsModifier = 1f;
		
		paused = false;
		gameOver = false;
		
		totalPoints = 0;
		pointsTarget = 12;
		newPoints = 0;
		filedMissions = 0;
		textTotalPoints = "Points: " + totalPoints;		
		objectTotalPoints = GameObject.Find("GameInfoPoints");
		objectTotalPoints.GetComponent<TextMesh>().text = textTotalPoints;
		
		objectEndScreenTime = GameObject.Find("EndScreenTime");
		objectEndScreenPoints = GameObject.Find("EndScreenPoints");
		objectEndScreenMissions = GameObject.Find("EndScreenMissions");
		
		backgroundMusic = gameObject.GetComponent<AudioSource>();
		backgroundMusic.clip = musicLevel;
		backgroundMusic.Play();
		
		selectedButton = 0;
		currentScreen = 0;
		maxButtons = 0;
		minButtons = 0;
		
		gameTimer = 0f;
		gameTime = 0;
		textCurrentTime = gameTime.ToString("00") + ":" + gameTime.ToString("00");
		objectCurrentTime = GameObject.Find("GameInfoTime");
		objectCurrentTime.GetComponent<TextMesh>().text = "Time: " + textCurrentTime;
		
		backEndButton = GameObject.Find("ButtonBackEndScreen");
		backEndButton.SetActive(false);
		
		pauseQuitButton = GameObject.Find("ButtonPauseQuit");
		resumeButton = GameObject.Find("ButtonResume");		
		pausedGame = GameObject.Find("PausedGame");
		
		objectPauseMenu = GameObject.Find("PauseMenu");
		objectPauseMenu.SetActive(false);
		
		objectEndScreen = GameObject.Find("EndScreen");
		objectEndScreen.SetActive(false);		
		
		backgroundDefault = Color.black;
		textDefault = Color.white;
		backgroundSelected = new Color32(255,160,65,255);
		textSelected = new Color32(6,61,232,255);
  }

	void Update()
	{
		if (totalPoints > pointsTarget)
		{
			if (!gameOver)
			{
				backgroundMusic.Play();
				backgroundMusic.clip = musicEndScreen;
				backgroundMusic.Play();
				
				gameOver = true;
				currentScreen = 2;
				selectedButton = 3;
				objectEndScreen.SetActive(true);
				backEndButton.SetActive(true);

				objectEndScreenTime.GetComponent<Text>().text = "Level completed in " + textCurrentTime;
				objectEndScreenPoints.GetComponent<Text>().text = "Total points: " + totalPoints;
				objectEndScreenMissions.GetComponent<Text>().text = "Total missions: " + filedMissions;
				
				resumeButton.transform.Find("Text").gameObject.GetComponent<Text>().text = "Revisit room";
				backEndButton.GetComponent<selectColorPauseMenu>().buttonIndex = 2;
				pauseQuitButton.GetComponent<selectColorPauseMenu>().buttonIndex = 3;
				
				gameOver = true;
				selectedButton = 3;
			}
		}
		
		if (currentScreen == 0)
		{
			Time.timeScale = 1;
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				currentScreen = 1;
				selectedButton = 1;
				objectPauseMenu.SetActive(true);
				if (gameOver)
				{
					pausedGame.SetActive(false);
				}
				selectedButton = 1;
				paused = true;
			}
		}		
		else if (currentScreen == 1)
		{
			Time.timeScale = 0;
			maxButtons = (gameOver) ? 3 : 2;
			
			selectedButton += (Input.GetKeyDown(KeyCode.DownArrow)) ? 1          : 0;
			selectedButton -= (Input.GetKeyDown(KeyCode.UpArrow))   ? 1          : 0;
			selectedButton  = (selectedButton > maxButtons)         ? 1          : selectedButton;
			selectedButton  = (selectedButton < 1)                  ? maxButtons : selectedButton;
			
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				objectPauseMenu.SetActive(false);
				currentScreen = 0;
				paused = false;
			}
			
			if (Input.GetKeyDown(KeyCode.Return))
			{
				Time.timeScale = 1;
				
				switch(selectedButton)
				{
					case 1:
						currentScreen = 0;
						objectPauseMenu.SetActive(false);
						paused = false;
						break;
					case 2:
						if (gameOver)
						{
							currentScreen = 2;
							selectedButton = 3;
							objectPauseMenu.SetActive(false);
							objectEndScreen.SetActive(true);
						}
						else
						{
							SceneManager.LoadScene("MainMenu");
						}
						break;
					case 3:
						SceneManager.LoadScene("MainMenu");
						break;
					default:
						Debug.Log("Nothing selected");
						break;
				}
			}
		}
		else if (currentScreen == 2)
		{
			Time.timeScale = 0;
			maxButtons = (gameOver) ? 4 : 3;
			minButtons = (gameOver) ? 3 : 2;
			
			selectedButton -= (Input.GetKeyDown(KeyCode.RightArrow)) ? 1 : 0;
			selectedButton += (Input.GetKeyDown(KeyCode.LeftArrow))  ? 1 : 0;
			selectedButton  = (selectedButton > 3)                   ? 2 : selectedButton;
			selectedButton  = (selectedButton < 2)                   ? 3 : selectedButton;
			
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				currentScreen = 1;
				selectedButton = 1;
				objectEndScreen.SetActive(false);
				objectPauseMenu.SetActive(true);				
				pausedGame.SetActive(false);
			}
			
			if (Input.GetKeyDown(KeyCode.Return))
			{
				Time.timeScale = 1;
				switch(selectedButton)
				{
					case 2:
						SceneManager.LoadScene("MainMenu");
						break;
					case 3:
						currentScreen = 0;
						objectPauseMenu.SetActive(false);
						objectEndScreen.SetActive(false);
						break;
					default:
						Debug.Log("Nothing selected");
						break;
				}
			}
		}
		
		if (!paused && !gameOver)
		{
			gameTimer += Time.deltaTime;
			gameTime = (int) gameTimer;
		
			if (gameTime > 60*60)
			{
				gameTimer = 0f;
				gameTime = 0;
			}
		
			gameTimeMinutes = gameTime / 60;
			gameTimeSeconds = gameTime - gameTimeMinutes*60;
			textCurrentTime = gameTimeMinutes.ToString("00") + ":" + gameTimeSeconds.ToString("00");
			objectCurrentTime.GetComponent<TextMesh>().text = "Time: " + textCurrentTime;
		
			if ((totalPoints + newPoints) > totalPoints)
			{
				totalPoints += newPoints;
				textTotalPoints = "Points: " + totalPoints;
				objectTotalPoints.GetComponent<TextMesh>().text = textTotalPoints;
				newPoints = 0;
			}
		}
	}
}
