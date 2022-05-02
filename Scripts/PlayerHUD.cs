using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Text p_name;
    public Text p_hp;
    public Text p_mp;
    public Text p_sp;

    public void set_hud(PlayerBattle p)
    {
        p_name.text = $"{p.name}";
        p_hp.text = $"{p.hp}/{p.MAX_HP}";
        p_mp.text = $"{p.mp}";
        p_sp.text = $"{p.sp}";
    }

    public void set_hp(int hp) {p_hp.text = $"{hp}"; }
    public void set_mp(int mp) {p_mp.text = $"{mp}"; }
    public void set_sp(int sp) {p_sp.text = $"{sp}"; }
}
