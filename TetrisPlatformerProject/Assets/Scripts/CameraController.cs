using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float followSpeed;
    private float startingY;
    private float heighestY;
    // Start is called before the first frame update
    void Start()
    {
        startingY = transform.position.y-target.transform.position.y;
        heighestY = target.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(target.transform.position.y > heighestY && target.transform.position.y >= transform.position.y+2){
            heighestY = (target.transform.position.y-transform.position.y)/2;
            Debug.Log(heighestY+startingY);
        
        }
        if(target.transform.position.y < transform.position.y-startingY-.2){
            //Debug.Log("fallen off the screen");
        }
        Vector3 posToFollow = new Vector3(transform.position.x, heighestY + startingY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position,posToFollow, followSpeed*Time.deltaTime);
    }
}
