using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnergyCharge : Magic
{
    public Bag bag;
    public override string name { get { return "Energy Charge"; } }
    public override string description { get { return string.Empty; } }
    public override int level { get { return 1; } }
    public override int ring_pieces { get { return 1; } }
    public override MagicType type { get { return MagicType.BUFF; } }

    public override int base_damage { get { return 0; } } // will not be used
    public override int cost { get { return 0; } }
    public override int ring_index { get { return 7; } }

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

    public void GetCost(PlayerBattle player) 
    {
        float temp = (float)(player.MAX_MP) * 0.25f;
        this.cost = Convert.ToInt32(temp);
    }

    public override void UseMagic(PlayerBattle player, List<EnemyBattle> enemies, bool is_strike)
    {
        this.GetCost(player);
        if (is_strike)
        {
            if (player.p_atk_buff != 1.0f)
            {
                player.p_atk_buff += (player.is_tech) ? 1.80f : 1.50f;
            }
            else 
            {
                player.p_atk_buff = (player.is_tech) ? 1.80f : 1.50f;
            }
            Debug.Log($"MAGIC ENERGY CHARGE: {player.name} increases their P.ATK by {((player.is_tech) ? 1.80f : 1.50f) * 100}%!");
        }
        else
        {
            if (player.p_atk_buff != 1.0f)
            {
                player.p_atk_buff += (player.is_tech) ? 1.80f : 1.50f;
            }
            else
            {
                player.p_atk_buff = (player.is_tech) ? 1.80f : 1.50f;
            }
            player.p_atk_buff = 1.20f;
            Debug.Log($"MAGIC ENERGY CHARGE: {player.name} increases their P.ATK by {((player.is_tech) ? 1.80f : 1.50f) * 100}%!");
        }

        if (!player.has_buffs) { player.has_buffs = true; }
        player.energy_charge_count = 2; // Will instantly decrement to 1

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