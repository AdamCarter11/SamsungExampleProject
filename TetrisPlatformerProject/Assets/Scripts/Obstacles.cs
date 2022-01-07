using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacles : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private Transform topCheck;
    private bool isTouchingTop;
    private float topColLength = .01f;
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
        isTouchingTop = Physics2D.OverlapCircle(topCheck.position, topColLength, playerLayer);
        if(rb.isKinematic == false && isTouchingTop){
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
