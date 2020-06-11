using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon; 
using Photon.Pun;
using System.Net;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject LocalPlayerInstance;
    private static int TotalPoints;
    private int NewPoints;
    private int Time;

    void Start()
    {
        cameraWork _cameraWork = this.gameObject.GetComponent<cameraWork>();

        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();

            }
        }
        else
        {
            Debug.LogError("Missing CameraWork component on playerPrefab", this);
        }

        TotalPoints = 0;
    }

    void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Time = GameObject.Find("Global").GetComponent<globalModifiers>().gameTime;
                stream.SendNext(Time);
            }
            TotalPoints = GameObject.Find("Global").GetComponent<globalModifiers>().totalPoints;
            // We own this player: send the others our data
            stream.SendNext(TotalPoints);
        }
        else
        {
            // Network player, receive data
            if (!PhotonNetwork.IsMasterClient)
            {

                Time = (int)stream.ReceiveNext();
                GameObject.Find("Global").GetComponent<globalModifiers>().gameTimeMinutes = Time / 60;
                GameObject.Find("Global").GetComponent<globalModifiers>().gameTimeSeconds = Time - (Time / 60) * 60;
            }
            TotalPoints = (int)stream.ReceiveNext();
            GameObject.Find("Global").GetComponent<globalModifiers>().totalPoints = TotalPoints;

        }


        #endregion
    }

}
