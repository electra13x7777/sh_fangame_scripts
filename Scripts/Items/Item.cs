using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Item.cs - Base for items
//

public abstract class Item
{
    // virtual members
    public Bag bag;
    public virtual string name { get; set; }
    public virtual string description { get; set; }
    public virtual GameObject jr_fab { get; set; }
    public virtual int ring_pieces { get; set; }
    public virtual ItemType type { get; set; }

    // member function prototypes
    public abstract void UseItem(PlayerBattle player, bool is_strike);
}
