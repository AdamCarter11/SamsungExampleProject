using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IAPStuff : MonoBehaviour
{
    private System.Action<Samsung.ProductInfoList> lst;
    private List<Samsung.ProductVo> listOfProducts;
    void Start()
    {
        lst = new System.Action<Samsung.ProductInfoList>(returnInfoList);
        Samsung.SamsungIAP.Instance.GetProductsDetails("", lst);
        //test when seller account goes through
        //Samsung.SamsungIAP.Instance.SetOperationMode(Samsung.OperationMode.OPERATION_MODE_TEST_FAILURE);
        
    }

    void Update()
    {

    }

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
}
