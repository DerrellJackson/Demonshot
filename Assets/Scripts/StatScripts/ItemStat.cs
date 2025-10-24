using CharacterBuildData.CharacterStats;
//Right now this is just a refrence for me to use the character/stat modifier scripts
public class Character
{
  public CharacterStats Strength;
}

public class ItemStats
{
    public void Equip(Character c)
    {  
        c.Strength.AddModifier(new StatModifier(10, StatModType.Flat, this));//"this is where they came from;
        c.Strength.AddModifier(new StatModifier(0.1f, StatModType.PercentMult, this));
    }
    public void Unequip(Character c)
    {
        c.Strength.RemoveAllModifiersFromSource(this); 
    }
}
