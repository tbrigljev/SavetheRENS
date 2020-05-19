using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using Photon.Pun; 
using Photon.Realtime; 


public class GameManager : MonoBehaviourPunCallbacks
{
    
    [Tooltip("The prefab representing the player")]
    public GameObject playerPrefab; 

	#region Photon Callbacks
	public override void OnLeftRoom()
	{
		SceneManager.LoadScene(0); 
	}

	public override void OnPlayerEnteredRoom(Player other)
	{
		Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); 

		if(PhotonNetwork.IsMasterClient)
		{
			Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); 

			LoadArena(); 
		}
	}

	public override void OnPlayerLeftRoom(Player other)
	{
		Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); 

		if(PhotonNetwork.IsMasterClient)
		{
			Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); 

			LoadArena(); 
		}
	}
	#endregion 


	#region Public Methods

	public void Start()
	{
		if(playerPrefab==null)
		{
			Debug.LogError("Missing playerPrefab reference."); 
		}
		else
		{
			Debug.LogFormat("Instantiating player from {0}", Application.loadedLevelName); 
			PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0); 
		}
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom(); 
	}
	#endregion

	#region Private Methods

	void LoadArena()
	{
		if(!PhotonNetwork.IsMasterClient)
		{
			Debug.LogError("PhotonNetwork : Trying to load a level but we are not the master"); 
		}
		
		Debug.LogFormat("PhotonNetwork : Loading Level {0}", PhotonNetwork.CurrentRoom.PlayerCount);
		PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);  
	}


	#endregion

}
