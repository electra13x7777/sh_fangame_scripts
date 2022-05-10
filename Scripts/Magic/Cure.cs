using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cure : Magic
{
    public Bag bag;
    public override string name { get { return "Cure"; } }
    public override string description { get { return "Restores a small amount of HP to one ally."; } }
    public override int level { get { return 1; } }
    public override int ring_pieces { get { return 1; } }
    public override MagicType type { get { return MagicType.HEALING; } }
    public override int ring_index { get { return 9; } }

    public override int base_damage { get { return 30; } } // our heal ammount in this instance
    public override int cost { get { return 6; } }

    public override bool is_weakness(EnemyBattle enemy)
    {
        return false;
    }

    public override bool is_nullified(EnemyBattle enemy)
    {
        return false;
    }

    public override bool is_weakness_e(PlayerBattle player)
    {
        return false;
    }

    public override bool is_nullified_e(PlayerBattle player)
    {
        return false;
    }

    public override void UseMagic(PlayerBattle player, List<EnemyBattle> enemies, bool is_strike) 
    {
        float temp = 0.0f;
        if (is_strike)
        {
            temp = (float)(this.base_damage) * ((player.is_tech) ? player.tech_ring_strike_bonus : player.normal_ring_strike_bonus);
            base_damage = Convert.ToInt32(temp);
        }
        
        //this.base_damage += this.base_damage/4;

        if (player.hp + base_damage >= player.MAX_HP)
        {
            player.hp += (player.MAX_HP - player.hp);
            //Debug.Log($"ITEM MEDILEAF: {player.name} heals for {player.MAX_HP - player.hp} Hit Points!");
            Debug.Log($"MAGIC CURE: {player.name} heals for {base_damage} Hit Points!");
        }
        else
        {
            player.hp += base_damage;
            Debug.Log($"MAGIC CURE: {player.name} heals for {base_damage} Hit Points!");
        }
        if (player.mp - this.cost <= 0)
        {
            player.mp = 0;
        }
        else
        {
            player.mp -= this.cost;
        }
    }

    public override void EnemyMagic(EnemyBattle enemy, List<PlayerBattle> players)
    {
        return;
    }
}
