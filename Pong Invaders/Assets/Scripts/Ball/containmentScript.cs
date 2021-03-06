using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//handles ball container behavior
public class containmentScript : MonoBehaviour 
{

	int maxHP;
	int hp;
	float ballForce = 500f;
    bool paused = false;
	
	public GameObject prefab_ball;
	private ballScript ballScr;
    public GameObject InvaderFleet;
    public GameObject InvaderFleet2;
    public GameObject energy;
    public GameObject energy2;
    Rigidbody2D ballBody;
	Rigidbody2D containment;
	
	// Oscillation variables
	float amp =  6.5f;		                        // Amplitude of displacement
	float freq = 0f;	                            // Frequency of oscillation  (increases with every shot fired)
	float t = 0f;		                	        // Time since oscillation began (in frames)
	float x = 0f; 		                            // Current x position
	float prevX = 0f;	                            // Previous x position
	Vector3 newPosition = new Vector3 (0f,0f,0f);	// Need a Vector3 for use with MovePosition()
	int direction = 1;	                            // Indicates whether the containment is going right or left. Crucial for maintaining direction when frequency changes.

	
    //audio
    AudioSource hitSound;
    //AudioSource relSound;

    // Use this for initialization
    void Start ()
    {
		//tentative array-based sound implementation, may be unneccessary
        //AudioSource[] sounds = GetComponents<AudioSource>();
        //hitSound = sounds[0];
        //relSound = sounds[1];

		//initalize containment HP
		maxHP = 5; //rand.Next(6,12); (testing health)
		hp = maxHP;
		containment = gameObject.GetComponent<Rigidbody2D>();
		
        Instantiate(InvaderFleet, new Vector2(0f, -1.8f), Quaternion.identity);
        Instantiate(InvaderFleet2, new Vector2(0f, 1.8f), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update ()
    {
		// pause detection
		if (Input.GetKeyDown ("escape")) 
		{
			paused = !paused;
			//Debug.Log ("paused = " + paused);
		}
			

		//Movement oscillation speed based on HP
		if(hp < maxHP) 
		{
			// If NOT paused, move normally
			if (paused == false) 
			{
				t += 1 * direction;
				prevX = x;
				x = amp * Mathf.Sin (2 * Mathf.PI * freq * t);	// oscillation formula, in terms of x.
				newPosition.x = x;
				containment.MovePosition (newPosition);
			}
		}
	}
	
	// Collision event (hopefully just with player projectiles)
	void OnTriggerEnter2D(Collider2D other)
    {	
		
		string collTag = other.gameObject.tag;
		
		// If containment is hit by player projectile
		if (collTag == "Projectile_p1" || collTag == "Projectile_p2") //other.gameObject.name == "playerProjectile(Clone)")
		{
			// HP modification
			hp -= 1;
			
			// Frequency modification
			freq += 0.001f;
			
			// If not moving
			if (x-prevX == 0) 
			{
				// direction is set depending on whether the initial shot collided on the left or right of the containment
				direction = (int)Mathf.Sign(gameObject.transform.position.x - other.transform.position.x);
			}
			// If moving right
			else if (x-prevX > 0) 
			{
				t = (Mathf.Asin(x/amp) + (2* Mathf.PI)) / (2*Mathf.PI*freq);	// Oscillation formula, in terms of t. This offsets t using the new x value, so x won't make a crazy jump when frequency changes
				direction = 1;
			}
			// If moving left
			else 
			{
				t = ((Mathf.Asin(x/amp) + (2* Mathf.PI)) / (2*Mathf.PI*freq));  // Oscillation formula, in terms of t.
				direction = -1;
			}
			
			//audio, plays if containment is hit by projectile
			gameObject.GetComponent<AudioSource>().Play();
			
			// If HP has reached 0
			if (hp < 1)
			{
				// Activate ball, dissolve child/parent relationship :'(
				prefab_ball.SetActive(true);
				prefab_ball.transform.SetParent(null);
				
				// Set ballScript so we can set possession.
				ballScr = prefab_ball.GetComponent<ballScript>();
				
				// If player One (bottom) gets last shot
				if (collTag == "Projectile_p1")
				{
					// Throw ball upwards, set possession, and spawn energy for p1
					prefab_ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-150.0f, 150.0f), ballForce));
					ballScr.SetPossession(1);
					Instantiate(energy, newPosition, Quaternion.identity);
				}
				else // Player Two (top) gets last shot
				{
					// Throw ball down, set possession, spawn energy for p2
					prefab_ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-150.0f, 150.0f),-ballForce));
					ballScr.SetPossession(-1);
					Instantiate(energy2, newPosition, Quaternion.identity);
				}
				Destroy (gameObject);		
			}
		}
	}
}
