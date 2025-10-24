using System.Collections;
using System.Collections.Generic;
using UI.QuestScriptableObject;
using UnityEngine;


namespace UI.Quests
{
    public class QuestCompletion : MonoBehaviour
    {

        [SerializeField] QuestSO quest;
        [SerializeField] string objective;


        public void CompleteObjective()
        {

            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.CompleteObjective(quest, objective);

        }


    }
}