using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Money;
    public PlayersInventorySaveData Inventory; //needs data
    public ItemGridSaveData ItemGrid; //needs data
    public PlayerExperience Experience; //ok
    public PlayerNeeds Needs; //ok
    public PlayerWagonStats WagonStats; //почти ok, но надо сериализовать ScriptableObjects

    public PlayerData(Player player)
    {
        Money = player.Money;


        Experience = player.Experience;
        Needs = player.Needs;

    }
}
