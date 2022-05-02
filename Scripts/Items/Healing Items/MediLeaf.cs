using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediLeaf : Item
{
    
    public override string name { get { return "Medi Leaf"; } }
    public override string description { get { return "A wild herb from the highlands of both Europe and Asia. Has remarkable regenerative and healing properties and almost no taste, making it perfect even for picky children."; } }


    //public override AudioSource sound;
    //public GameObject jr_go;
    public Bag bag;
    public override GameObject jr_fab { get { return new GameObject(); } }//bag.rings_list[0]; } }
    public override int ring_pieces { get { return 1; } }
    public AudioSource sound;

    public int base_heal_amt;

    public MediLeaf() 
    {
        //this.jr
        this.base_heal_amt = 25;
        //this.jr_fab = bag.rings_list[0].gameObject;

    }

    public void heal_hp(PlayerBattle player, bool is_strike) 
    {
        this.base_heal_amt = 25;
        if (is_strike)
            this.base_heal_amt += this.base_heal_amt/4;

        if (player.hp + base_heal_amt >= player.MAX_HP)
        {
            player.hp += (player.MAX_HP - player.hp);
            //Debug.Log($"ITEM MEDILEAF: {player.name} heals for {player.MAX_HP - player.hp} Hit Points!");
            Debug.Log($"ITEM MEDILEAF: {player.name} heals for {base_heal_amt} Hit Points!");
        }
        else 
        {
            player.hp += base_heal_amt;
            Debug.Log($"ITEM MEDILEAF: {player.name} heals for {base_heal_amt} Hit Points!");
        }
    }
    // currently just heals
    public override void UseItem(PlayerBattle player, bool is_strike)
    {
        heal_hp(player, is_strike);
        return;
    }
}
