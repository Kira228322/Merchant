using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Money;

    public PlayersInventorySaveData Inventory;

    public PlayerExperience Experience;

    public PlayerNeedsSaveData Needs;

    public PlayerWagonStatsSaveData WagonStats;

    public PlayerRecipesSaveData Recipes;
    public PlayerData(Player player)
    {
        Money = player.Money;
        Experience = player.Experience;

        Inventory = player.Inventory.SaveData();
        Needs = player.Needs.SaveData();
        WagonStats = player.WagonStats.SaveData();
        Recipes = new(player.Recipes);
    }

}
