using System.Collections;
using System.Collections.Generic;
using UI.QuestScriptableObject;
using TMPro;
using UnityEngine;

namespace UI.Quests 
{

    public class QuestTooltipUI : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveIncompletePrefab;
        [SerializeField] TextMeshProUGUI rewardText;
        //[HideInInspector] public ItemDetails itemDetails;


        public void Setup(QuestStatus status)
        {

            QuestSO quest = status.GetQuest();
            title.text = quest.GetTitle();
            objectiveContainer.DetachChildren();
            foreach(var objective in quest.GetObjectives())
            {
                GameObject prefab = objectiveIncompletePrefab;
                if(status.IsObjectiveComplete(objective.reference))
                {
                    prefab = objectivePrefab;
                }
                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective.description;
                
            }
            rewardText.text = GetRewardText(quest);

        }
   

        private string GetRewardText(QuestSO quest)
        {

            string rewardText = "";
            //string itemDetails = itemDetails.itemDescription;
            //InventoryTextBoxUI inventoryTextBox = inventoryManagement.inventoryTextBoxGameobject.GetComponent<InventoryTextBoxUI>();
            foreach(var reward in quest.GetRewards())
            {
                if(rewardText != "")
                {
                    rewardText += ", ";
                }
                rewardText += "";//something
            }
            if(rewardText == "")
            {
                rewardText = "No reward.";
            }
            rewardText += ".";
            return rewardText;

        }


    }

}