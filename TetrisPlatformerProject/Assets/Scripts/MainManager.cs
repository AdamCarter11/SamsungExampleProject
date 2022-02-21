using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    [HideInInspector] public int testNum;           //I will need to save this
    [HideInInspector] public bool betterPlayer = false;
    [HideInInspector] public bool adFree = false;
    
    private void Awake() {
        if(Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        testNum = PlayerPrefs.GetInt("coins");  //retrieves coin data
        DontDestroyOnLoad(gameObject);
    }

    //used to save data (coins)
    private void OnApplicationFocus(bool focusStatus) {
       PlayerPrefs.SetInt("coins", testNum); 
    }
    private void OnApplicationQuit() {
        PlayerPrefs.SetInt("coins", testNum); 
    }
}
