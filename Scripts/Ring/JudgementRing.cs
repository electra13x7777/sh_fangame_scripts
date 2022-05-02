using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementRing : MonoBehaviour
{
    // Members
    public int ring_pieces;
    //public float ring_speed;

    // Members for testing
    
    // ring types
    public bool is_normal, is_beginner, is_technical, is_gamble;
    // ring dial speeds
    public bool is_fast_speed, is_medium_speed, is_slow_speed;

    // Ring Hit Booleans
    private bool ring_hit, ring_strike, ring_miss;
    // Other Members that will be accessed
    public const float ring_radius = 50f;

    // References
    public Dial dial;
    //public HitArea hit;
    //public StrikeArea strike;

    public JudgementRing() { }

    // Start is called before the first frame update
    void Start()
    {
        ring_hit = false;
        ring_strike = false;
        ring_miss = false;
        Debug.Log("RING: START FINISHED");
        //dial.rotate_dial();
        //this.dial = GameObject.FindWithTag("Dial");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (dial.player.ring_finished && this != null) 
        {
            Destroy(this);
        }
        //Debug.Log(dial.transform.rotation.w);
        
        if (dial.transform.rotation.w >= dial.del_ring.w) 
        {
            Destroy(this);
        }
        // Checks to see if player hits one of the hit/strike areas*/


    }

    void set_dial_speed() 
    {
        // get reference to dial
        if (is_fast_speed) 
        {
            dial.set_speed(120f);
        }
        if (is_medium_speed)
        {
            dial.set_speed(90f);
        }
        if (is_slow_speed)
        {
            dial.set_speed(60f);
        }
        Debug.Log("RING: SPEED SET");
    }

    /* Check if Dial is over hit area
    void OnTriggerEnter2D(Collider2D dial_collider, Collider2D area_collider) 
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // We need to check for strikes first
        if (other.gameObject.CompareTag("StrikeArea"))
        {
            this.ring_strike = true;

        }
        // now check for hit area
        else if (other.gameObject.CompareTag("HitArea"))
        {
            this.ring_hit = true;
        }
        else 
        {
            this.ring_miss = true;
        }

    }*/
}
