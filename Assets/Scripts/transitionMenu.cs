using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class transitionMenu : MonoBehaviour
{
	private GameObject mainLogo;
	private GameObject mainMenuButtons;
	private GameObject darkBackground;
	
	private AudioSource menuMusic;
	
	private float time;
	private float waitTime;
	
	private bool fadeInLogo;
	private bool fadeOutLogo;
	private bool fadeInMainMenu;
	private static bool firstLaunch = true;
	
	void Start()
	{
		time = 0f;
		waitTime = 2f;
		
		mainLogo = GameObject.Find("MainMenuLogo");
		darkBackground = GameObject.Find("BackBlackLogo");
		mainMenuButtons = GameObject.Find("MainMenuButtons");
		
		mainLogo.GetComponent<Image>().color = new Color (1, 1, 1, 0);
		
		menuMusic = GameObject.Find("MainMenuMusic").GetComponent<AudioSource>();
		
		fadeInLogo = true;
		fadeOutLogo = false;
		fadeInMainMenu = false;
	}

  void Update()
	{
		time += Time.deltaTime;
		
		if (!firstLaunch && fadeInLogo)
		{
			fadeInLogo = false;
			fadeOutLogo = false;
			fadeInMainMenu = false;
			mainLogo.SetActive(false);
			darkBackground.SetActive(false);
			menuMusic.Play();
		}
		
		if (time > waitTime)
		{
			time = 0f;
			
			if (fadeInMainMenu)
			{
				fadeInMainMenu = false;
				firstLaunch = false;
				StartCoroutine(FadeInOut(darkBackground.GetComponent<Image>(), false));				
			}
			if (fadeOutLogo)
			{
				fadeOutLogo = false;
				fadeInMainMenu = true;
				StartCoroutine(FadeInOut(mainLogo.GetComponent<Image>(), false));
				menuMusic.Play();
			}
			if (fadeInLogo)
			{
				fadeInLogo = false;
				fadeOutLogo = true;
				StartCoroutine(FadeInOut(mainLogo.GetComponent<Image>(), true));
				gameObject.GetComponent<AudioSource>().Play();
			}
		}			
	}
	
	IEnumerator FadeInOut(Image image, bool fadeIn)
	{
		if (fadeIn)
		{
			for (float i = 0; i <= 1; i += Time.deltaTime*2)
			{
				i = (i > 0.96) ? 1 : i;
				image.color = new Color (1, 1, 1, i);
				yield return null;
			}
		}
		else
		{
			for (float i = 1; i >= 0; i -= Time.deltaTime*2)
			{
				i = (i < 0.04) ? 0 : i;
				image.color = new Color (1, 1, 1, i);
				yield return null;
			}
		}
	}
}
