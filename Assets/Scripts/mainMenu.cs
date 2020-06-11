using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class mainMenu : MonoBehaviourPunCallbacks
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


	#region Private Serializable Fields

	[Tooltip("Maximum number of players per room.")]
	[SerializeField]
	private byte maxPlayersPerRoom = 2;
	#endregion

	#region Private Fields
	bool isConnecting;
	string gameVersion = "1";

	#endregion

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
						Connect();
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
	

	void Awake()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
	}


	#region Public Methods

	public void Connect()
	{

		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			isConnecting = PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = gameVersion;
		}

	}
	#endregion


	#region MonoBehaviourPunCallbacks CallBacks

	public override void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster called by PUN");
		if (isConnecting)
		{
			PhotonNetwork.JoinRandomRoom();
			isConnecting = false;
		}
	}

	public override void OnDisconnected(DisconnectCause cause)
	{

		isConnecting = false;
		Debug.LogWarningFormat("Disconnected with reason {0}", cause);
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("OnjoinRandomFailed() called by PUN, no random room available. Creating one...");
		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Try to join room");
		if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
		{
			Debug.Log("SimpleLevel1 loaded.");
			PhotonNetwork.LoadLevel("SimpleLevel1");
		}
		Debug.Log("OnJoindedRoom called by PUN, client joined the room.");
	}

	#endregion



}
