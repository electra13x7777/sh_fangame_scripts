using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : MonoBehaviour
{
    public int hp, mp, agi, exp_rewarded, place_holder_dmg;
    public string name;
    public string element;
    public bool is_dead;
    bool mode_db;

    public EnemyBattle() 
    {
        this.hp = 22;
        this.mp = 65;
        this.place_holder_dmg = 4;
        this.agi = 8;
        this.name = "Violent Thug";
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
    // TODO: ADD MOVES
    // RED FLARE


    // TODO: ADD Basic AI
    // RED FLARE
}
