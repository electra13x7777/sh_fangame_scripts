using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedNova : Magic
{
    public Bag bag;
    public override string name { get { return "Red Nova"; } }
    public override string description { get { return string.Empty; } }
    public override int level { get { return 1; } }
    public override int ring_pieces { get { return 2; } }
    public override MagicType type { get { return MagicType.FIRE; } }

    public override int base_damage { get { return 30; } }
    public override int cost { get { return 25; } }

    public override bool is_weakness(EnemyBattle enemy) 
    {
        if (enemy.type == MagicType.ICE) 
        {
            return true;
        }
        return false;
    }

    public override bool is_nullified(EnemyBattle enemy)
    {
        if (enemy.type == MagicType.FIRE)
        {
            return true;
        }
        return false;
    }

    public override bool is_weakness_e(PlayerBattle player)
    {
        if (player.type == MagicType.ICE)
        {
            return true;
        }
        return false;
    }

    public override bool is_nullified_e(PlayerBattle player)
    {
        if (player.type == MagicType.FIRE)
        {
            return true;
        }
        return false;
    }

    // THINGS TO NOTE
    // EVENTUALLY WE WILL NEED TO INCOPERATE FORMATIONS ON THE GAME FIELD WITH
    // POSITION! KEEP THIS IN MIND

    // ALSO WILL PUSH BACK

    // Override base class member function
    public override void UseMagic(PlayerBattle player, List<EnemyBattle> enemies, bool is_strike) 
    {
        int damage_dealt = 0;
        // DO BASE DAMAGE MODIFACTION FROM PLAYER STATS HERE!!!
        if (enemies.Count == 1)
        {
            if (is_nullified(enemies[0])) 
            {
                Debug.Log($"MAGIC RED NOVA: {player.name} casts {this.name} on {enemies[0].name} but it nullfies Fire!");
                return;
            }

            if (is_strike) 
            {
                if (is_weakness(enemies[0]))
                {
                    damage_dealt = ((base_damage / 2) + (base_damage + (base_damage / 2)));
                }
                else
                {
                    damage_dealt = ((base_damage / 2) + base_damage);
                }
            }
            else
            {
                if (is_weakness(enemies[0]))
                {
                    damage_dealt = ((base_damage / 4) + (base_damage + (base_damage / 2)));
                }
                else 
                {
                    damage_dealt = ((base_damage / 4) + base_damage); 
                }
            }
            if (player.ring_equips.Contains(player.charge_piece)) 
            {
                float multiplier = 0.8f;
                multiplier += (float)(player.charges) * 0.02f;
                float temp = (float)(damage_dealt) * multiplier;
                Debug.Log($"Charge Multiplier is {multiplier}");
                damage_dealt = (int)(temp);
            }
            // APPLY ENEMY STATS HERE
            //enemies[0].s_def

            Debug.Log($"MAGIC RED NOVA: {player.name} casts {this.name} on {enemies[0].name} and deals {damage_dealt}HP damage!");
            enemies[0].hp -= damage_dealt;
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