using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePiece : Equip
{
    public Bag bag;
    public override string name { get { return "Charge Piece"; } }
    public override string description { get { return "Changes Ring Step Areas to Charge Areas"; } }
    // TODO: PLEASE REMOVE THIS FROM ALL ITEMS
    public override GameObject jr_fab { get { return new GameObject(); } }
    public override int ring_pieces { get { return 1; } }

    public override EquipType equip_type { get { return EquipType.RING_EQUIP; } }

    public override void UseItem(PlayerBattle player, bool is_strike)
    {
        return;
    }
}
