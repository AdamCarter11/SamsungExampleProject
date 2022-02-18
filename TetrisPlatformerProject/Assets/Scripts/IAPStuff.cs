using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Samsung;
using UnityEngine.UI;
public class IAPStuff : MonoBehaviour
{
    //private System.Action<Samsung.ProductInfoList> lst;
    //private List<Samsung.ProductVo> listOfProducts;
    [SerializeField] private Button productsButton;
    [SerializeField] private Dropdown itemList;
    [SerializeField] private Text itemText;
    void Start()
    {
        //lst = new System.Action<Samsung.ProductInfoList>(returnInfoList);
        //Samsung.SamsungIAP.Instance.GetProductsDetails("", lst);
        //test when seller account goes through
        //Samsung.SamsungIAP.Instance.SetOperationMode(Samsung.OperationMode.OPERATION_MODE_TEST_FAILURE);
        productsButton.onClick.AddListener(OnGetProductsButton);
    }

    void Update()
    {

    }
    
    /*
    void returnInfoList(Samsung.ProductInfoList pil){
        int i = 0;
        if(pil.errorInfo != null){

        }
        else{
            print("error");
            return;
        }
        foreach(var tempPil in pil.results){
            if(tempPil != null){
                listOfProducts.Add(tempPil);
                print(listOfProducts[i]);
                i++;
            }
        }
    }

    //should be almost same thing as products list but with payments (will need to create the new listeners of correct type)
    void onPayment(){

    }
    */
    void OnGetProductsButton(){
        //get all the product details
        SamsungIAP.Instance.GetProductsDetails("", OnGetProductsDetails); 
    }     
    void OnGetProductsDetails(ProductInfoList _productList){
        if (_productList.errorInfo != null){
            if (_productList.errorInfo.errorCode == 0){// 0 means no error
                if (_productList.results != null){
                    itemList.ClearOptions();
                    List<string> optionItems = new List<string>();
                    int i = 1;
                    foreach (ProductVo item in _productList.results){
                            string temp = i+ ". " + item.mItemName + ": $ " + item.mItemPrice;
                            optionItems.Add(temp);
                            i++;
                    }
                    itemList.AddOptions(optionItems);
                }
            }
        }
    }

    void OnConsume(ConsumedList _consumedList){
        if(_consumedList.errorInfo != null){
            if(_consumedList.errorInfo.errorCode == 0){
                if(_consumedList.results != null){
                    foreach(ConsumeVo item in _consumedList.results){
                        if(item.mStatusCode == 0){
                            //successfully consumed and ready to be purchased again.
                        }
                 }
             }
         }
     }
    }

    void OnGetOwnedList(OwnedProductList _ownedProductList){
        if(_ownedProductList.errorInfo != null){
            if(_ownedProductList.errorInfo.errorCode == 0){// 0 means no error
                if(_ownedProductList.results != null){
                    foreach(OwnedProductVo item in _ownedProductList.results){
                        if(item.mConsumableYN == "Y"){
                            //consume the consumable items and OnConsume callback is triggered afterwards
                            SamsungIAP.Instance.ConsumePurchasedItems(item.mPurchaseId, OnConsume);
                        }
                        if(item.mItemId == "1"){
                            //superJump++;
                            itemText.text = "You bought first IAP";
                        }
                        else if(item.mItemId == "BuyUpgradedPlayer"){                         
                            //playerMaterial = Resources.Load<Material>("playerMaterial");
                            //MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                            //meshRenderer.material = playerMaterial;                        
                        }                    
                    }
                } 
            }
        }
    }
}
