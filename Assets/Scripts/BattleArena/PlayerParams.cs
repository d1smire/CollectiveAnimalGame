using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum IsShotter
{
    Yes,
    No,
}

[CreateAssetMenu(fileName = "New params", menuName = "Characters/Characteristics")]
public class PlayerParams : ScriptableObject
{
    [Header("AboutAnimal")]
    public int AnimalId;
    public string Name;
    public IsShotter Shooter;

    [Header("Characteristics")]
    public float Health;
    public float ATK;
    public float DEF;
    public float Speed;
    public int CritChance;
    public int EnergyRecovery;
    public float EnergyForUltimates;
    public float NeedEnergyToUseUltimates;

    [Header("AllAboutLvl")]
    public float CharXP;
    public float NeedXPToLvlUp = 15f;
    public int CharLevel;
    public int PointsForUpgrade;

    public void AddExperience(float amount)
    {
        if (CharLevel <= 10)
        {
            CharXP += amount;
            while (CharXP >= NeedXPToLvlUp)
            {
                LevelUp();
            }
        }
    }

    public void SetParams(Character parameters) 
    {
        this.AnimalId = parameters.AnimalId;
        this.Name = parameters.Name;
        this.Shooter = parameters.Shooter;
        this.Health = parameters.Health;
        this.ATK = parameters.ATK;
        this.DEF = parameters.DEF;
        this.Speed = parameters.Speed;
        this.CritChance = parameters.CritChance;
        this.EnergyRecovery = parameters.EnergyRecovery;
        this.EnergyForUltimates = parameters.EnergyForUltimates;
        this.NeedEnergyToUseUltimates = parameters.NeedEnergyToUseUltimates;
        this.CharXP = parameters.CharXP;
        this.NeedXPToLvlUp = parameters.NeedXPToLvlUp;
        this.CharLevel = parameters.CharLevel;
        this.PointsForUpgrade = parameters.PointsForUpgrade;
    }

    private void LevelUp()
    {
        CharXP -= NeedXPToLvlUp;
        NeedXPToLvlUp = CalculateExperienceToNextLevel(CharLevel);
        CharLevel++;
        PointsForUpgrade += 4;
    }

    private float CalculateExperienceToNextLevel(int level)
    {
        return 30 * (Mathf.Pow(1.5f, level + 1) - 1);
    }

    public Character ReturnClass()
    {
        return new Character(this.AnimalId, this.Name, this.Shooter, this.Health,this.ATK, this.DEF, this.Speed, 
            this.CritChance, this.EnergyRecovery, this.EnergyForUltimates, this.NeedEnergyToUseUltimates, 
            this.CharXP, this.NeedXPToLvlUp, this.CharLevel, this.PointsForUpgrade);
    }

    public void Reset()
    {
        AnimalId = 0;
        Name = "";
        Health = 0;
        ATK = 0;
        DEF = 0;
        Speed = 0;
        CritChance = 0;
        EnergyRecovery = 0;
        EnergyForUltimates = 0;
        NeedEnergyToUseUltimates = 0;
        CharXP = 0;
        NeedXPToLvlUp = 0;
        CharLevel = 0;
        PointsForUpgrade = 0;
    }

    public void UpgradeHealth() 
    {
        Health += 50;
        PointsForUpgrade--;
    }
    public void UpgradeATK()
    {
        ATK += 50;
        PointsForUpgrade--;
    }
    public void UpgradeDEF()
    {
        DEF += 50;
        PointsForUpgrade--;
    }
    public void UpgradeSpeed()
    {
        Speed += 50;
        PointsForUpgrade--;
    }
    public void UpgradeCritChance()
    {
        CritChance += 50;
        PointsForUpgrade--;
    }
    public void UpgradeEnergyRecovery()
    {
        EnergyRecovery += 50;
        PointsForUpgrade--;
    }
}



public class Character
{
    public int AnimalId;
    public string Name;
    public IsShotter Shooter;
    public float Health;
    public float ATK;
    public float DEF;
    public float Speed;
    public int CritChance;
    public int EnergyRecovery;
    public float EnergyForUltimates;
    public float NeedEnergyToUseUltimates;
    public float CharXP;
    public float NeedXPToLvlUp;
    public int CharLevel;
    public int PointsForUpgrade;

    public Character(int animalId, string name, IsShotter shooter, float health, float atk, float def, float speed,
        int critChance, int energyRecovery, float energyForUltimates, float needEnergyToUseUltimates,
        float charXP, float needXpToLvlUp, int charLevel, int pointsForUpgrade)
    {
        this.AnimalId = animalId;
        this.Name = name;
        this.Shooter = shooter;
        this.Health = health;
        this.ATK = atk;
        this.DEF = def;
        this.Speed = speed;
        this.CritChance = critChance;
        this.EnergyRecovery = energyRecovery;
        this.EnergyForUltimates = energyForUltimates;
        this.NeedEnergyToUseUltimates = needEnergyToUseUltimates;
        this.CharXP = charXP;
        this.NeedXPToLvlUp = needXpToLvlUp;
        this.CharLevel = charLevel;
        this.PointsForUpgrade = pointsForUpgrade;
    }
}