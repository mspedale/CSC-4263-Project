using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles the actual projectile for player 1
public class playerProjectile : MonoBehaviour 
{
	
	public float speed = 1f;
	int direction = 1;
	Vector3 velocity = new Vector3(0,0,0);
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		// Position = position + speed
		velocity.y = speed * direction;
		gameObject.transform.position += velocity;
	}

	//destroys projectile upon impact
	void OnTriggerEnter2D(Collider2D coll) 
	{
        if(coll.gameObject.tag != "Player1" && coll.gameObject.tag!="blueDrone" && coll.gameObject.tag != "energy")
		{
			Destroy (gameObject);
        }
		
		if(coll.gameObject.tag == "Projectile_p2") {
			Destroy(gameObject);
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
