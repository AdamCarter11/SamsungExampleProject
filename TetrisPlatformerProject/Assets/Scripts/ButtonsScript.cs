using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Samsung;

public class ButtonsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayAgain(){
        SceneManager.LoadScene("MainStage");
    }
    public void ReviewApp(){
        Application.OpenURL("samsungapps://AppRating/<App Package Name>");  //make sure to change, <App Package Name>, to the URL of your app
        print("Loaded Review");
    }
    public void OpenShop(){
        SceneManager.LoadScene("Shop");
    }
    public void Menu(){
        SceneManager.LoadScene("Menu");
    }
    public void ConsumeCoins(){
        MainManager.Instance.testNum--;
    }

    
    
}
