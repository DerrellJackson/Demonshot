using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace Shopping.Shops
{
    
    public class Shopper : MonoBehaviour
    {
      
        Shop activeShop = null;

        public event Action activeShopChange;

        
        private bool canActivate;

        public void SetActiveShop(Shop shop) 
        {   
            if(activeShop != null)
            {
                activeShop.SetShopper(null);
            }
            activeShop = shop;
            if(activeShop != null)
            {
                activeShop.SetShopper(this);
            }
            if(activeShopChange != null)
            {
                activeShopChange();
            }

        }


        public Shop GetActiveShop()
        {

            return activeShop;

        }


    }
    
}
