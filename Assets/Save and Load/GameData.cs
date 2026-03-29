using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // Core progression
    public int level;
    public int currentXP;
    public int xpToNextLevel;
    public int statPoints;

    // Major stats
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    // Derived stats
    public int damage;
    public int critChance;
    public int critPower;
    public int maxHealth;
    public int armor;
    public int evasion;


    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public SerializableDictionary<string, float> volumeSettings;


    // Constructor sets defaults
    public GameData()
    {
        level = 1;
        currentXP = 0;
        xpToNextLevel = 100;
        statPoints = 5;

        strength = 0;
        agility = 0;
        intelligence = 0;
        vitality = 0;

        damage = 10;
        critChance = 0;
        critPower = 150; // default crit power
        maxHealth = 100;
        armor = 0;
        evasion = 0;

        closestCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();

        volumeSettings = new SerializableDictionary<string, float>();
    }
}

