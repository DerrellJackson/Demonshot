using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {

        //ENEMY[] May be changed for NON DUNGEON ENEMIES
        [SerializeField] Enemy[] enemies;
        [SerializeField] bool activateOnStart = false;


        private void Start() 
        {

            Activate(activateOnStart);

        }


        public void  Activate(bool shouldActivate)
        {

            foreach(Enemy enemy in enemies)
            {
                //CombatTarget target = enemy.GetComponent<CombatTarget>();
                /*if(target != null)
                {
                    target.enabled = shouldActivate;
                }*/
                enemy.enabled = shouldActivate;
            }

        }

    }
}