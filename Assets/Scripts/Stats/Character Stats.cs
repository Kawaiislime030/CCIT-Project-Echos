using System.Data;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public enum StatType
{
    strength,
    agillity,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHealth,
    armor,
    evasion
}
public class CharacterStats : MonoBehaviour, ISaveManager
{
    private EntityFX fx;

    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public int statPoints = 5;

    [Header("Major Stats")]
    public Stat strength; // 1 point increases damage by 1 and crit.power by 1%
    public Stat agillity; // 1 point increases evasion by 1% and crit.chance by 1%
    public Stat intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality; // 1 point increases health by 3 or 5 points 

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; // default 150% damage on crits 

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;





    public int currentHealth;

    public System.Action onHealthChanged;
    public System.Action onStatsChanged;   
    public System.Action onXPChanged;     


    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    private void Update()
    {
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);

        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }


    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealth(_damage);

        Debug.Log(_damage);

        if (currentHealth <= 0)
        {
            Die();
        }


    }

    protected virtual void DecreaseHealth(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }

       if (_damage > 0)
          fx.CreatePopUpText(_damage.ToString());
    }

    protected virtual void Die()
    {
        // Handle character death (e.g., play animation, disable character, etc.)
        //throw new NotImplementedException();
    }
    public void KillEntity() => Die();
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agillity.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agillity.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + (vitality.GetValue() * 5);
    }

    public Stat GetStat(StatType _statType)
    {
        if (_statType == StatType.strength) return strength;
        else if (_statType == StatType.agillity) return agillity;
        else if (_statType == StatType.intelligence) return intelligence;
        else if (_statType == StatType.vitality) return vitality;
        else if (_statType == StatType.damage) return damage;
        else if (_statType == StatType.critChance) return critChance;
        else if (_statType == StatType.critPower) return critPower;
        else if (_statType == StatType.maxHealth) return maxHealth;
        else if (_statType == StatType.armor) return armor;
        else if (_statType == StatType.evasion) return evasion;


        return null;
    }

    public void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;
        statPoints += 5; // give 5 points per level, adjust as needed
        Debug.Log("Level Up! You are now level " + level + " and have " + statPoints + " stat points.");
        onStatsChanged?.Invoke(); 
        onXPChanged?.Invoke();
        SaveManager.instance.SaveGame();
    }

    public void AddStatPoint(StatType type)
    {
        if (statPoints > 0)
        {
            Stat stat = GetStat(type);
            if (stat != null)
            {
                stat.IncreaseBaseValue(1); // you¡¦ll need a method in Stat to increase its value
                statPoints--;
                // Recalculate derived stats
                if (type == StatType.vitality)
                {
                    currentHealth = GetMaxHealthValue();
                    onHealthChanged?.Invoke(); // notify health UI
                }
                onStatsChanged?.Invoke();


                //Notify all stat slots to refresh
                foreach (UI_StatSlot slot in FindObjectsOfType<UI_StatSlot>())
                {
                    slot.UpdateStatValueUI();
                    slot.UpdateStatPointsUI();
                }


                
            }
        }
    }
    public void GainXP(int amount)
    {
        currentXP += amount;
        onXPChanged?.Invoke();

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }

        SaveManager.instance.SaveGame();
    }

    public void SaveData(ref GameData _data)
    {
        if (CompareTag("Player"))
        {
            _data.level = level;
            _data.currentXP = currentXP;
            _data.xpToNextLevel = xpToNextLevel;
            _data.statPoints = statPoints;

            _data.strength = strength.GetValue();
            _data.agility = agillity.GetValue();
            _data.intelligence = intelligence.GetValue();
            _data.vitality = vitality.GetValue();

            _data.damage = damage.GetValue();
            _data.critChance = critChance.GetValue();
            _data.critPower = critPower.GetValue();
            _data.maxHealth = maxHealth.GetValue();
            _data.armor = armor.GetValue();
            _data.evasion = evasion.GetValue();
        }
    }
    public void LoadData(GameData _data)
    {
        if (CompareTag("Player"))
        {
            level = _data.level;
            currentXP = _data.currentXP;
            xpToNextLevel = _data.xpToNextLevel;
            statPoints = _data.statPoints;

            strength.SetDefaultValue(_data.strength);
            agillity.SetDefaultValue(_data.agility);
            intelligence.SetDefaultValue(_data.intelligence);
            vitality.SetDefaultValue(_data.vitality);

            damage.SetDefaultValue(_data.damage);
            critChance.SetDefaultValue(_data.critChance);
            critPower.SetDefaultValue(_data.critPower);
            maxHealth.SetDefaultValue(_data.maxHealth);
            armor.SetDefaultValue(_data.armor);
            evasion.SetDefaultValue(_data.evasion);

            currentHealth = GetMaxHealthValue();

            onStatsChanged?.Invoke();
            onXPChanged?.Invoke();
        }
    }

}



