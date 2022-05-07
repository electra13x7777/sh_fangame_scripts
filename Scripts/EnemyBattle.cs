using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : MonoBehaviour
{
    public Bag bag;
    public int hp, mp, agi, exp_rewarded, place_holder_dmg;
    public int p_atk, p_def, s_atk, s_def;
    public string name;
    public string element;
    public bool is_dead;
    bool mode_db;
    public MagicType type;
    public Magic rock_bump;
    public Magic hail_dust;
    public List<Magic> magic_list;

    // Prototypes for when 3D position logic is ready
    //public Transform transform;


    public EnemyBattle() 
    {

        //this.hp = 22;
        //this.mp = 65;
        //this.place_holder_dmg = 4;
        this.p_atk = 0;
        this.p_def = 0;
        this.s_atk = 0;
        this.s_def = 0;
        this.agi = 8;
        //rock_bump = new RockBump();
        //this.magic_list.Add(rock_bump);
        //this.type = MagicType.ICE;
        //this.name = "Violent Thug";
    }

    // Start is called before the first frame update
    void Start()
    {
        is_dead = false;
        mode_db = true;
        if (mode_db)
        {
            this.hp = 122;
            this.mp = 65;
            this.place_holder_dmg = 8;
            this.agi = 11;
            this.magic_list = new List<Magic>();
            rock_bump = new RockBump();
            hail_dust = new HailDust();
            this.magic_list.Add(rock_bump);
            this.magic_list.Add(hail_dust);
            this.type = MagicType.ICE;
            this.name = "Violent Thug";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.hp <= 0) 
        {
            is_dead = true;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Attack(PlayerBattle player) 
    {
        player.hp -= this.place_holder_dmg;
        Debug.Log($"{this.name} deals {this.place_holder_dmg} damage to {player.name}!");
    }

    public void delayed_attack_pattern(List<PlayerBattle> players) 
    {
        if (this.hp >= 80)
        {
            Attack(players[0]);
        }
        else if (this.hp < 80 && this.hp >= 60)
        {
            //Random rand;
            int atk = Random.Range(0, 2);
            //p_atk = 1;
            switch (atk)
            {
                case 0:
                    Attack(players[0]);
                    break;
                case 1:
                    this.magic_list[0].EnemyMagic(this, players);
                    break;
            }
        }
        else if(this.hp <60)
        {
            this.magic_list[0].EnemyMagic(this, players);
        }
    }
}
