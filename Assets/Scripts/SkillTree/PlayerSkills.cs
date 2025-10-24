using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
   
    public enum SkillType
    {
        //HP SKILLS
        increaseHp,
        increaseSpeed,
        increaseHungerAmount,
        regenHPWhileStandingStill,
        morePotionsInShop,
        regenHP,
        regenHPFaster,
        regenHPMaxSpeed,
        increaseDashDistance,
        increaseDashSpeed,
        decreaseTimeForNextDash,
        vampiricHealing, //heal when damaging enemy
        frighteningPresence, //heal while draining enemy hp in the radius
        noLongerPoisoned, //removes poison forever
        noLongerBleeding, //removes bleeding forever
        noLongerHungry, //removes hunger
        noLongerHuman, //demonic, heals twice as fast while damaging enemies while also having a demon inside you to randomly hurt you

        //COMBAT SKILLS
        increaseAttackSpeed,
        increaseEnemyDropRate,
        increaseCritDamage,
        increaseCritChance,
        increaseMeleeDamage,
        increaseRangedDamage,
        increaseGoldDropRate,
        increaseItemDropRate,
        enemiesDropPotions, //enemies can now drop potions
        enemyBleeds, //hitting enemy causes bleed
        enemyGetsPoisioned, //hitting enemy causes poison effect
        enemyGetsSlowed, //hitting the enemy causes the enemy to slowdown
        shadowBlade, //swing your weapon a second time
        noMoreLosingItems, //death no longer takes away items, money not included
        noLongerTired, //removes energy
        theChosenOne, //can wield Unique Weapons
        theDestroyerOfAll, //deals double damage at risk of occasionally hurting yourself with that same amount of damage

        //INTERACTION SKILLS
        increaseInteractions,
        slowNegativeStats,       
        removeNegativeStats,
        increaseEnergyAmount,
        increaseJoyAmount,
        decreaseTownPrice,
        decreaseShopPrice,
        decreaseFarmToolsPrice,
        decreaseFarmSeedsPrice,
        decreaseWeaponPrice,
        decreasePotionPrice,
        obtainBasicItemSales, //basic gear
        obtainMidItemSales, //average gear, maybe rings, some new tool types, decor
        obtainRareItemSales, //decent gear, rings, unusual seeds, better tools, more decor
        obtainRarestItemSales, //top quality gear, rings, rare seeds, rare tools, home upgrades, more decor, stronger potions
        obtainLegendartItemSales, //unique gear, unique rings, unique seeds, unique tools, unique decor decor items, best potion sales, boss spawn battles
        noLongerUnhappy, //removes happiness
        allAreHappy, //all villagers like you and give reduced prices, but now demons can spawn in the overworld a little more frequent

        //FARMING SKILLS
        increaseWaterRadius,
        increaseWaterRadiusMore,
        increaseWaterRadiusMax,
        increaseCropToolStrength,
        increaseCropToolStrengthMore,
        increaseCropToolStrengthMax,
        addBasicFarmRecipes, //beginning ores, beginning animal hides
        addMidFarmRecipes, //average ores, average animal hides
        addSkilledFarmRecipes, //harder ores to find and make, harder animals hides to get
        addTalentedFarmRecipes, //rare ores to make, rare animal hides
        addMasterFarmRecipes, //rarest ores to make, rarest animal hides 
        removeDaysPastLastWater,
        staminaDoesNotDrainOnWater,
        farmingIncreasesJoy,
        farmingIncreasesEnergy,
        farmingYieldsMoreCrops,
        farmingInAnySeason, //farm in any season, but demons can naturally spawn in your farm now

        //EXTRA SKILLS (Money skills)
        increaseFishingHookRate,
        fishingIncreasesJoy,
        fishingIncreasesEnergy,
        increaseMiningSpeed,
        increaseOreValue,
        increaseSellingPrice,
        someCropsDropCoins, //crops can now have money in it
        someOresDropCoins, //ores can now have money in it
        someFishDropCoins, //fish can now have money in it
        noMoreGoldLossWhenDead, //death no longer drops gold
        addBasicCookingRecipes,
        addMidCookingRecipes,
        addSkilledCookingRecipes,
        addTalentedCookingRecipes,
        addMasterCookingRecipes,
        increaseXPgain,
        freeLevelUp, //gives 2 skill points
        gatesOfHell, //all dungeons are accessible at all times, but enemies can spawn in any area
        
    }

    private List<SkillType> unlockedSkillTypeList;

    public PlayerSkills()
    {
    
        unlockedSkillTypeList = new List<SkillType>();
    
    }


    public void UnlockSkill(SkillType skillType)
    {

        if(!IsSkillUnlocked(skillType))
        {
            unlockedSkillTypeList.Add(skillType);
        }
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        
        return unlockedSkillTypeList.Contains(skillType);

    }

}
