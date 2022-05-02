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
    END_TURN
}


public class BattleSystem : MonoBehaviour
{
    // take in the prefabs for the actors
    public GameObject p_fab;
    public GameObject e_fab;
    public GameObject jr_fab;
    // get the positions to spawn them in at

    public Transform p_loc_init;
    public Transform e_loc_init;
    public Transform jr_loc_init;

    // jr game object
    public GameObject jr_go;

    public bool manual_ring_del, auto_ring_del;
    /*public Text p_name;
    public Text p_hp;
    public Text p_mp;
    public Text p_sp;*/
    //[RequireComponent(typeof(PlayerHUD))]
    public PlayerHUD p_hud;
    public AudioSource[] sounds;

    public Bag bag;
    public PlayerBattle player;
    EnemyBattle enemy;
    
    //public JudgmentRing judgment_ring;

    public BattleState state;
    public PlayerState p_state;

    public int item_num;
    
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
        this.item_num = 2;
        this.jr_go = null;
        //prepare_battle();
        sounds[0].Play();
        player = spawn_player();
        enemy = spawn_enemy();
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

                // HANDLE PLAYER INPUT HERE
                // Here we only want to take in the player input then redirect them to a ring or possibly something else
                if (Input.GetKeyDown(KeyCode.A) && this.jr_go == null)
                {
                    this.jr_go = player.spawn_standard_ring(bag.rings_list[player.ring_num]);
                    p_state = PlayerState.STANDARD;
                }

                if (Input.GetKeyDown(KeyCode.D) && this.jr_go == null && bag.items[bag.item_list[item_num]] >= 0)
                {
                    bag.item_list[item_num].jr_fab = this.jr_fab;
                    foreach(Item i in bag.items.Keys)
                        Debug.Log($"bag.Keys: {i.name}");
                    foreach (Item i in bag.item_list)
                        Debug.Log($"bag.item_list: {i.name}");

                    Debug.Log($"{bag.item_list[item_num].name}");
                    Debug.Log($"{bag.item_list[item_num].jr_fab}");
                    this.jr_go = player.spawn_item_ring(bag.rings_list[0]);
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
                    }
                    else
                    {
                        Debug.Log($"ITEM INDEX: {++item_num}");
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
                        bag.sounds[item_num].Play();
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
                enemy.Attack(player);
                if (player.hp <= 0) 
                {
                    Debug.Log("PLAYER LOSES");
                    state = BattleState.LOSE;
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
                state = BattleState.DONE;
                break;
            case BattleState.LOSE:

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
        Debug.Log($"Player Game Object '{player.name}' Created.");
        return player;
    }

    EnemyBattle spawn_enemy()
    {
        // Instantiate the actors in a battle
        GameObject e_go = Instantiate(e_fab, e_loc_init);
        EnemyBattle enemy = e_go.GetComponent<EnemyBattle>();
        Debug.Log($"Enemy Game Object '{enemy.name}' Created.");
        return enemy;
    }
    
    IEnumerator RemoveRing() 
    {
        Debug.Log("DEBUG: Removing Ring From Scene");
        float s = 0.5f;
        yield return new WaitForSeconds(s);
        DestroyImmediate(GameObject.FindGameObjectsWithTag("Ring")[0]);
    }
    /*
    JudgmentRing spawn_ring(PlayerBattle player)
    {
        Debug.Log("PB: Getting Ring Input...");
        player.ring_finished = false;
        //.is_done = false;
        GameObject jr_go = Instantiate(jr_fab, jr_loc_init);
        JudgmentRing judgment_ring = jr_go.GetComponent<JudgmentRing>();
        Debug.Log($"Ring Created");
        return jr;
    }
    */
}