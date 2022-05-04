using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagicType
{
    NONE,
    FIRE,
    ICE,
    WIND,
    EARTH,
    DARK,
    LIGHT
}

public enum ItemType
{
    CONSUMABLE,
    EQUIPMENT
}

public enum EquipType 
{
    // EQUIP TYPES FOR PLAYER BODY - EFFECTS STATS
    WEAPON,
    HEAD,
    BODY,
    LEGS,
    // EQUIP TYPES FOR RING - EFFECTS RING IN VARIOUS WAYS
    RING_EQUIP, // WILL ALTER THE WAY THE PLAYER'S RING WORKS
    RING_EFFECT // Will work like the standard ring items in SH:FtNW
}

// Bag
//
// The Bag will act as a singleton which contains a list of items for all player actors to use
public class Bag : MonoBehaviour
{
    //
    //List<Item> items;
    //KeyValuePair<Item, int> items;
    public GameObject heal_ring;
    public Dictionary<Item, int> items;

    // Singleton Instance
    public List<Item> item_list;
    public List<GameObject> rings_list;
    public AudioSource[] sounds;
    public static Bag Instance
    {
        get; private set;
    }


    public MagicType magic_type;
    public ItemType item_type;
    public EquipType equip_type;

    // Start is called before the first frame update
    void Start()
    {
        items = new Dictionary<Item, int>(); //new KeyValuePair<Item, int>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //GameObject i_go = Instantiate(new GameObject());
        //Item medi_leaf = i_go.GetComponent<MediLeaf>();//
        Item medi_leaf = new MediLeaf();
        Item mana_leaf = new ManaLeaf();
        Item pure_leaf = new PureLeaf();
        //medi_leaf.jr_fab = heal_ring.gameObject;
        AppendToBag(medi_leaf);
        AppendToBag(mana_leaf);
        AppendToBag(pure_leaf);
        item_list = GenerateItemList();
        //Instance.items.Add(medi_leaf, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Compares an item against the current bag to see if we alread have an instance
    //  Returns true if it is in the bag already
    //  Returns false if it is not currently in the bag
    static bool CheckIfInBag(Item item) 
    {
        if (Instance.items.ContainsKey(item)) 
        {
            return true;
        }
        return false;
        /*
        foreach (Item i in Instance.items.Keys) 
        {
            if (item.name == i.name) 
            {
                return true;
            }
        }
        return false;
        */
    }

    // AppendToBag
    //  Checks if item is already in bag
    //
    public static void AppendToBag(Item item) 
    {
        if (CheckIfInBag(item))
        {
            Instance.items[item] += 1; // increase count if already in bag
        }
        else 
        {
            Instance.items[item] = 1; // make a new entry in dictionary and init its count
        }
    }

    // RemoveFromBag
    // if our item count is not going to be 0 just do a decrement
    // if our item count will be 0 we want to remove it from the bag
    public static void RemoveFromBag(Item item, int amt=1) 
    {
        if (CheckIfInBag(item)) 
        {
            if ((Instance.items[item] - amt) != 0)
            {
                Instance.items[item] -= amt;
            }
            else 
            {
                Instance.items.Remove(item);
            }
        }
    }
    public static List<Item> GenerateItemList() 
    {
        List<Item> ret = new List<Item>();
        foreach (Item item in Instance.items.Keys) 
        {
            ret.Add(item);
        }
        return ret;

    }

    // Equip related stuff

    public static void EquipToPlayer(PlayerBattle player, Equip equip) 
    {
        return; //if()
    }

}
