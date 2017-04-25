using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//handles mothership behavior.
//mothership has a shield that is disabled by the ball,
//and health that is lowered by damage from fighter ship weapons.
//once the mothership health reaches zero, the ship is destroyed and the round ends.


public class motherScript : MonoBehaviour 
{
	
    //audio
    AudioSource shielddisable;
    AudioSource shiphit;
    AudioSource shieldenable;
    //AudioSource shipdestroyed;


    public GameObject shieldObj;
    public GameObject ContainmentPrefab;
    public GameObject healthBar;
    int eng =0;
    int hp=10;
    bool shield=true;
    GameObject shieldclone;
    Vector3 position = new Vector3(0f,-10.18f,0f);

    void OnTriggerEnter2D(Collider2D other)
	{
        if(other.gameObject.name=="energy(Clone)")
        {
         eng++;
         Destroy(other.gameObject);
        }
        //handles HP if shield is down
        if(shield==false && other.gameObject.name!="energy")
		{
            print(hp);
        	hp=hp-1;
            decreaseHealth();
            shiphit.Play();
        
			if(hp<1)
			{
				Application.LoadLevel("P2Win");
            	//Destroy (gameObject);
			}
        }

		//handles shield if ball hits mothership
        if(other.gameObject.tag=="Ball")
		{
            print("BallHit");
            shield=false;
            shieldclone.SetActive(false);
            //Destroy(shieldclone);
            print(shield);
            Destroy (other.gameObject);
            StartCoroutine(shieldDelay());
            shielddisable.Play();
        }
    }

    /*
    void OnGUI()   for text HP display
    {
        GUI.Label(new Rect(Screen.width - 500, Screen.height - 50, Screen.width, Screen.height), "TEST TEST TEST");
    }
    */

    // method that decreases HP on hit for the health bar
    protected void decreaseHealth()
    {
        float calc_health = hp / 10f;
        setHealthBar(calc_health);
    }

    // changes the size of the HP bar
    public void setHealthBar(float myHealth)
    {
        healthBar.transform.localScale = new Vector3(myHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        healthBar.transform.position = new Vector3(healthBar.transform.position.x - 0.15f, healthBar.transform.position.y, healthBar.transform.position.z);
    }

    //regens shield over time
    IEnumerator shieldDelay() 
	{
		
        yield return new WaitForSeconds(5);
        shield=true;
        shieldclone.SetActive(true);
        //GameObject shieldclone = Instantiate(shieldObj, position, Quaternion.identity) as GameObject;
        GameObject Containment = Instantiate(ContainmentPrefab, new Vector2(0,0), Quaternion.identity);
        shieldenable.Play();
 	}

	// Use this for initialization
	void Start () 
	{
        shieldclone  = Instantiate(shieldObj, position, Quaternion.identity) as GameObject;

        //audio
        AudioSource[] audio = GetComponents<AudioSource>();
        shielddisable = audio[0];
        shiphit = audio[1];
        shieldenable = audio[2];
        //shipdestroyed = audio[3];
    }
	
	// Update is called once per frame
	void Update () 
	{
		if(eng>=10){
        //drone fleet spawn code
        }
	}
}
