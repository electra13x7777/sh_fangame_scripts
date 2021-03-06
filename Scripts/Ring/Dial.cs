using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialHitState 
{ 
    STRIKE,   // Working
    HIT,      // Working
    CHARGE,   // Implemented - More Testing Needed
    STEP,     // Working
    MODULATE, // Working
    MISS      // Working
}

// Class: Dial
// 
// Description: Defines the default behavior of the dial monobehavior.
public class Dial : MonoBehaviour
{
    public float dial_speed;
    public bool collider_flag, is_charge, flag_hitcount, flag_delring, dial_sound_played; // flag for debug output
    public DialHitState state;
    public AudioSource[] sounds;

    [SerializeField]
    public int hit_count, strike_count, step_count, modulate_count, miss_count, charge_count, total_charge_frames;
    
    // rotation related
    public Transform origin;
    public Quaternion q, originalRotation, currentRotation; //, del_ring;
    public bool is_rotating_around_parent, on_hit_area, has_hit, first_hit, first_strike;
    private Vector3 originalPosition;

    [SerializeField]
    public PlayerBattle player;
    /*
    public float lerp_time = 1.0f;
    public float rotateAmt = 1.0f;
    public float x = 1.0f;
    public float y = 1.0f;
    public float z = 1.0f;
    int deg = 0;
    */

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("DIAL: Live");
        Debug.Log($"{player.name}'s Ring Start");
        hit_count = 0;
        step_count = 0;
        modulate_count = 0;
        strike_count = 0;
        miss_count = 0;
        charge_count = 0;
        is_charge = false;
        dial_sound_played = false;
        this.first_hit = false;
        this.first_strike = false;
        this.on_hit_area = false;
        this.has_hit = false;
        state = DialHitState.MISS; // Initialize to MISS; Works without this but wanna make sure; We will never have a ring start on non miss.

        // 
        q = transform.rotation;
        set_speed(120f);
        // Set Position and Rotation for Dial to reset back to
        Vector3 origin_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        this.originalPosition = origin_pos;
        Quaternion origin_rot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, 0);
        this.originalRotation = origin_rot;
        this.currentRotation = origin_rot;
        //this.del_ring = new Quaternion(0f, 0f, 0.19234f, 0.98133f);
        this.is_rotating_around_parent = true;
        player.is_miss = false;
        //rotate_dial();
        //StartCoroutine(RotateDial());
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the ring
        //this.rotate_dial();
        
       
        if (!dial_sound_played) 
        {
            
            sounds[3].Play();
            dial_sound_played = true;
        }
        this.rotate_dial();


        // HANDLE WHEN PLAYER MISSES A HIT AREA
        switch (state) 
        {
            case DialHitState.MISS:
                if (!has_hit && on_hit_area)
                {
                    // TODO: Add check for normal ring and update accordingly
                    has_hit = false;
                    on_hit_area = false;
                    is_charge = false;
                    this.first_hit = false;
                    this.first_strike = false;
                    sounds[2].Play(); // Miss Sound
                    Debug.Log("MISS!");
                    miss_count = 1;
                }
                else if (has_hit && on_hit_area) 
                {
                    has_hit = false;
                    on_hit_area = false;
                    //is_charge = false;
                }
                break;
            case DialHitState.CHARGE:
                total_charge_frames++;
                break;
            default:
                break;
        }
        
        // HANDLE SINGLE-HIT HIT AREAS
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Current Execution pattern for non charge rings
            // Could change if I end up going with a more object oriented singleton pattern approach
            // OOP is very scary and I fear that I will become lame and object oriented million manager class memelord
            // but I need to stop being a libertarian ancap loonicks larp god all my data should be data and only care about itself very limited message passing between things cringelord
            // Why yes I do run ArchLinux. How could you tell? Gigachad_smile.png
            switch (state)
            {
                case DialHitState.STRIKE:
                    if(!flag_hitcount)
                        Debug.Log("STRIKE!");
                    strike_count++;
                    player.strikes++; // not working
                    if (flag_hitcount)
                        Debug.Log($"STRIKE! {strike_count}");
                    if (!has_hit)
                    {
                        has_hit = true;
                    }
                    if (!this.first_hit && !this.first_strike) 
                    {
                        this.first_strike = true;
                    }
                    else if (this.first_hit && !this.first_strike)
                    {
                        this.first_strike = false;
                    }
                    sounds[0].Play(); // Strike Sound                
                    break;
                case DialHitState.HIT:
                    if (!flag_hitcount)
                        Debug.Log("HIT!");
                    hit_count++;
                    if (flag_hitcount)
                        Debug.Log($"HIT! {hit_count}");
                    if (!has_hit)
                    {
                        has_hit = true;
                    }
                    if (!this.first_hit && !this.first_strike)
                    {
                        this.first_hit = true;
                    }
                    else if(!this.first_hit && this.first_strike) 
                    {
                        this.first_hit = false;
                    }
                    sounds[1].Play(); // Hit Sound
                    player.hits++;
                    break;
                case DialHitState.STEP: // 
                    if (!flag_hitcount)
                        Debug.Log("STEP!");
                    step_count++;
                    if (flag_hitcount)
                        Debug.Log($"STEP! {step_count}");
                    if (!has_hit)
                    {
                        has_hit = true;
                    }
                    sounds[1].Play(); // Hit Sound
                    break;
                case DialHitState.MODULATE:
                    if (!flag_hitcount)
                        Debug.Log($"MODULATE!");
                    modulate_count++;
                    if(flag_hitcount)
                        Debug.Log($"MODULATE! {modulate_count}");
                    sounds[1].Play(); // Hit Sound
                    player.modulates++;
                    break;
                case DialHitState.MISS:
                    Debug.Log("MISS!");
                    sounds[2].Play(); // Miss Sound
                    //player.is_miss = true;
                    miss_count++;
                    break;
            }
        }

        // Handle Key Holds (Probably just for charge areas)
        // THIS IS KINDA BUGGY
        if (Input.GetKey(KeyCode.Z) && is_charge) 
        {
            switch (state) 
            {
                case DialHitState.CHARGE:
                    Debug.Log($"CHARGING! {++charge_count}"); // prefix increment in an interpolated string in a log statement looks really sexy goddam
                    sounds[0].Play(); // Charge Sound
                    //is_charge = true;
                    if (!has_hit)
                    {
                        has_hit = true;
                    }
                    if (!is_charge)
                    {
                        is_charge = true;
                    }
                    break;
                case DialHitState.MISS:
                    if (!is_charge) 
                    {
                        break;
                    }
                    has_hit = false;
                    on_hit_area = false;
                    is_charge = false;
                    miss_count = 1;
                    Debug.Log("MISS AND CHARGE BREAK!");
                    sounds[1].Play(); // Miss Sound
                    break;
            }
        }

            // Update our player
        player.strikes = strike_count;
        //player.hits = hit_count;
        //if (player.strikes >= player.ring_pieces || player.hits >= player.ring_pieces) 
        //if(strike_count > 0)
        //{
            
            //player.ring_finished = true; // Working
            //Debug.Log($"Strike Count: {player.strikes}, FINFLAG: {player.ring_finished}");
            //DestroyImmediate(player.jr_go);
        //}
    }
    
    // standard setter
    public void set_speed(float speed) 
    {
        this.dial_speed = speed;
        Debug.Log("DIAL: SPEED SET");
    }

    // Rotates the dial at the speed set on the game object
    public void rotate_dial() 
    {
        this.transform.RotateAround(this.transform.parent.position, new Vector3(0, 0, (-1) * this.transform.parent.forward.z), this.dial_speed * Time.deltaTime * 1.5f);
        this.currentRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);

        //Debug.Log(this.currentRotation);
    }

    IEnumerator RotateDial()
    {
        //Debug.Log("Dial Rotation Starting");
        float s = 0.5f;
        yield return new WaitForSeconds(s);
        //rotate_dial();
    }

    public void reset_dial_pos()
    {
        if (this.gameObject.activeInHierarchy)
        {
            Debug.Log("Ring Dial Reset");
            this.transform.position = this.originalPosition;
            this.transform.rotation = this.originalRotation;
        }
    }

    // Member Funtion: OnTriggerEnter2D
    //
    // Description: Standard 2D Collider callback. Sets flags for the dial to be handled in update
    void OnTriggerEnter2D(Collider2D other)
    {

        // We need to check for strikes first
        if (other.gameObject.CompareTag("StrikeArea"))
        {
            if (collider_flag)
                Debug.Log("DIAL: ON STRIKE AREA");
            state = DialHitState.STRIKE;
            if (!on_hit_area)
            {
                on_hit_area = true;
            }
            if (!has_hit) 
            {
                has_hit = false;
            }
        }

        // Hit Areas
        else if (other.gameObject.CompareTag("HitArea"))
        {
            if (collider_flag)
                Debug.Log("DIAL: ON HIT AREA");
            state = DialHitState.HIT;
            if (!on_hit_area)
            {
                on_hit_area = true;
            }
            if (!has_hit)
            {
                has_hit = false;
            }
        }
        // Step Areas
        else if (other.gameObject.CompareTag("StepArea"))
        {
            if (collider_flag)
                Debug.Log("DIAL: ON STEP AREA");
            state = DialHitState.STEP;
            if (!on_hit_area)
            {
                on_hit_area = true;
            }
            if (!has_hit)
            {
                has_hit = false;
            }
        }
        // Modulate Areas
        else if (other.gameObject.CompareTag("ModulateArea")) 
        {
            if (collider_flag)
                Debug.Log("DIAL: ON MODULATE AREA");
            state = DialHitState.MODULATE;
            if (!on_hit_area)
            {
                on_hit_area = true;
            }
            //modulate_count++;
        }
        // Charge Areas are a new mechanic I'm coming up with to function similar to how a charge character works in a fighting game
        // You need to hold the button down to get the best results from them
        else if (other.gameObject.CompareTag("ChargeArea"))
        {
            if (collider_flag)
                Debug.Log("DIAL: ON CHARGE AREA");
            state = DialHitState.CHARGE;
            if (!on_hit_area)
            {
                on_hit_area = true;
            }
            if (!has_hit)
            {
                has_hit = false;
            }
            if (!is_charge) { is_charge = true; }
                
        }
        // check if miss
        else if (other.gameObject.CompareTag("MissArea"))
        {
            if (collider_flag)
                Debug.Log("DIAL: ON MISS AREA");

            state = DialHitState.MISS;
            
            // use this to reset all other flags 

        }
    }
}
