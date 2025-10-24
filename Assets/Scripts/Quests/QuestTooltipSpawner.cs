using System.Collections;
using System.Collections.Generic;
using Demonshot.UI.TooltipsInvUI;
using UI.QuestScriptableObject;
using UI.Quests;
using UnityEngine;

namespace UI.QuestsTooltipSpawner
{
public class QuestTooltipSpawner : TooltipSpawner
{
   
    public override bool CanCreateTooltip() 
    {

        return true;   

    }


    public override void UpdateTooltip(GameObject tooltip)
    {

        
        QuestStatus status = GetComponent<QuestItemUI>().GetQuestStatus();
        tooltip.GetComponent<QuestTooltipUI>().Setup(status);


    }


}
}