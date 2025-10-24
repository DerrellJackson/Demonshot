using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.QuestScriptableObject
{

    [CreateAssetMenu(fileName = "QuestSO", menuName = "Scriptable Objects/QuestSO", order = 0)]
    public class QuestSO : ScriptableObject 
    {
 
        [SerializeField] List<Objective> objectives = new List<Objective>();
        [SerializeField] List<Reward> rewards = new List<Reward>();


        [System.Serializable]
        public class Reward
        {

            public int number;
            public Item item;

        }


        [System.Serializable]
        public class Objective 
        {

            public string reference;
            public string description;

        }


        public string GetTitle() 
        {
            return name;
        }


        public int GetObjectiveCount() 
        {

            return objectives.Count;

        }


        public IEnumerable<Objective> GetObjectives() 
        {
            
            return objectives;

        }


        public IEnumerable<Reward> GetRewards()
        {

            return rewards;

        }


        public bool HasObjective(string objectiveRef)
        {

            foreach(var objective in objectives)
            {
                if(objective.reference == objectiveRef)
                {
                    return true;
                }
            }
            return false;
            
        }


        public static QuestSO GetByName(string questName)
        {

            foreach(QuestSO quest in Resources.LoadAll<QuestSO>(""))
            {
                if(quest.name == questName)
                {
                    return quest;
                }
            }
            return null;

        }


    }   
}