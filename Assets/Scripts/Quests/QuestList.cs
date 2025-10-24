using System;
using System.Collections;
using System.Collections.Generic;
using Quests.Saving;
using UI.QuestScriptableObject;
using UnityEngine;

namespace UI.Quests 
{
    
    public class QuestList : MonoBehaviour, ISaveQuests
    {
  
        List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action onUpdate;


        public void AddQuest(QuestSO quest)
        {

            if(HasQuest(quest)) return;
            QuestStatus newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);
            if(onUpdate != null)
            {
                onUpdate();
            }

        }


        public void CompleteObjective(QuestSO quest, string objective)
        {


            QuestStatus status = GetQuestStatus(quest);
            status.CompleteObjective(objective);
            if(status.IsComplete())
            {
                GiveReward(quest);
            }
            if(onUpdate != null)
            {
                onUpdate();
            }
            

        }


        public bool HasQuest(QuestSO quest)
        {

            return GetQuestStatus(quest) != null;

        }


        public IEnumerable<QuestStatus> GetStatuses() 
        {

            return statuses;

        }


        private QuestStatus GetQuestStatus(QuestSO quest)
        {

            foreach (QuestStatus status in statuses)
            {
                if(status.GetQuest() == quest)
                {
                    return status;
                }
            }
            return null;

        }

        
        private void GiveReward(QuestSO quest)
        {

            foreach(var reward in quest.GetRewards()) 
            {
                Item item = GetComponent<Item>();
                //InventoryManager.Instance.AddItem(InventoryLocation.player, item);
                //bool success = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                //if(!success)
                //{
                // GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                //}
                
            }

        }


        public object CaptureState()
        {

            List<object> state = new List<object>();
            foreach(QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());
            }
            return state;

        }


        public void RestoreState(object state)
        {

            List<object> stateList = state as List<object>;
            if(stateList == null) return;

            statuses.Clear();
            foreach(object objectState in stateList)
            {
                statuses.Add(new QuestStatus(objectState));
            }

        }


    }

}