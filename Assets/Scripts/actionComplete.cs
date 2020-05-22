using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionComplete : MonoBehaviour
{
  public float destroyTime = 1f;
		
  void Start()
  {
    Destroy(gameObject, destroyTime);
  }
}
