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
    private Text scoreBox;
    private float playerScore;
    // Start is called before the first frame update
    void Start()
    {
        startingY = transform.position.y-target.transform.position.y;
        heighestY = target.transform.position.y;
        StartCoroutine(MoveCamera());
    }

    // Update is called once per frame
    void Update()
    {
        if(target.transform.position.y > heighestY && target.transform.position.y >= transform.position.y+2){
            heighestY = (target.transform.position.y-transform.position.y)/2;
            //Debug.Log(heighestY+startingY);
        }
        
        if(target.transform.position.y < transform.position.y-startingY-1.1){
            //Debug.Log("fallen off the screen");
            SceneManager.LoadScene("GameOver");
        }
        Vector3 posToFollow = new Vector3(transform.position.x, heighestY + startingY + constCamera, transform.position.z);
        transform.position = Vector3.Lerp(transform.position,posToFollow, followSpeed*Time.deltaTime);

        if(target.transform.position.y > playerScore){
            playerScore = target.transform.position.y;
        }
        scoreBox.text = "Distance: " + playerScore.ToString("F2");
    }
    IEnumerator MoveCamera(){
        while(true){
            yield return new WaitForSeconds(.1f);
            constCamera += cameraChangeAmount;
        }
    }
}
