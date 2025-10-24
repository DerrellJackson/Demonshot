using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//MAKE SURE TO SAY IM USING THE BELOW FOR ANYTHING THAT USES THIS SCRIPT
namespace CharacterBuildData.CharacterStats
{
[Serializable]
public class CharacterStats 
{
    public float BaseValue;
    
    //This is so we can retrieve the calculated value
    public virtual float Value { 
        get {
            if (isDirty || BaseValue != lastBaseValue){
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
          }
        }
   
    //this is for recalculating 
    protected bool isDirty = true;
   
    //holds our most recent calulation
    protected float _value;

    protected float lastBaseValue = float.MinValue;
   
    protected readonly List<StatModifier> statModifiers;
    public readonly ReadOnlyCollection <StatModifier> StatModifiers;

   //void someFunc()
    //{  so we cant acidentally modify it or break it
      //  statModifiers = null;
      // statModifiers = new List<StatModifier>();
     // add in the list itself
     //statModifiers[0] = null;
     //   statModifiers.Add(new StatModifier());
     //}
      public CharacterStats(){
      statModifiers = new List<StatModifier>();
      StatModifiers = statModifiers.AsReadOnly();
    }
     public CharacterStats(float baseValue) : this()
     {
      BaseValue = baseValue;
      }
      public virtual void AddModifier(StatModifier mod)
      {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);
      }
      protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
      {
        if (a.Order < b.Order)
        return -1;
        else if (a.Order > b.Order)
        return 1;
        return 0;//if (a.Order == b.Order)
      }
      public bool RemoveModifier(StatModifier mod)
      { 
        if (statModifiers.Remove(mod))
        {
          isDirty = true;
          return true;
        }
         return false;
      }
     
     public virtual bool RemoveAllModifiersFromSource(object source)
     {
        bool didRemove = false;

        for (int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {   
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
     }

     protected virtual float CalculateFinalValue()
     {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for (int i = 0; i < statModifiers.Count; i++)
        {
           StatModifier mod = statModifiers[i];
           if (mod.Type == StatModType.Flat)
           //adding all the values together to calculate it
           { finalValue += mod.Value;}
           else if (mod.Type == StatModType.PercentAdd)
           {sumPercentAdd += mod.Value;
           if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
           {
                finalValue *= + sumPercentAdd;
                sumPercentAdd = 0;
           } 
           }
           else if (mod.Type == StatModType.PercentMult)
           {//adding the percent so instead on 100% it is 110%
            finalValue *= 1 + mod.Value;
           }
        }
        //this is for float errors so 4 is usually precise enough
        return (float)Math.Round(finalValue, 4);
     }
}
}