using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float followSpeed;
    private float startingY;
    private float heighestY;
    private float constCamera=0;
    [SerializeField]
    private float cameraChangeAmount;
    [SerializeField]
    private Text scoreBox, powerUpText;
    private float playerScore;
    private float playerStartingY;
    // Start is called before the first frame update
    void Start()
    {
        startingY = transform.position.y-target.transform.position.y;
        heighestY = target.transform.position.y;
        playerStartingY = target.transform.position.y;
        StartCoroutine(MoveCamera());
    }

    // Update is called once per frame
    void Update()
    {
        //used to calculate where the camera should be based on the players heighest Y val
        if(target.transform.position.y > heighestY && target.transform.position.y >= transform.position.y+2){
            heighestY = (target.transform.position.y)-6;
            //Debug.Log(heighestY+startingY);
        }
        
        //checks if player has fallen off the screen
        if(target.transform.position.y < transform.position.y-startingY-1.1){
            Debug.Log("fallen off the screen");
            //SceneManager.LoadScene("GameOver");
            SceneManager.LoadScene("tempAd");
        }

        //what actually moves the camera
        Vector3 posToFollow = new Vector3(transform.position.x, heighestY + startingY + constCamera, transform.position.z);
        transform.position = Vector3.Lerp(transform.position,posToFollow, followSpeed*Time.deltaTime);

        //calculates and displays score
        if(target.transform.position.y > playerScore){
            playerScore = target.transform.position.y;
        }
        scoreBox.text = "Distance: " + playerScore.ToString("F2");
        powerUpText.text = "PowerUps: " + MainManager.Instance.freezePowerUpCount;
    }

    //makes camera move slightly over time
    IEnumerator MoveCamera(){
        while(true){
            yield return new WaitForSeconds(.1f);
            constCamera += cameraChangeAmount;
        }
    }
}
