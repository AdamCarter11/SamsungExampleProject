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
    public int health = 2;
    private bool canHurt = true;
    private Renderer render;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<Renderer>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {  
        //used to create invisible colliders at base of block (instead of all around)
        isTouchingTopL = Physics2D.OverlapCircle(topCheckL.position, transform.localScale.x/4, playerLayer);    //topColLength needs to change based on size
        isTouchingTopR = Physics2D.OverlapCircle(topCheckR.position, transform.localScale.x/4, playerLayer);

        //checks if the bottom of the obstacle collides with player
        if(canHurt && (isTouchingTopL || isTouchingTopR)){
            Debug.Log("game over");
            SceneManager.LoadScene("GameOver");
        }

        if(render.isVisible && player.GetComponent<Player>().freezeObstacles){
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            //block is being seen by camera
            canHurt = false;
        }
    }

    //once block collides with ground or other block, it becomes un-movable (and can't harm the player)
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("GroundTag")){
            canHurt = false;
        }
    }

    //destroys blocks when they leave camera
    private void OnBecameInvisible() {
        //Destroy(gameObject); //commented out until I can figure out how to freeze the bottom row so the blocks don't keep falling
    }
}
