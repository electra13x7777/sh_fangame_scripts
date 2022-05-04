using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HailDust : Magic
{
    public Bag bag;
    public override string name { get { return "Hail Dust"; } }
    public override string description { get { return string.Empty; } }
    public override int level { get { return 1; } }
    public override int ring_pieces { get { return 2; } }
    public override MagicType type { get { return MagicType.ICE; } }

    public override int base_damage { get { return 15; } }
    public override int cost { get { return 16; } }
    public int hits { get { return 2; } }
    
    public override bool is_weakness(EnemyBattle enemy)
    {
        if (enemy.type == MagicType.FIRE)
        {
            return true;
        }
        return false;
    }

    public override bool is_nullified(EnemyBattle enemy)
    {
        if (enemy.type == MagicType.ICE)
        {
            return true;
        }
        return false;
    }

    public override bool is_weakness_e(PlayerBattle player)
    {
        if (player.type == MagicType.FIRE)
        {
            return true;
        }
        return false;
    }

    public override bool is_nullified_e(PlayerBattle player)
    {
        if (player.type == MagicType.ICE)
        {
            return true;
        }
        return false;
    }

    // FINISH THIS
    public override void UseMagic(PlayerBattle player, List<EnemyBattle> enemies, bool is_strike)
    {
        if (is_nullified(enemies[0]))
        {
            Debug.Log($"MAGIC HAIL DUST: {player.name} casts {this.name} on {enemies[0].name} but it nullfies Ice!");
            return;
        }
        int damage_dealt = 0;
        // DO BASE DAMAGE MODIFACTION FROM PLAYER STATS HERE!!!

        if (enemies.Count == 1)
        {
            Debug.Log($"MAGIC HAIL DUST: {player.name} casts {this.name} on {enemies[0].name}!");
            if (is_strike)
            {
                if (is_weakness(enemies[0]))
                {
                    //damage_dealt = ((base_damage / 2) + (base_damage + (base_damage / 2)));
                    for (int i = 0; i < hits; i++)
                    {
                        damage_dealt += base_damage + (base_damage / 2) + (base_damage / 4);
                        enemies[0].hp -= base_damage + (base_damage / 2) + (base_damage / 4);
                        Debug.Log($"Combo: {i + 1} Hits for {base_damage + (base_damage / 2) + (base_damage / 4)}HP damage!");
                    }
                }
                else
                {
                    for (int i = 0; i < hits; i++)
                    {
                        damage_dealt += base_damage + (base_damage / 2);
                        enemies[0].hp -= base_damage + (base_damage / 2);
                        Debug.Log($"Combo: {i + 1} Hits for {base_damage + (base_damage / 3)}HP damage!");
                    }
                }
            }
            else
            {
                if (is_weakness(enemies[0]))
                {
                    for (int i = 0; i < hits; i++)
                    {
                        damage_dealt += base_damage + (base_damage / 2);
                        enemies[0].hp -= base_damage + (base_damage / 2);
                        Debug.Log($"Combo: {i + 1} Hits for {base_damage + (base_damage / 2)}HP damage!");
                    }
                }
                else
                {
                    for (int i = 0; i < hits; i++)
                    {
                        damage_dealt += base_damage;
                        enemies[0].hp -= base_damage;
                        Debug.Log($"Combo: {i + 1} Hits for {base_damage}HP damage!");
                    }
                }
            }
            // APPLY ENEMY STATS HERE
            //enemies[0].s_def

            Debug.Log($"MAGIC HAIL DUST: {player.name} dealt a total {damage_dealt}HP of damage to {enemies[0].name}!");
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
        return;
    }
}
