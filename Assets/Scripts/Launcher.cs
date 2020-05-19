using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 
using Photon.Realtime; 

public class Launcher : MonoBehaviourPunCallbacks
{

	#region Private Serializable Fields

    [Tooltip("Maximum number of players per room.")]
    [SerializeField]
    private byte maxPlayersPerRoom = 2; 

    [Tooltip("Control panel to enter the username and play")]
    [SerializeField]
    private GameObject controlPanel; 

    [Tooltip("Informs a connection is in progress")]
    [SerializeField]
    private GameObject progressLabel; 

	#endregion



	#region Private Fields
    bool isConnecting; 
	string gameVersion = "1"; 

	#endregion



	#region MonoBehaviour CallBacks

	void Awake()
	{
		PhotonNetwork.AutomaticallySyncScene = true; 
	}
    // Start is called before the first frame update
    void Start()
    {
        progressLabel.SetActive(false); 
        controlPanel.SetActive(true); 
    }
    #endregion



    #region Public Methods

    public void Connect()
    {
        progressLabel.SetActive(true); 
        controlPanel.SetActive(false); 
        
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
        if(isConnecting)
        {
            PhotonNetwork.JoinRandomRoom(); 
            isConnecting = false; 
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false); 
        controlPanel.SetActive(true);
        isConnecting = false; 
        Debug.LogWarningFormat("Disconnected with reason {0}", cause); 
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnjoinRandomFailed() called by PUN, no random room available. Creating one..."); 
        PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayersPerRoom}); 
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Try to join room"); 
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Level0 loaded."); 
            PhotonNetwork.LoadLevel("Level0"); 
        }
        Debug.Log("OnJoindedRoom called by PUN, client joined the room."); 
    }

    #endregion

}
