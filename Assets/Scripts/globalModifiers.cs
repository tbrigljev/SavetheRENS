using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class globalModifiers : MonoBehaviourPun
{
	private GameObject objectTotalPoints;
	private GameObject objectCurrentTime;
	private GameObject objectPauseMenu;
	private GameObject objectEndScreen;
	private GameObject backEndButton;
	private GameObject pauseQuitButton;
	private GameObject resumeButton;
	private GameObject visitGameButton;
	private GameObject pausedGame;
	private GameObject[] players;
	
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
	public int gameTime;
	public int gameTimeSeconds;
	public int gameTimeMinutes;
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
	private bool up;
	private bool down;
	private bool left;
	private bool right;
	
  void Start()
  {
		playerSpeedModifier = 1f;
		missionsPointsModifier = 1f;
		completeMissionsModifier = 1f;
		cooldownMissionsModifier = 1f;
		
		paused = false;
		gameOver = false;
		up = false;
		down = false;
		left = false;
		right = false;
		
		totalPoints = 0;
		pointsTarget = 50;
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
		visitGameButton = GameObject.Find("ButtonVisitGame");
		
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
		
		//left = (Input.GetAxis("Horizontal") < -0.8f) ? true : false;
		//right = (Input.GetAxis("Horizontal") > 0.8f) ? true : false;
		//up = (Input.GetAxis("Vertical") < -0.8f) ? true : false;
		//down = (Input.GetAxis("Vertical") > 0.8f) ? true : false;
		
		
		if (totalPoints > pointsTarget)
		{
			if (!gameOver)
			{
				backgroundMusic.Play();
				backgroundMusic.clip = musicEndScreen;
				backgroundMusic.Play();
				
				gameOver = true;
				//currentScreen = 2;
				//selectedButton = 3;
				objectEndScreen.SetActive(true);
				visitGameButton.gameObject.GetComponent<Button>().Select();
				backEndButton.SetActive(true);
				objectTotalPoints.SetActive(false); 
				objectEndScreenTime.GetComponent<Text>().text = "Level completed in: " + textCurrentTime;
				objectEndScreenPoints.GetComponent<Text>().text = "Total points: " + totalPoints;
				objectEndScreenMissions.GetComponent<Text>().text = "Total missions: " + filedMissions;
				
				resumeButton.transform.Find("Text").gameObject.GetComponent<Text>().text = "Revisit room";
				//backEndButton.GetComponent<selectColorPauseMenu>().buttonIndex = 2;
				//pauseQuitButton.GetComponent<selectColorPauseMenu>().buttonIndex = 3;
				
				gameOver = true;
				selectedButton = 3;
			}
		}
		
		if (Input.GetButtonDown("Cancel"))
		{
			if (gameOver)
			{
				objectEndScreen.SetActive(false);
				
				if (objectPauseMenu.activeSelf)
				{
					Time.timeScale = 1;
					objectPauseMenu.SetActive(false);					
				}
				else
				{
					Time.timeScale = 0;
					objectPauseMenu.SetActive(true);
					resumeButton.gameObject.GetComponent<Button>().Select();
				}
			}
			
			if (paused)
			{
				unpause();
			}
			else
			{
				pause();
			};
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

			if (PhotonNetwork.IsMasterClient)
			{
				gameTimeMinutes = gameTime / 60;
				gameTimeSeconds = gameTime - gameTimeMinutes * 60;
			}
			textCurrentTime = gameTimeMinutes.ToString("00") + ":" + gameTimeSeconds.ToString("00");
			objectCurrentTime.GetComponent<TextMesh>().text = "Time: " + textCurrentTime;

			/*if ((totalPoints + newPoints) > totalPoints)
			{
				totalPoints += newPoints;
				textTotalPoints = "Points: " + totalPoints;
				
				
				newPoints = 0;
				objectTotalPoints.GetComponent<TextMesh>().text = textTotalPoints;
			}*/
			textTotalPoints = "Points: " + totalPoints;
			objectTotalPoints.GetComponent<TextMesh>().text = textTotalPoints;


		}
	}
	
	#region Public Methods
	public void pause()
	{
		paused = true;
		if (gameOver)
		{
			pausedGame.SetActive(false);
		}
		else
		{
			Time.timeScale = 0;
		}
		objectPauseMenu.SetActive(true);
		resumeButton.gameObject.GetComponent<Button>().Select();
	}
	
	public void unpause()
	{
		pauseQuitButton.gameObject.GetComponent<Button>().Select();
		paused = false;
		//print("Before is: " + Time.timeScale);
		Time.timeScale = 1;
		//print("After is: " + Time.timeScale);
		objectPauseMenu.SetActive(false);
	}
	
	public void backLevel()
	{
		objectPauseMenu.SetActive(false);
		objectEndScreen.SetActive(false);
		pausedGame.SetActive(false);
	}
	
	public void endScreen()
	{
		objectPauseMenu.SetActive(false);
		objectEndScreen.SetActive(true);
		visitGameButton.gameObject.GetComponent<Button>().Select();
	}
	
	public void quitToMainMenu()
	{
		Time.timeScale = 1;	
		SceneManager.LoadScene("Launcher");
		/*players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players)
		{
			Destroy(player);
		}*/
	}
	#endregion
}
