using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PureLeaf : Item
{
    public override string name { get { return "Pure Leaf"; } }
    public override string description { get { return "Grown on the South American plains, this relaxant relieves deep down stress when eaten. At the present time no adverse side effects have been reported."; } }


    //public override AudioSource sound;
    //public GameObject jr_go;
    public Bag bag;
    // TODO: PLEASE REMOVE THIS FROM ALL ITEMS
    public override GameObject jr_fab { get { return new GameObject(); } }//bag.rings_list[0]; } }
    public override int ring_pieces { get { return 1; } }
    public AudioSource sound;

    public int base_heal_amt;

    public void heal_sp(PlayerBattle player, bool is_strike)
    {
        this.base_heal_amt = 3;
        if (is_strike)
            this.base_heal_amt += this.base_heal_amt / 3;

        if (player.sp + base_heal_amt >= player.MAX_SP)
        {
            player.sp += (player.MAX_SP - player.sp);
            //Debug.Log($"ITEM MEDILEAF: {player.name} heals for {player.MAX_HP - player.hp} Hit Points!");
            Debug.Log($"ITEM PURELEAF: {player.name} heals for {base_heal_amt} Sanity Points!");
        }
        else
        {
            player.sp += base_heal_amt;
            Debug.Log($"ITEM PURELEAF: {player.name} heals for {base_heal_amt} Sanity Points!");
        }
    }          // currently just heals
    public override void UseItem(PlayerBattle player, bool is_strike)
    {
        //this.sound.Play();
        //heal()
        heal_sp(player, is_strike);
        Bag.RemoveFromBag(this);
        return;
        //this.jr_go = player.spawn_item_ring()
    }
}
