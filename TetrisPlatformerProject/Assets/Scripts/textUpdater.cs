using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textUpdater : MonoBehaviour
{
    [SerializeField] private Text coinText;
    [SerializeField] private Text powerUpText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = "Coins: " + MainManager.Instance.testNum;
        powerUpText.text = "PowerUps: " + MainManager.Instance.freezePowerUpCount;
    }
}
