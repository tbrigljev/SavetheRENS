using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class avatarControlsEmpty : MonoBehaviour
{
    private Rigidbody rb; 
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    void FixedUpdate()
    {
        float moveVertical = -Input.GetAxis("Horizontal"); 
        float moveHorizontal = Input.GetAxis("Vertical"); 

        Vector3 movement = new Vector3(moveHorizontal,0.0f,moveVertical);
				Vector3 looking = new Vector3(moveVertical,0.0f,-moveHorizontal);
				if(movement != Vector3.zero)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(looking), 0.15f);
				}
        //rb.AddForce (movement*speed);
				
				Vector3 newposition = transform.position + movement * speed * Time.deltaTime;
				//Debug.Log(newposition);
				rb.MovePosition(newposition);
				
				/*
				if(((newposition.x > 0.7) && newposition.x < 9.3) && ((newposition.z > -6.3) && (newposition.z < 6.3)))
				{
					rb.MovePosition(newposition);
				}*/
				
				
				//DO NOT USE TRANSLATE WITH RIGIDBODY
        //transform.Translate (movement * speed * Time.deltaTime, Space.World);
    }

    /*void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false); 
            score = score +1; 
            countTextFunc(); 
        }
    }*/


}
