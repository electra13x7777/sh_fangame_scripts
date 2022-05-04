using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerBattleState 
{
    WAITING_FOR_PLAYER_INPUT,
    STANDARD,
    MAGIC,
    ITEM,
    RING_DONE
}

public class PlayerBattle : MonoBehaviour
{

    public int hp, mp, sp, place_holder_dmg; // Health, Magic, Sanity
    public int p_atk, p_def, s_atk, s_def, agi;
    public float stock; // Stock Gauge
    public string name;
    public int MAX_HP, MAX_MP, MAX_SP;

    // Player's Standard Ring
    //public JudgmentRing jr;
    public GameObject jr_fab;
    public Transform jr_loc_init;
    public GameObject jr_go;
    public int strikes, hits, steps, modulates, charges, misses;
    public int ring_pieces;
    public bool ring_finished, mode_db, is_done, is_miss, is_dead, is_winner, is_tech;

    public AudioSource[] sounds;
    public Bag bag;
    public int ring_num;
    [SerializeField]
    Dial dial;
    public MagicType type;
    public Magic red_nova;
    public Magic rock_bump;
    public Magic hail_dust;
    public List<Magic> magic_list;
    public Equip charge_piece;
    public List<Equip> ring_equips;
    public PlayerBattleState state;
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
            this.p_atk = 0;
            this.p_def = 0;
            this.s_atk = 0;
            this.s_def = 0;
            this.agi = 10;
            this.name = "Raidou";
            this.ring_pieces = 3;
            this.strikes = 0;
            this.hits = 0;
            this.charges = 0;
            this.misses = 0;
            this.is_dead = false;
            this.is_miss = false;
            this.is_winner = false;
            this.is_tech = true;
            this.ring_num = 5;
            this.magic_list = new List<Magic>();
            red_nova = new RedNova();
            rock_bump = new RockBump();
            hail_dust = new HailDust();
            this.magic_list.Add(red_nova);
            this.magic_list.Add(rock_bump);
            this.magic_list.Add(hail_dust);
            this.type = MagicType.NONE;
            this.ring_equips = new List<Equip>();
            this.charge_piece = new ChargePiece();
            this.state = PlayerBattleState.WAITING_FOR_PLAYER_INPUT;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dial != null && this.jr_go != null)
        {
            switch (state)
            {
                case PlayerBattleState.WAITING_FOR_PLAYER_INPUT:
                    break;
                case PlayerBattleState.STANDARD:
                    this.hits = dial.hit_count;
                    this.strikes = dial.strike_count;
                    this.misses = dial.miss_count;
                    if (hits >= ring_pieces || strikes >= ring_pieces || hits + strikes >= ring_pieces || misses >= 1)
                    {
                        //StartCoroutine(RemoveDial());
                        Destroy(dial.gameObject);
                        state = PlayerBattleState.RING_DONE;
                        //DestroyImmediate(GameObject.FindGameObjectsWithTag("Ring")[0]);
                    }
                    break;
                case PlayerBattleState.ITEM:
                    this.hits = dial.hit_count;
                    this.strikes = dial.strike_count;
                    this.misses = dial.miss_count;
                    if (hits >= 1 || strikes >= 1 || hits + strikes >= 1 || misses >= 1)
                    {
                        Destroy(dial.gameObject);
                        state = PlayerBattleState.RING_DONE;
                    }
                    break;
                case PlayerBattleState.MAGIC:
                    if (this.ring_equips.Contains(charge_piece)) 
                    {
                        this.charges = dial.charge_count;
                        this.modulates = dial.modulate_count;
                        this.strikes = dial.strike_count;
                        this.misses = dial.miss_count;
                        if (charges >= 1 && modulates >= 1 || charges >= 1 && strikes >= 1 || misses >= 1)
                        {
                            //StartCoroutine(RemoveDial());
                            Destroy(dial.gameObject);
                            state = PlayerBattleState.RING_DONE;
                        }
                    }
                    this.steps = dial.step_count;
                    this.modulates = dial.modulate_count;
                    this.strikes = dial.strike_count;
                    this.misses = dial.miss_count;
                    if (steps >= 1 && modulates >= 1 || steps >= 1 && strikes >= 1 || misses >= 1)
                    {
                        //StartCoroutine(RemoveDial());
                        Destroy(dial.gameObject);
                        state = PlayerBattleState.RING_DONE;
                    }
                    break;
                case PlayerBattleState.RING_DONE:
                    break;
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
            this.steps = 0;
            this.modulates = 0;
            this.charges = 0;
            this.misses = 0;
            this.is_miss = false;
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
        this.state = PlayerBattleState.STANDARD;
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
        this.state= PlayerBattleState.ITEM;
        return jr_go;
    }

    // when spawning a ring to use an item we need to get the Magic's ring fab
    public GameObject spawn_magic_ring(GameObject ring)
    {
        Debug.Log("PB: Getting Ring Input For Magic Attack...");
        this.ring_finished = false;
        GameObject jr_go = Instantiate(ring, jr_loc_init);
        Debug.Log($"Ring Created");
        // Kinda f'd up way to get the dial when it is spawned in
        this.dial = GameObject.FindGameObjectsWithTag("Dial")[0].GetComponent<Dial>();
        this.state = PlayerBattleState.MAGIC;
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
            this.is_miss = true;
            return;
        }
        if (this.strikes >= 1)
        {
            item.UseItem(this, true);
        }
        else 
        {
            item.UseItem(this, false);
        }
    }

    public void HandleMagicAttack(Magic magic, List<EnemyBattle> enemies) 
    {
        if (this.misses >= 1)
        {
            Debug.Log($"{this.name} missed and wasn't able to cast {magic.name}!");
            this.is_miss = true;
            return;
        }
        if (this.strikes >= 1)
        {
            magic.UseMagic(this, enemies, true);
        }
        else 
        {
            magic.UseMagic(this, enemies, true);
        }
    }
}