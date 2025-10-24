using System.Collections;
using System.Collections.Generic;
using UI.QuestScriptableObject;
using UnityEngine;

namespace UI.Quests
{
  
    public class QuestGiver : MonoBehaviour
    {
   
        [SerializeField] QuestSO[] quests;


        public void GiveQuest(int index) 
        {

            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.AddQuest(quests[index]);

        }


    }

}