using System.Collections;
using System.Collections.Generic;
using Shopping.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shops 
{
    public class ShopUI : MonoBehaviour
    {
        
        [SerializeField] TextMeshProUGUI shopName;
        [SerializeField] Transform listRoot;
        [SerializeField] RowUI rowPrefab;
        [SerializeField] TextMeshProUGUI totalAmountToPay;
        [SerializeField] Button switchButton;
        [SerializeField] TextMeshProUGUI shopModeText;

        Shopper shopper = null;
        Shop currentShop = null;


        void Start() 
        {

            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
            if(shopper == null) 
            {
                return;
            }

            shopper.activeShopChange += ShopChanged;
            switchButton.onClick.AddListener(SwitchMode);
            
            ShopChanged();

        }      


        private void ShopChanged()
        {
            if(currentShop != null)
            {
                currentShop.onChange -= RefreshUI;
            }

            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);

            if(currentShop == null) return;
            shopName.text = currentShop.GetShopName();

            currentShop.onChange += RefreshUI;

            RefreshUI();

        }


        private void RefreshUI()
        {
            foreach(Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }
            foreach(ItemDetails item in currentShop.GetFilteredItems())
            {
                RowUI row = Instantiate<RowUI>(rowPrefab, listRoot);
                row.Setup(currentShop, item);
            }
            totalAmountToPay.text = $"${currentShop.TransactionTotal():N2}";
            
            TextMeshProUGUI switchText = switchButton.GetComponentInChildren<TextMeshProUGUI>();
            if(currentShop.IsBuyingMode())
            {
                switchText.text = "Buy Mode";
                shopModeText.text = "Shop Buying";
            }
            else 
            {
                switchText.text = "Sell Mode";
                shopModeText.text = "Shop Selling";
            }
        }


        public void Close()
        {
            shopper.SetActiveShop(null);
        }

        public void ConfirmTransaction()
        {
            currentShop.ConfirmTransaction();
        }


        public void SwitchMode()
        {

            currentShop.SelectMode(!currentShop.IsBuyingMode()); 

        }

    }
}       