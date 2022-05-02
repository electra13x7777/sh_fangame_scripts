using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEPRECATED SEE DIAL.CS
//
//
public class RotateAroundAxis : MonoBehaviour
{
    public Transform origin;
    public Quaternion q, originalRotation;
    public float lerp_time = 1.0f;
    public float rotateAmt = 1.0f;
    public bool rot, rotate_around_axis, reset_dial, rotate, rot_around_parent;
    public float x = 1.0f;
    public float y = 1.0f;
    public float z = 1.0f;
    int deg = 0;
    private Vector3 originalPosition;
    
    
    // Start is called before the first frame update
    void Start()
    {
        q = transform.rotation;
        
        // Set Position and Rotation for Dial to reset back to
        Vector3 origin_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        this.originalPosition = origin_pos;
        Quaternion origin_rot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, 0);
        this.originalRotation = origin_rot;
        rot_around_parent = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rot)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * lerp_time);
        }
        if (rotate_around_axis) 
        {
            transform.RotateAround(new Vector3(0,50f,0), new Vector3(x, y, z), rotateAmt);
        }
        if (reset_dial) 
        {
            reset_dial_pos();
        }
        if (rotate) 
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, deg));
            //transform.localRoation = Quaternion.Euler(new Vector3(0, 0, deg));
            //deg++;
           // if (deg >= 360) { deg = 0; }
        }
        if (rot_around_parent) 
        {
            rot_jr_dial(120f);
        }
    }

    void snapRot() 
    {
        transform.rotation = q;
    }

    // Function: reset_dial_pos()
    //
    // Description: Resets dial position
    public void reset_dial_pos() 
    {
        if (this.gameObject.activeInHierarchy) 
        {
            Debug.Log("Ring Dial Reset");
            this.transform.position = this.originalPosition;
            this.transform.rotation = this.originalRotation;
        }
    }
    void rot_jr_dial(float speed) 
    {
        this.transform.RotateAround(this.transform.parent.position, new Vector3(0, 0, (-1) * this.transform.parent.forward.z), speed * Time.deltaTime * 1.5f);

    }

    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        // We need to check for strikes first
        if (other.gameObject.CompareTag("StrikeArea"))
        {
            Debug.Log("ROT: STRIKE");
            //this.ring_strike = true;
        }
        // now check for hit area
        else if (other.gameObject.CompareTag("HitArea"))
        {
            //this.ring_hit = true;
            Debug.Log("ROT: HIT");
        }
        else
        {
            //this.ring_miss = true;
        }

    }
    */
}
