using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invaderShoot : MonoBehaviour {
public float speed = 0.5f;
	int direction = -1;
	Vector3 velocity = new Vector3(0,0,0);
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Position = position + speed
		velocity.y = speed * direction;
		gameObject.transform.position += velocity;
	}
		
	//destroys projectile upon impact
	void OnTriggerEnter2D(Collider2D coll) 
	{
        if(coll.gameObject.tag!="invader")
		{
		    Destroy (gameObject);
        }
	}

	//destroys projectile upon impact
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "Ball") 
		{
			Destroy (gameObject);
		}
	}
}
