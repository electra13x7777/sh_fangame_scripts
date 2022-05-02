using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script: RingTester.cs
// Author: Alex Barney
//
// Description: A script that sets up a test suite for all of the ring prefabs in the game.
//

// ENUMS : CURRENTLY UNUSED
public enum RingType 
{
    NORMAL,
    BEGINNER,
    TECHNICAL,
    GAMBLE,
    CHARGE     // Not really a ring type
}

public enum NumAreas 
{
    ONE,
    TWO,
    THREE
}

// Object Definition
public class RingTester : MonoBehaviour
{
    public Transform jr_loc_init;
    //public GameObject[] jr_fabs;
    public List<GameObject> jr_fab_list;
    public AudioSource[] sounds;
    int index;
    public bool auto_del_ring, bad_debug_flag_too_stupid_to_program_well;
    int frames;
    GameObject jr_go;
    // Start is called before the first frame update
    void Start()
    {
        //init list
        //jr_fab_list = new List<GameObject>();
        this.index = 0;
        this.frames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            this.sounds[1].Play();
            if (index + 1 == jr_fab_list.Count)
            {
                index = 0;
                Debug.Log($"RING INDEX: {index}");
            }
            else
            {
                Debug.Log($"RING INDEX: {++index}");
            }
        }
        if (Input.GetKeyDown(KeyCode.A) && this.jr_go == null)
        {
            this.jr_go = Instantiate(jr_fab_list[index] , jr_loc_init);
            //jr_go.GetComponent<>
            //jr_fab.SetActive(true);
        }
        //if(autoring)
        if (Input.GetKeyDown(KeyCode.S) && this.jr_go != null)
        {
            this.sounds[0].Play();
            Destroy(this.jr_go);
        }
        if (auto_del_ring && this.jr_go != null) // not working well
        {
            this.frames++;
            Debug.Log(frames);
            if (frames == 240) 
            {
                this.sounds[0].Play();
                this.frames = 0;
                Destroy(this.jr_go);
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            this.sounds[2].Play(); // Don't mess with New Yorkers
        }

    }
}
