using System.Collections.Generic;
using UI.QuestScriptableObject;
using UnityEngine;


namespace UI.Quests
{
 
    public class QuestStatus
    {

        QuestSO quest;
        List<string> completedObjectives = new List<string>();


        [System.Serializable]
        public class QuestStatusRecord
        {
            
            public string questName;
            public List<string> completedObjectives;

        }


        public QuestStatus(QuestSO quest)
        {

            this.quest = quest;

        }


        public QuestStatus(object objectState)
        {

            QuestStatusRecord state = objectState as QuestStatusRecord;
            quest = QuestSO.GetByName(state.questName);
            completedObjectives = state.completedObjectives;

        }


        public QuestSO GetQuest()
        {
            return quest;
        }


        public bool IsComplete()
        {

            return true;

        }


        public int GetCompletedCount() 
        {

            return completedObjectives.Count;

        }


        public bool IsObjectiveComplete(string objective)
        {

            return completedObjectives.Contains(objective);

        } 


        public void CompleteObjective(string objective)
        {
            
            completedObjectives.Add(objective);
            
        }


        public object CaptureState()
        {

            QuestStatusRecord state = new QuestStatusRecord();
            state.questName = quest.name;
            state.completedObjectives = completedObjectives;
            return state;

        }


    }
}