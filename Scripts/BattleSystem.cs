using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    START,
    PLAYER_TURN,
    ENEMY_TURN,
    RING_FINISHED,
    WIN,
    LOSE,
    DONE
}

public enum PlayerState 
{
    START,
    STANDARD,
    STANDARD_ATTACK_DONE,
    ITEM,
    HEALING_ITEM,
    HEALING_ITEM_DONE,
    MAGIC_ATTACK,
    MAGIC_ATTACK_DONE,
    END_TURN
}

public class BattleSystem : MonoBehaviour
{
    // take in the prefabs for the actors
    public GameObject p_fab;
    public GameObject e_fab;
    public GameObject jr_fab;
    public GameObject cfb_fab;
    // get the positions to spawn them in at

    public Transform p_loc_init;
    public Transform e_loc_init;
    public Transform jr_loc_init;
    public Transform cfb_loc_init;

    // jr game object
    public GameObject jr_go;

    public bool manual_ring_del, auto_ring_del;
    
    public Text p_name;
    public Text p_hp;
    public Text p_mp;
    public Text p_sp;
    
    //[RequireComponent(typeof(PlayerHUD))]
    public PlayerHUD p_hud;
    public AudioSource[] sounds;

    public Bag bag;
    public PlayerBattle player;
    EnemyBattle enemy;
    
    //public JudgmentRing judgment_ring;

    public BattleState state;
    public PlayerState p_state;
    public List<EnemyBattle> enemies;
    public List<PlayerBattle> players;
    public int item_num, mag_num;
    
    // SINGLETON
    
    private static BattleSystem Instance;
    public static BattleSystem GetInstance
    {
        get; private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //not using rn
        state = BattleState.START;
        if (state == BattleState.START)
        {

            //StartCoroutine(prepare_battle());
        }
        this.item_num = 0;
        this.mag_num = 0;
        this.jr_go = null;
        //prepare_battle();
        sounds[0].Play();
        player = spawn_player();
        enemy = spawn_enemy();
        enemies.Add(enemy);
        this.players = new List<PlayerBattle>();
        players.Add(player);
        if (player.agi >= enemy.agi)
        {
            state = BattleState.PLAYER_TURN;
            Debug.Log($"{player.name}'s Turn");
            Debug.Log($"{player.name} {player.hp}HP/{player.MAX_HP}HP {player.mp}MP/{player.MAX_MP}MP {player.sp}SP/{player.MAX_SP}SP");
        }
        else 
        {
            state = BattleState.ENEMY_TURN;
        }
    }

    void Update()
    {
        //bool standard_done = false;
        //bool healing = false;
        //bool healed = false;
        switch (state)
        {
            case BattleState.PLAYER_TURN:
                p_name.text = $"{player.name}";
                p_hp.text = $"{player.hp}HP/{player.MAX_HP}HP";
                p_mp.text = $"{player.mp}MP/{player.MAX_MP}MP";
                p_sp.text = $"{player.sp}SP/{player.MAX_SP}SP";
                // TODO: HANDLE BERSERK HERE WHEN SP REACHES ZERO


                // HANDLE PLAYER INPUT HERE
                // Here we only want to take in the player input then redirect them to a ring or possibly something else
                if (Input.GetKeyDown(KeyCode.A) && this.jr_go == null)
                {
                    this.jr_go = player.spawn_standard_ring(bag.rings_list[player.ring_num]);
                    p_state = PlayerState.STANDARD;
                }

                if (Input.GetKeyDown(KeyCode.C) && this.jr_go == null) 
                {
                    if (player.mp >= player.magic_list[mag_num].cost)
                    {
                        // HANDLE HEALING MAGIC
                        if (player.magic_list[mag_num].type == MagicType.HEALING)
                        {
                            this.jr_go = player.spawn_magic_ring(bag.rings_list[player.magic_list[mag_num].ring_index]);
                        }
                        // HANDLE BUFF MAGIC
                        else if (player.magic_list[mag_num].type == MagicType.BUFF) 
                        {
                            this.jr_go = player.spawn_magic_ring(bag.rings_list[player.magic_list[mag_num].ring_index]);
                        }
                        // HANDLE CHARGE RING EQUIP
                        else if (player.ring_equips.Contains(player.charge_piece))
                        {
                            this.jr_go = player.spawn_magic_ring(bag.rings_list[player.magic_list[mag_num].ring_index + 1]);
                        }
                        // HANDLE NON CHARGE MAGIC ATTACK
                        else
                        {
                            this.jr_go = player.spawn_magic_ring(bag.rings_list[player.magic_list[mag_num].ring_index]);
                        }
                        p_state = PlayerState.MAGIC_ATTACK;
                    }
                    else 
                    {
                        Debug.Log("Not Enough MP");
                    }
                }

                if (Input.GetKeyDown(KeyCode.D) && this.jr_go == null && bag.items.ContainsKey(bag.item_list[item_num]))
                {
                    bag.item_list[item_num].jr_fab = this.jr_fab;
                    foreach(Item i in bag.items.Keys)
                        Debug.Log($"bag.Keys: {i.name}");
                    foreach (Item i in bag.item_list)
                        Debug.Log($"bag.item_list: {i.name}");

                    Debug.Log($"{bag.item_list[item_num].name}");
                    //Debug.Log($"{bag.item_list[item_num].jr_fab}");
                    this.jr_go = player.spawn_item_ring(bag.rings_list[9]);
                    //healing = true;
                    //p_state = PlayerState.ITEM;
                    p_state = PlayerState.HEALING_ITEM;
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    bag.sounds[3].Play();
                    if (item_num + 1 == 3)
                    {
                        item_num = 0;
                        Debug.Log($"ITEM INDEX: {item_num}");
                        try
                        {
                            Debug.Log($"Current Item: {bag.item_list[item_num].name}");
                            Debug.Log($"Amt: {bag.items[bag.item_list[item_num]]}");
                        }
                        catch (KeyNotFoundException e)
                        {
                            
                        }
                    }
                    else
                    {
                        Debug.Log($"ITEM INDEX: {++item_num}");
                        try
                        {
                            Debug.Log($"Current Item: {bag.item_list[item_num].name}");
                            Debug.Log($"Amt: {bag.items[bag.item_list[item_num]]}");
                        }
                        catch (KeyNotFoundException e) 
                        {
                            
                        }
                    }
                    
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    bag.sounds[3].Play();
                    if (mag_num + 1 == player.magic_list.Count)
                    {
                        mag_num = 0;
                        Debug.Log($"ITEM INDEX: {mag_num}");
                        Debug.Log($"Current Magic Attack: {player.magic_list[mag_num]} | Cost: {player.magic_list[mag_num].cost}MP");
                    }
                    else
                    {

                        Debug.Log($"ITEM INDEX: {++mag_num}");
                        Debug.Log($"Current Magic Attack: {player.magic_list[mag_num]} | Cost: {player.magic_list[mag_num].cost}MP");
                    }

                }

                if (Input.GetKeyDown(KeyCode.V)) 
                {
                    bag.sounds[3].Play();
                    if (!player.ring_equips.Contains(player.charge_piece))
                    {
                        player.ring_equips.Add(player.charge_piece);
                        Debug.Log($"BATTLE SYSTEM: {player.name} equips a {player.charge_piece.name} to their Judgement Ring");
                    }
                    else 
                    {
                        player.ring_equips.Remove(player.charge_piece);
                        Debug.Log($"BATTLE SYSTEM: {player.name} unequips their {player.charge_piece.name} from their Judgement Ring");
                    }
                }

                // HANDLE STATE OF PLAYER TURN HERE
                switch (p_state) 
                {
                    case PlayerState.STANDARD:
                        if (player.hits >= player.ring_pieces || player.strikes >= player.ring_pieces || player.hits + player.strikes >= player.ring_pieces || player.misses >= 1) 
                        {
                            p_state = PlayerState.STANDARD_ATTACK_DONE;
                        }
                        break;
                    case PlayerState.HEALING_ITEM:
                        if (player.hits >= 1 || player.strikes >= 1 || player.misses >= 1) 
                        {
                            p_state = PlayerState.HEALING_ITEM_DONE;
                        }
                        break;
                    case PlayerState.MAGIC_ATTACK://player.magic_list[0].ring_pieces
                        if (player.magic_list[mag_num].type == MagicType.BUFF)
                        {
                            if (player.steps >= 1 && player.modulates >= 1 || player.steps >= 1 && player.strikes >= 1 || player.misses >= 1)
                            {
                                p_state = PlayerState.MAGIC_ATTACK_DONE;
                            }
                        }
                        if (player.magic_list[mag_num].type == MagicType.HEALING) 
                        {
                            if (player.hits >= 1 || player.strikes >= 1 || player.misses >= 1)
                            {
                                p_state = PlayerState.MAGIC_ATTACK_DONE;
                            }
                        }
                        if (player.ring_equips.Contains(player.charge_piece)) 
                        {
                            if (player.charges >= 1 && player.modulates >= 1 || player.charges >= 1 && player.strikes >= 1 || player.misses >= 1) 
                            {
                                p_state = PlayerState.MAGIC_ATTACK_DONE;
                            }
                        }
                        if (player.steps >= 1 && player.modulates >= 1 || player.steps >= 1 && player.strikes >= 1 || player.misses >= 1)
                        {
                            p_state = PlayerState.MAGIC_ATTACK_DONE;
                        }
                        break;
                    case PlayerState.STANDARD_ATTACK_DONE:
                        player.StandardAttack(enemy);
                        player.sp -= 1;
                        player.hits = 0;
                        player.strikes = 0;
                        player.misses = 0;
                        StartCoroutine(RemoveRing());
                        p_state = PlayerState.END_TURN;
                        break;
                    case PlayerState.HEALING_ITEM_DONE:
                        player.HandleHealingItem(bag.item_list[item_num]);
                        player.sp -= 1;
                        player.hits = 0;
                        player.strikes = 0;
                        player.misses = 0;
                        StartCoroutine(RemoveRing());
                        if (!player.is_miss)
                        {
                            bag.sounds[item_num].Play();
                        }
                        player.is_miss = false;
                        bag.sounds[item_num].Play();
                        p_state = PlayerState.END_TURN;
                        break;
                    case PlayerState.MAGIC_ATTACK_DONE:
                        player.HandleMagicAttack(player.magic_list[mag_num], enemies);//player.magic_list[0], enemies);
                        player.sp -= 1;
                        player.hits = 0;
                        player.strikes = 0;
                        player.steps = 0;
                        player.modulates = 0;
                        player.misses = 0;
                        StartCoroutine(RemoveRing());
                        if (!player.is_miss)
                        {
                            bag.sounds[mag_num + 4].Play(); // currently dumb bullshit
                        }
                        player.is_miss = false;
                        p_state = PlayerState.END_TURN;
                        break;
                    case PlayerState.END_TURN:
                        if (enemy.hp <= 0)
                        {
                            Debug.Log("PLAYER WINS");
                            state = BattleState.WIN;
                            Destroy(enemy);
                        }
                        else
                        {
                            Debug.Log($"{enemy.name} {enemy.hp}HP");
                            Debug.Log($"{enemy.name}'s Turn");
                            state = BattleState.ENEMY_TURN;
                        }
                        break;
                }
                break; // End PLAYER_TURN
            case BattleState.RING_FINISHED:

                break;
            case BattleState.ENEMY_TURN:
                enemy.delayed_attack_pattern(players);
                if (player.hp <= 0) 
                {
                    Debug.Log("PLAYER LOSES");
                    state = BattleState.LOSE;
                    break;
                }
                if (!player.is_dead)
                {
                    Debug.Log($"{player.name}'s Turn");
                    Debug.Log($"{player.name} {player.hp}HP/{player.MAX_HP}HP {player.mp}MP/{player.MAX_MP}MP {player.sp}SP/{player.MAX_SP}SP");
                    state = BattleState.PLAYER_TURN;
                    p_state = PlayerState.START;
                }
                else if (player.is_dead)
                    state = BattleState.LOSE;
                break;
            case BattleState.WIN:
                sounds[0].Stop();
                sounds[1].Play();
                player.is_winner = true;
                //if(enemy.gameObject != null)
                    //Destroy(enemy.gameObject);
                state = BattleState.DONE;
                break;
            case BattleState.LOSE:
                p_hp.text = $"0HP/{player.MAX_HP}HP";
                sounds[0].Stop();
                sounds[2].Play();
                player.is_loser = true;
                if (player.is_loser)
                {
                    sounds[3].Play();
                }
                Destroy(player.gameObject);
                Instantiate(cfb_fab, cfb_loc_init);
                state = BattleState.DONE;
                break;
            case BattleState.DONE:
                break;
            default:
                break;
        }
    }

    PlayerBattle spawn_player()
    {
        // Instantiate the actors in a battle
        GameObject p_go = Instantiate(p_fab, p_loc_init);
        PlayerBattle player = p_go.GetComponent<PlayerBattle>();
        Debug.Log($"BATTLE SYSTEM: Player Game Object '{player.name}' Created.");
        return player;
    }

    EnemyBattle spawn_enemy()
    {
        // Instantiate the actors in a battle
        GameObject e_go = Instantiate(e_fab, e_loc_init);
        EnemyBattle enemy = e_go.GetComponent<EnemyBattle>();
        Debug.Log($"BATTLE SYSTEM: Enemy Game Object '{enemy.name}' Created.");
        return enemy;
    }
    

    IEnumerator RemoveRing() 
    {
        Debug.Log("BATTLE SYSTEM: Removing Ring From Scene");
        float s = 0.5f;
        yield return new WaitForSeconds(s);
        DestroyImmediate(GameObject.FindGameObjectsWithTag("Ring")[0]);
    }
}