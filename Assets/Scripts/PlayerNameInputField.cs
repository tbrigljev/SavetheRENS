using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using Photon.Pun; 
using Photon.Realtime; 

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
	#region Private Constants
	const string playerNamePrefKey = "PlayerName";
	
	private string[] names = new string[] {
		"Tohru",
		"Kyoko",
		"Arisa",
		"Saki",
		"Akito",
		"Yuki",
		"Kyo",
		"Shigure",
		"Kagura",
		"Momiji",
		"Hatori",
		"Hatsuharu",
		"Ayame",
		"Kisa",
		"Hiro",
		"Ritsu",
		"Isuzu",
		"Kureno"
	};
	#endregion

	#region MonoBehaviour CallBacks
  void Start()
	{
		string defaultName = "Choose a name !";
		InputField _inputField = this.GetComponent<InputField>();
		_inputField.text = defaultName;

		if(_inputField == null)
   	{
			if(PlayerPrefs.HasKey(playerNamePrefKey))
   		{
				defaultName = PlayerPrefs.GetString(playerNamePrefKey);
				_inputField.text = names[Random.Range(0, names.Length)];
   		}
			else
			{
				_inputField.text = names[Random.Range(0, names.Length)];
			}
   	}

		PhotonNetwork.NickName = defaultName;
  }
	#endregion

	#region Public Methods	
	public void SetPlayerName(string value)
	{
		if(string.IsNullOrEmpty(value))
		{
			//Debug.LogError("Player name is null or empty !");
			return;
		}
		else
		{
			PhotonNetwork.NickName = value;
			PlayerPrefs.SetString(playerNamePrefKey, value);
		}
	}
	#endregion
}
