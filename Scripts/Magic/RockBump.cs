using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RockBump : Magic
{
    public Bag bag;
    public override string name { get { return "Rock Bump"; } }
    public override string description { get { return string.Empty; } }
    public override int level { get { return 1; } }
    public override int ring_pieces { get { return 2; } }
    public override MagicType type { get { return MagicType.EARTH; } }

    public override int base_damage { get { return 4; } }
    public override int cost { get { return 16; } }
    public override int ring_index { get { return 7; } }
    public int hits { get { return 5; } }


    public override bool is_weakness(EnemyBattle enemy)
    {
        if (enemy.type == MagicType.WIND)
        {
            return true;
        }
        return false;
    }

    public override bool is_nullified(EnemyBattle enemy)
    {
        if (enemy.type == MagicType.EARTH)
        {
            return true;
        }
        return false;
    }

    public override bool is_weakness_e(PlayerBattle player)
    {
        if (player.type == MagicType.WIND)
        {
            return true;
        }
        return false;
    }

    public override bool is_nullified_e(PlayerBattle player)
    {
        if (player.type == MagicType.EARTH)
        {
            return true;
        }
        return false;
    }

    public override void UseMagic(PlayerBattle player, List<EnemyBattle> enemies, bool is_strike)
    {
        if (is_nullified(enemies[0]))
        {
            Debug.Log($"MAGIC ROCK BUMP: {player.name} casts {this.name} on {enemies[0].name} but it nullfies Earth!");
            return;
        }
        int damage_dealt = 0;
        float temp = 0.0f;
        // DO BASE DAMAGE MODIFACTION FROM PLAYER STATS HERE!!!

        if (enemies.Count == 1)
        {
            Debug.Log($"MAGIC ROCK BUMP: {player.name} casts {this.name} on {enemies[0].name}!");
            if (is_strike)
            {
                if (is_weakness(enemies[0]))
                {
                    //damage_dealt = ((base_damage / 2) + (base_damage + (base_damage / 2)));
                    temp = (float)(this.base_damage) * ((player.is_tech) ? player.tech_ring_strike_bonus : player.normal_ring_strike_bonus) * player.weakness_bonus;
                    damage_dealt = Convert.ToInt32(temp * player.s_atk_buff);
                }
                else
                {
                    temp = (float)(this.base_damage) * ((player.is_tech) ? player.tech_ring_strike_bonus : player.normal_ring_strike_bonus);
                    damage_dealt = Convert.ToInt32(temp * player.s_atk_buff);
                }
            }
            else
            {
                if (is_weakness(enemies[0]))
                {
                    temp = (float)(this.base_damage) * player.weakness_bonus;
                    damage_dealt = Convert.ToInt32(temp * player.s_atk_buff);
                }
                else
                {
                    temp = (float)(this.base_damage);
                    damage_dealt = Convert.ToInt32(temp * player.s_atk_buff);
                    //damage_dealt += this.base_damage;
                }
            }

            if (player.ring_equips.Contains(player.charge_piece))
            {
                float multiplier = (player.is_tech) ? 0.8f : 0.9f;
                multiplier += (player.is_tech) ? (float)(player.charges) * 0.02f : (float)(player.charges) * 0.01f;
                temp = (float)(damage_dealt) * multiplier;
                Debug.Log($"Charge Multiplier is {multiplier}");
                damage_dealt = Convert.ToInt32(temp * player.s_atk_buff);
            }

            // APPLY ENEMY STATS HERE
            //enemies[0].s_def

            // DO THE ATTACK
            for (int i = 0; i < hits; i++)
            {
                enemies[0].hp -= damage_dealt;
                Debug.Log($"Combo: {i + 1} Hits for {damage_dealt}HP damage!");
            }

            Debug.Log($"MAGIC ROCK BUMP: {player.name} dealt a total {damage_dealt*hits}HP of damage to {enemies[0].name}!");
            //enemies[0].hp -= damage_dealt;
            if (player.mp - this.cost <= 0)
            {
                player.mp = 0;
            }
            else
            {
                player.mp -= this.cost;
            }
        }
        else
        {
            foreach (EnemyBattle enemy in enemies)
            {
                if (is_strike)
                {
                    return;
                }
            }
        }
    }
    public override void EnemyMagic(EnemyBattle enemy, List<PlayerBattle> players)
    {
        //this.base_damage -= 1;
        if (players.Count == 1)
        {
            if (is_nullified_e(players[0]))
            {
                Debug.Log($"MAGIC ROCK BUMP: {enemy.name} casts {this.name} on {players[0].name} but it nullfies Earth!");
                return;
            }
            int damage_dealt = 0;
            if (is_weakness_e(players[0]))
            {
                for (int i = 0; i < hits; i++)
                {
                    damage_dealt += base_damage + (base_damage / 2);
                    players[0].hp -= base_damage + (base_damage / 2);
                    Debug.Log($"Combo: {i + 1} Hits for {base_damage + (base_damage / 2)}HP damage!");
                }
            }
            else
            {
                for (int i = 0; i < hits; i++)
                {
                    damage_dealt += base_damage;
                    players[0].hp -= base_damage;
                    Debug.Log($"Combo: {i + 1} Hits for {base_damage}HP damage!");
                }
            }
        }
        else
        {
            return;
        }
    }
}