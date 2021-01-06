// Item Database - Esta classe le um arquivo Json e cria uma database
// de items, talvez seja melhor criar cada item e manter uma lista de itens no jogo


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class ItemDatabase : MonoBehaviour {
    
    //public List<Item> database = new List<Item>();
    private JsonData itemData;
    public int numItems;
    public List<DataItemScriptableObject> wasteItemsDatabase;
    public List<DataItemScriptableObject> oldItemsDatabase;
    public List<DataItemScriptableObject> normalItemsDatabase;
    public List<DataItemScriptableObject> goodItemsDatabase;
    public List<DataItemScriptableObject> greatItemsDatabase;
    public List<DataItemScriptableObject> flawlessItemsDatabase;


    void Start()
    {


        //if (Application.platform == RuntimePlatform.IPhonePlayer)
        //    itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Raw/resourcedata.json"));

        //else itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/resourcedata.json"));

        //ConstructItemDatabase();

        //Debug.Log(FetchItemByID(1).Description);
        numItems = wasteItemsDatabase.Count + oldItemsDatabase.Count + normalItemsDatabase.Count + goodItemsDatabase.Count + greatItemsDatabase.Count + flawlessItemsDatabase.Count;
        
    }

    //public Item FetchItemByDatabase(int db, int index)
    //{
    //    //if (index <= database2[db].Count) return database2[db][index];
    //}
    
    public DataItemScriptableObject FetchItemByDatabaseAndIndex(List<DataItemScriptableObject> db, int idx)
    {

        return db[idx];
        //switch(db)
        //{
        //    case 0:
        //        return wasteItemsDatabase[idx];
                
        //    case 1:
        //        return oldItemsDatabase[idx];
                
        //    case 2:
        //        return normalItemsDatabase[idx];
                
        //    case 3:
        //        return goodItemsDatabase[idx];
                
        //    case 4:
        //        return greatItemsDatabase[idx];
                
        //    case 5:
        //        return flawlessItemsDatabase[idx];
                
        //    default: return null;
                
        //}

        
    }

    public DataItemScriptableObject FetchItemById(int id)
    {
        
        foreach(DataItemScriptableObject it in wasteItemsDatabase)
        {
            if (it.id == id) return it;
        }

        foreach (DataItemScriptableObject it in oldItemsDatabase)
        {
            if (it.id == id) return it;
        }

        foreach (DataItemScriptableObject it in normalItemsDatabase)
        {
            if (it.id == id) return it;
        }

        foreach (DataItemScriptableObject it in goodItemsDatabase)
        {
            
            if (it.id == id) return it;
        }

        foreach (DataItemScriptableObject it in greatItemsDatabase)
        {
            
            if (it.id == id) return it;
        }

        foreach (DataItemScriptableObject it in flawlessItemsDatabase)
        {
            
            if (it.id == id) return it;
        }
        
        return null;
    }


    //public Item FetchItemByID(int id)
    //{
        
    //    for (int i = 0; i < numItems; i++)
    //    {

    //        if (database[i].ID == id)
    //        {

    //            return database[i];
    //        }
    //    }
        
    //    return null;
    //}
    //void ConstructItemDatabase()
    //{
    //    for(int i = 0; i < itemData.Count; i++)
    //    {
    //        database.Add(new Item((int)itemData[i]["id"],
    //                                    itemData[i]["title"].ToString(),
    //                                    (int)itemData[i]["type"],
    //                                    (int)itemData[i]["stats"]["xp"],
    //                                    (int)itemData[i]["stats"]["strength"],
    //                                    (int)itemData[i]["stats"]["dexterity"],
    //                                    (int)itemData[i]["stats"]["inteligence"],
    //                                    (int)itemData[i]["stats"]["hp"],
    //                                    (int)itemData[i]["stats"]["energy"],
    //                                    (int)itemData[i]["stats"]["hp_rate"],
    //                                    (int)itemData[i]["stats"]["energy_rate"],
    //                                    (int)itemData[i]["stats"]["damage"],
    //                                    (int)itemData[i]["stats"]["crit_chance"],
    //                                    (int)itemData[i]["stats"]["defense"],
    //                                    (int)itemData[i]["stats"]["movement"],
    //                                    (int)itemData[i]["stats"]["energy_consume"],
    //                                    itemData[i]["description"].ToString(),
    //                                    (bool)itemData[i]["stackable"],
    //                                    (int)itemData[i]["rarity"],
    //                                    itemData[i]["slug"].ToString()));
    //    }
    //    numItems = database.Count;
    //}
   // void ConstructItemDatabase()
   // {
        //for (int i = 0; i < itemData.Count; i++)
        //{
        //    database.Add(new Item((int)itemData[i]["id"],
        //                                itemData[i]["title"].ToString(),
        //                                (int)itemData[i]["type"],
        //                                (int)itemData[i]["value"],
        //                                itemData[i]["description"].ToString(),
        //                                (bool)itemData[i]["stackable"],
        //                                itemData[i]["rarity"].ToString(),
        //                                itemData[i]["slug"].ToString(),
        //                                (int)itemData[i]["quality"]));
        //}
        //numItems = database.Count;
        
   // }


}


//public class Item
//{

//    public int ID { get; private set; }
//    public string Title { get; private set; }
//    public int Type { get; private set; }
//    public int XP { get; private set; }
//    public int Strenght { get; private set; }
//    public int Dexterity { get; private set; }
//    public int Inteligence { get; private set; }
//    public int Hp { get; private set; }
//    public int Energy { get; private set; }
//    public int HpRate { get; private set; }
//    public int EnergyRate { get; private set; }
//    public int Damage { get; private set; }
//    public int CritChance { get; private set; }
//    public int Defense { get; private set; }
//    public int Movement { get; private set; }
//    public int EnergyConsume { get; private set; }
//    public string Description { get; private set; }
//    public bool Stackable { get; private set; }
//    public int Rarity { get; private set; }
//    public string Slug { get; private set; }
//    public Sprite Sprite { get; private set; }

//    public Item(int id, 
//                string title,
//                int type,
//                int xp,
//                int strength,
//                int dexterity,
//                int inteligence,
//                int hp,
//                int energy,
//                int hprate,
//                int energyrate,
//                int damage,
//                int critchance,
//                int defense,
//                int movement,
//                int energyconsume,
//                string description,
//                bool stackable,
//                int rarity,
//                string slug)
//    {
//        this.ID = id;
//        this.Title = title;
//        this.Type = type;
//        this.XP = xp;
//        this.Strenght = strength;
//        this.Dexterity = dexterity;
//        this.Inteligence = inteligence;
//        this.Hp = hp;
//        this.Energy = energy;
//        this.HpRate = hprate;
//        this.EnergyRate = energyrate;
//        this.Damage = damage;
//        this.CritChance = critchance;
//        this.Defense = defense;
//        this.Movement = movement;
//        this.EnergyConsume = energyconsume;
//        this.Description = description;
//        this.Stackable = stackable;
//        this.Rarity = rarity;
//        this.Slug = slug;
//        this.Sprite = Resources.Load<Sprite>("Sprites/items/" + slug);
//    }

//    public Item()
//    {
//        this.ID = -1;
//    }
//}

//[System.Serializable]
//public class Item
//{
//    public enum modifier { XP, STRENGTH, DEXTERITY, INTELIGENCE, HP,
//                           ENERGY, HPRATE, ENERGYRATE, DAMAGE, CRITCHANCE,
//                            DEFENSE, AGILITY, ENERGYCONSUME, GOLDGAIN, HPRESTORE, ENERGYRESTORE}
//    public enum QualityType { DAMAGED, OLD, NORMAL, GOOD, GREAT, FLAWLESS}
//    public int ID { get; private set; }
//    public string Title { get; private set; }
//    public modifier Type { get; private set; }
//    public int Value { get; private set; }
//    public string Description { get; private set; }
//    public bool Stackable { get; private set; }
//    public string Rarity { get; private set; }
//    public string Slug { get; private set; }
//    public Sprite Sprite { get; private set; }
//    public QualityType Quality { get; private set; }

//    public Item(int id,
//                string title,
//                modifier type,
//                int value,
//                string description,
//                bool stackable,
//                string rarity,
//                string slug,
//                QualityType quality)
//    {
//        this.ID = id;
//        this.Title = title;
//        this.Type = type;
//        this.Value = value;
//        this.Description = description;
//        this.Stackable = stackable;
//        this.Rarity = rarity;
//        this.Slug = slug;
//        this.Sprite = Resources.Load<Sprite>("Sprites/items/" + slug);
//        this.Quality = quality;
//    }

//    public Item()
//    {
//        this.ID = -1;
//    }
//}