using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Base Class for Magic Skills
//
//


public abstract class Magic
{
    public Bag bag;
    public virtual string name { get; set; }
    public virtual string description { get; set; }
    public virtual int level { get; set; } // Tiering for magic
    public virtual int ring_pieces { get; set; }
    public virtual MagicType type { get; set; }
    public virtual int base_damage { get; set; }
    public virtual int cost { get; set; }
    public virtual int ring_index { get; set; }
    // Maybe virtual animation reference here later???

    public abstract bool is_weakness(EnemyBattle enemy);
    public abstract bool is_nullified(EnemyBattle enemy);
    public abstract bool is_weakness_e(PlayerBattle player);
    public abstract bool is_nullified_e(PlayerBattle player);
    public abstract void UseMagic(PlayerBattle player, List<EnemyBattle> enemies, bool is_strike);
    public abstract void EnemyMagic(EnemyBattle enemy, List<PlayerBattle> players);

    // Prototype For Enemy Magic HERE
}
