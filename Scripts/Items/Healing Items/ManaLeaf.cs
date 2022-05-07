using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaLeaf : Item
{
    public override string name { get { return "Mana Leaf"; } }
    public override string description { get { return "A spiritual plant that restores lost magic power. Grown in secret by the descendants of witches, the exact method to cultivate it remains a mystery."; } }


    //public override AudioSource sound;
    //public GameObject jr_go;
    public Bag bag;
    public override GameObject jr_fab { get { return new GameObject(); } }//bag.rings_list[0]; } }
    public override int ring_pieces { get { return 1; } }
    public AudioSource sound;

    public int base_heal_amt;

    public void heal_mp(PlayerBattle player, bool is_strike)
    {
        this.base_heal_amt = 30;
        if (is_strike)
            this.base_heal_amt += this.base_heal_amt / 4;

        if (player.mp + base_heal_amt >= player.MAX_MP)
        {
            player.mp += (player.MAX_MP - player.mp);
            //Debug.Log($"ITEM MEDILEAF: {player.name} heals for {player.MAX_HP - player.hp} Hit Points!");
            Debug.Log($"ITEM MANALEAF: {player.name} heals for {base_heal_amt} Magic Points!");
        }
        else
        {
            player.mp += base_heal_amt;
            Debug.Log($"ITEM MANALEAF: {player.name} heals for {base_heal_amt} Magic Points!");
        }
    }          // currently just heals
    public override void UseItem(PlayerBattle player, bool is_strike)
    {
        //this.sound.Play();
        heal_mp(player, is_strike);
        //heal()
        Bag.RemoveFromBag(this);
        return;
        //this.jr_go = player.spawn_item_ring()
    }
}
