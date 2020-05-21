using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMusic : MonoBehaviour
{
	public float endIntro;
	
  void Start()
  {
		endIntro = 107f;
    StartCoroutine(AudioLoop(endIntro));
  }
	
	 IEnumerator AudioLoop (float endIntro)
  {
		AudioSource sound = GetComponent<AudioSource>();
		
    sound.loop = false;
    float l = sound.clip.length;
    int t = 0;
    sound.Play ();
    yield return new WaitForSeconds (endIntro);
    t = sound.timeSamples;
    yield return new WaitForSeconds (l - endIntro);
    
		LOOP:
			sound.timeSamples = t;
			sound.Play ();
			yield return new WaitForSeconds (l - endIntro);
      goto LOOP;
  }	
}
