using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacles : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private Transform topCheckL;
    [SerializeField]
    private Transform topCheckR;
    private bool isTouchingTopL, isTouchingTopR;
    [SerializeField]
    private float topColLength = .4f;
    [SerializeField]
    private LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {  
        //may want to add one in each corner
        isTouchingTopL = Physics2D.OverlapCircle(topCheckL.position, transform.localScale.x/4, playerLayer);    //topColLength needs to change based on size
        isTouchingTopR = Physics2D.OverlapCircle(topCheckR.position, transform.localScale.x/4, playerLayer);

        if(rb.isKinematic == false && isTouchingTopL || isTouchingTopR){
            Debug.Log("game over");
            SceneManager.LoadScene("GameOver");
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("GroundTag")){
            rb.isKinematic = true;
        }
    }
}
