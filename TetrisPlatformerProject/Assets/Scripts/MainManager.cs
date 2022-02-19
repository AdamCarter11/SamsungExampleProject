using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    [HideInInspector] public int testNum = 0;
    [HideInInspector] public bool betterPlayer = false;
    [HideInInspector] public bool adFree = false;
    
    private void Awake() {
        if(Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
