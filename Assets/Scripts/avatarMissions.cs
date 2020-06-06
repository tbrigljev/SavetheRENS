using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class avatarMissions : MonoBehaviour
{
	private GameObject currentMission;
	private GameObject canvas;
	private GameObject bookIcon;
	private GameObject mapIcon;
	private GameObject tabIcon;
	private GameObject fileIcon;
	
	public Quaternion angle;
	
	public string currentTag;
	public int currentPoints;
	
	public bool taskDone;
	private bool inMission;
	
  void Start()
	{
		taskDone = false;
		
		angle = Quaternion.Euler(40, 180, 0);
		
		canvas = transform.Find("Canvas").gameObject;
		bookIcon = canvas.transform.Find("Book").gameObject;
		mapIcon = canvas.transform.Find("Map").gameObject;
		tabIcon = canvas.transform.Find("Tab").gameObject;
		fileIcon = canvas.transform.Find("File").gameObject;
		
		bookIcon.SetActive(false);
		mapIcon.SetActive(false);
		tabIcon.SetActive(false);
		fileIcon.SetActive(false);
  }

  void Update()
	{
		canvas.transform.rotation = angle;
		
		inMission = gameObject.GetComponent<avatarControls>().inMission;		
		if (inMission)
		{
			currentMission = transform.Find("currentMission").gameObject;
			
			currentTag = currentMission.GetComponent<missionsTracking>().currentTag;
			currentPoints = currentMission.GetComponent<missionsTracking>().currentPoints;
			
			switch (currentTag)
			{
				case "shelf":
					bookIcon.SetActive(true);
					mapIcon.SetActive(false);
					tabIcon.SetActive(false);
					fileIcon.SetActive(false);
					break;
				case "map":
					bookIcon.SetActive(false);
					mapIcon.SetActive(true);
					tabIcon.SetActive(false);
					fileIcon.SetActive(false);
					break;
				case "tab":
					bookIcon.SetActive(false);
					mapIcon.SetActive(false);
					tabIcon.SetActive(true);
					fileIcon.SetActive(false);
					break;
				case "file":
					bookIcon.SetActive(false);
					mapIcon.SetActive(false);
					tabIcon.SetActive(false);
					fileIcon.SetActive(true);
					break;
				default:
					bookIcon.SetActive(false);
					mapIcon.SetActive(false);
					tabIcon.SetActive(false);
					fileIcon.SetActive(false);
					break;
			}
		}
		else
		{
			currentTag = "None";
			currentPoints = 0;
			bookIcon.SetActive(false);
			mapIcon.SetActive(false);
			tabIcon.SetActive(false);
			fileIcon.SetActive(false);
		}
		
		if (taskDone)
		{
			currentMission.GetComponent<missionsTracking>().taskDone = taskDone;
			taskDone = false;
		}
	}
}
