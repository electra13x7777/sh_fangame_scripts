using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surge : Magic
{
    public Bag bag;
    public override string name { get { return "Surge"; } }
    public override string description { get { return string.Empty; } }
    public override int level { get { return 1; } }
    public override int ring_pieces { get { return 1; } }
    public override MagicType type { get { return MagicType.BUFF; } }

    public override int base_damage { get { return 0; } } // will not be used
    public override int cost { get { return 12; } }
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

    public override void UseMagic(PlayerBattle player, List<EnemyBattle> enemies, bool is_strike)
    {
        //float temp = 0.0f;
        if (is_strike)
        {
            // Avoid overwriting player buffs
            if (player.s_atk_buff != 1.0f)
            {
                player.s_atk_buff += (player.is_tech) ? 1.46f : 1.36f;
            }
            else
            {
                player.s_atk_buff = (player.is_tech) ? 1.46f : 1.36f;
            }
            Debug.Log($"MAGIC SURGE: {player.name} increases their S.ATK by {((player.is_tech) ? 1.46f : 1.36f) * 100}%!");
        }
        else
        {
            if (player.s_atk_buff != 1.0f)
            {
                player.s_atk_buff += 1.30f;
            }
            else
            {
                player.s_atk_buff = 1.30f;
            }
            Debug.Log($"MAGIC SURGE: {player.name} increases their S.ATK by 130%!");
        }
        if (!player.has_buffs) { player.has_buffs = true; }
        player.s_atk_buff_count = 6; // Will instantly decrement to 5

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