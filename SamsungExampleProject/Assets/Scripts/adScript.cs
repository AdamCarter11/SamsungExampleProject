using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class adScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayBeforeScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DelayBeforeScene(){
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameOver");
    }
}
