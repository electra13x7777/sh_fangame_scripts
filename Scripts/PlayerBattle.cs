using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerBattle : MonoBehaviour
{

    public int hp, mp, sp, place_holder_dmg; // Health, Magic, Sanity
    public int agi;
    public float stock; // Stock Gauge
    public string name;
    public int MAX_HP, MAX_MP, MAX_SP;

    // Player's Standard Ring
    //public JudgmentRing jr;
    public GameObject jr_fab;
    public Transform jr_loc_init;
    public GameObject jr_go;
    public int strikes, hits, misses;
    public int ring_pieces;
    public bool ring_finished, mode_db, is_done, is_miss, is_dead, is_winner, is_tech;

    public AudioSource[] sounds;
    public Bag bag;
    public int ring_num;
    [SerializeField]
    Dial dial;
    //JudgmentRing jr;

    public PlayerBattle()
    {
        /*
        this.hp = 30;
        this.mp = 65;
        this.sp = 10;
        this.place_holder_dmg = 18;
        this.agi = 10;
        this.name = "Raidou";
        */
    }

    //public string[] movelist = new string[] {}
    // Start is called before the first frame update
    void Start()
    {
        //BattleSystem Instance = BattleSystem.GetInstance();
        //jr_go = Instance.jr_go;
        //jr_fab = BattleSystem.GetInstance().jr_fab;
        //jr_loc_init = BattleSystem.GetInstance().jr_loc_init;
        mode_db = true;
        if (mode_db)
        {
            this.hp = 50;
            this.mp = 65;
            this.sp = 10;
            this.MAX_HP = this.hp;
            this.MAX_MP = this.mp;
            this.MAX_SP = this.sp;
            this.place_holder_dmg = 18;
            this.agi = 10;
            this.name = "Raidou";
            this.ring_pieces = 3;
            this.strikes = 0;
            this.hits = 0;
            this.misses = 0;
            this.is_dead = false;
            this.is_miss = false;
            this.is_winner = false;
            this.is_tech = true;
            this.ring_num = 5;
        }
    }



    // Update is called once per frame
    void Update()
    {
        //Instance = Dial.GetInstance();
        if(dial != null && this.jr_go != null)
        {
            this.hits = dial.hit_count;
            this.strikes = dial.strike_count;
            this.misses = dial.miss_count;
            if (hits >= ring_pieces || strikes >= ring_pieces || hits + strikes >= ring_pieces || misses >= 1) 
            {
                //StartCoroutine(RemoveDial());
                Destroy(dial.gameObject);
                //DestroyImmediate(GameObject.FindGameObjectsWithTag("Ring")[0]);
            }
        }
        if (this.hp <= 0)
        {
            is_dead = true;
        }

        //Debug.Log($"Hits: {this.hits}, Strikes:{this.strikes}");
        if (this.ring_finished && this.jr_go != null)
        {
            // Destroy(this.jr_go);
        }
        if (this.jr_go == null) 
        {
            this.hits = 0;
            this.strikes = 0;
            this.misses = 0;
        }
        if (this.is_winner) 
        {
            sounds[0].Play();
            this.is_winner = false;
        }
        // catch any fallthrough misses
        /*if (this.jr_go != null && this.dial == null) 
        {
            this.misses = 1;
            this.ring_pieces = 1;
        }*/

    }

    public void Setup()
    {

    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void GetRing(GameObject jr_fab, Transform jr_loc_init, GameObject jr_go)
    {
        this.jr_fab = jr_fab;
        this.jr_loc_init = jr_loc_init;
        //this.jr_go = jr_go;
    }
    //JudgmentRing spawn_ring()
    public GameObject spawn_standard_ring(GameObject ring)
    {
        Debug.Log("PB: Getting Ring Input For Standard...");
        this.ring_finished = false;
        //.is_done = false;
        GameObject jr_go = Instantiate(ring, jr_loc_init);

        //JudgmentRing judgment_ring = jr_go.GetComponent<JudgmentRing>();
        Debug.Log($"Ring Created");
        // Kinda f'd up way to get the dial when it is spawned in
        this.dial = GameObject.FindGameObjectsWithTag("Dial")[0].GetComponent<Dial>();
        //this.jr = GameObject.FindGameObjectsWithTag("Ring")[0].GetComponent<JudgmentRing>();
        return jr_go;
        //return jr;
    }

    // when spawning a ring to use an item we need to get the Item's ring fab
    public GameObject spawn_item_ring(GameObject ring)//Item item)
    {
        Debug.Log("PB: Getting Ring Input For Item...");
        this.ring_finished = false;
        GameObject jr_go = Instantiate(ring, jr_loc_init);
        Debug.Log($"Ring Created");
        // Kinda f'd up way to get the dial when it is spawned in
        this.dial = GameObject.FindGameObjectsWithTag("Dial")[0].GetComponent<Dial>();
        return jr_go;
    }


///////////////////////////////////////////////////////////////////////////////////////////
//                               HANDLE RING INPUT RESULTS
///////////////////////////////////////////////////////////////////////////////////////////




    public void StandardAttack(EnemyBattle e)//, Action onAttackComplete) 
    {
        if (this.misses >= 1) 
        {
            Debug.Log($"{this.name} deals no damage to {e.name}!");
            return;
        }
        int damage_dealt = 0; // = place_holder_dmg;

        // 1 HIT RING IS THE LEAST COMMITMENT AND GIVES THE LEAST RISK VS REWARD. RING FOR SPEEDRUNNERS
        // 2 HIT RING IS MEDIUM COMMITMENT. RING FOR INSANE PEOPLE
        // 3 HIT RING HIT AREAS DO VERY LITTLE BUT STRIKES DO A LOT OF DAMAGE. RING FOR SUPER PLAYERS
        if (this.hits > 0)
        {
            if (this.ring_pieces == 1)
            {
                damage_dealt +=  place_holder_dmg * this.hits;
                //damage_dealt += damage_dealt * this.hits;
            }
            else if (this.ring_pieces == 2)
            {
                damage_dealt += (place_holder_dmg / 2) * this.hits;
            }
            else if (this.ring_pieces == 3)
            {
                damage_dealt += (place_holder_dmg / 4) * this.hits;
            }
        }
        if (this.strikes > 0)
        {
            if (this.ring_pieces == 1)
            {
                damage_dealt = ((place_holder_dmg / 4) + place_holder_dmg) * this.strikes;
            }
            else if (this.ring_pieces == 2)
            {
                damage_dealt += ((place_holder_dmg / 6) + place_holder_dmg / 2) * this.strikes;
            }
            else if (this.ring_pieces == 3) 
            {
                damage_dealt += ((place_holder_dmg / 8) + place_holder_dmg/2) * this.strikes;
            }
        }
        Debug.Log($"{this.name} deals {damage_dealt} damage to {e.name}!");
        e.hp -= damage_dealt;

    }

    public void HandleHealingItem(Item item) 
    {
        if (this.misses >= 1)
        {
            Debug.Log($"{this.name} missed and wasn't able to heal!");
            return;
        }
        if (this.strikes >= item.ring_pieces || this.strikes >= 1)
        {
            item.UseItem(this, true);
        }
        else 
        {
            item.UseItem(this, false);
        }
    }

    /*
    IEnumerator RemoveDial()
    {
        Debug.Log("Killing Dial");
        float s = 0.5f;
        yield return new WaitForSeconds(s);
        Destroy(this.dial.gameObject);
    }

    {
        Debug.Log("PLAYERBATTLE.STANDARDATTACK");

        // set direction of target
        int damage_dealt = place_holder_dmg;
        Vector3 attack_dir = (e.GetPosition() - this.GetPosition()).normalized;
        // play animation here
        ring_finished = false;
        this.is_done = false;
        this.jr_go = Instantiate(jr_fab, jr_loc_init);
        Debug.Log("Ring Created");
        //jr_go.GetComponent<>
        //jr_fab.SetActive(true);

        //if(autoring)

        if (this.hits >= 1 || this.strikes >= 1)
        {
            //Destroy(this.jr_go);
            //ring_finished = true;
        }
        if (ring_finished)
        {
            if (this.hits > 0)
            {
                damage_dealt *= this.hits;
                //Destroy(this.jr_go);
            }
            if (this.strikes > 0)
            {
                damage_dealt = ((damage_dealt / 2) + damage_dealt) * this.strikes;
                //Destroy(this.jr_go);
            }
            Debug.Log($"{this.name} deals {damage_dealt} damage to {e.name}!");
            e.hp -= damage_dealt;
            //this.is_done=true;
        }
    }*/
}

