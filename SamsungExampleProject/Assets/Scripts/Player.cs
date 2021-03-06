using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private float rayCastLength;
    [SerializeField]
    private float frontColLength;
    [SerializeField]
    private Vector3 rayCastOffset;
    [SerializeField]
    private Vector3 attackOffset;
    private bool onGround;
    private bool FacingRight = true;
    private int directionMoving = 0;
    [HideInInspector]
    public bool moveRight = false;
    [HideInInspector]
    public bool moveLeft = false;

    [Header("Movement Variables")]
    [SerializeField]
    private float movementAcc;
    [SerializeField]
    private float maxMoveSpeed;
    [SerializeField]
    private float drag;
    private float hDir;
    private bool changeDir => (rb.velocity.x > 0f && hDir < 0f) || (rb.velocity.x < 0f && hDir > 0f);

    [Header("Jump Variables")]
    [SerializeField]
    private float jumpHeight = 12f;
    [SerializeField]
    private float airDrag = 2.5f;
    [SerializeField]
    private float fallMultiplier = 8f;
    [SerializeField]
    private float lowJumpFallMultiplier = 5;
    [SerializeField]
    private int extraJumpsReset = 1;
    private int extraJumps;
    private bool isTouchingFront;
    [SerializeField]
    private Transform frontCheck;
    private bool wallSliding;
    [SerializeField]
    private float wallSlidingSpeed;
    private bool canJump;
    private bool wallJumping;
    [SerializeField]
    private float xWallForce;
    [SerializeField]
    private float yWallForce;
    public float wallJumpTime;
    private float resetYWallForce;

    private Animator anim;

    private GameObject whichToAttack= null;

    private bool canAttack = true;
    [SerializeField]
    private float attackDelay = .5f;

    [HideInInspector]
    public bool freezeObstacles = false;

    //private int freezePowerUpCount;
    private bool triggerJump = false;
    private bool triggerAttack = false;
    private bool triggerPowerUp = false;

    [SerializeField] private GameObject particleObject;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        resetYWallForce = yWallForce;
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        
        MoveChar();
        if(onGround){
            ApplyDrag();
            extraJumps = extraJumpsReset;
            yWallForce = resetYWallForce;
        }
        else{
            ApplyAirDrag();
            FallMultiplier();
        }
        if(FacingRight == false && hDir > 0){
            Flip();
        }
        else if(FacingRight && hDir < 0){
            Flip();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(MainManager.Instance.betterPlayer == true){
            particleObject.SetActive(true);
        }
        else{
            particleObject.SetActive(false);
        }
        
        canJump = Input.GetKeyDown(KeyCode.Space) || triggerJump && (onGround || extraJumps > 0);
        triggerJump = false;
        //gets user input and checks collision (also plays animations)
       
        if(moveRight){
            hDir = 1;
        }
        else if(moveLeft){
            hDir = -1;
        }
        else{
            hDir = GetInput().x;
        }
        anim.SetFloat("Speed", Mathf.Abs(hDir));
        CheckCollision();
        

        //Wall Sliding
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, frontColLength, ground);
        if(isTouchingFront && onGround == false && hDir != 0){
            wallSliding = true;
        }
        else{
            wallSliding = false;
        }
        if(wallSliding){
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        //wall Jumping
        if(triggerJump && wallSliding == true){
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
            yWallForce/=1.1f;
        }
        if(wallJumping){
            rb.AddForce(new Vector2(xWallForce * -hDir, yWallForce));
            //decrease yWallForce a little bit each time you walljump
            
        }

        //what triggers the jump
        if(canJump && !wallJumping){
            Jump();
        }

        //attacking
        if(whichToAttack != null && triggerAttack && canAttack){
            if(whichToAttack.transform.position.x >-2.5 && whichToAttack.transform.position.x < 2.5){
                canAttack = false;
                StartCoroutine(resetAttack());
                whichToAttack.GetComponent<Obstacles>().health--;
                //Debug.Log(whichToAttack.GetComponent<Obstacles>().health);
                if(whichToAttack.GetComponent<Obstacles>().health <= 0){
                    Destroy(whichToAttack);
                    GetComponent<AudioSource>().Play();
                }
            }
        }
        triggerAttack = false;

        if(triggerPowerUp && MainManager.Instance.freezePowerUpCount > 0){
            freezeObstacles = true;
            MainManager.Instance.freezePowerUpCount--;
        }
        else{
            freezeObstacles = false;
        }
        triggerPowerUp = false;
    }

    IEnumerator resetAttack(){
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    //used to get the input in update
    private static Vector2 GetInput(){
        return new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
    }
    public void jumpButton(){
        triggerJump = true;
    }
    public void attackButton(){
        triggerAttack = true;
    }
    public void powerUpButton(){
        triggerPowerUp = true;
    }
    public void moveR(){
        moveRight = true;
        moveLeft = false;
    }
    public void releaseRight(){
        moveRight = false;
    }
    public void moveL(){
        moveRight = false;
        moveLeft = true;
    }
    public void releaseLeft(){
        moveLeft = false;
    }

    //whats actually moving the character and applying force
    private void MoveChar(){
        rb.AddForce(new Vector2(hDir,0f)*movementAcc);
        /*
        if(Mathf.Abs(rb.velocity.x)>maxMoveSpeed){
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x)*maxMoveSpeed, rb.velocity.y);
        }
        */
    }

    //code to flip the player when they change directions
    private void Flip(){
        FacingRight = !FacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    //drag while on the ground (accelerate, de-accelerate)
    private void ApplyDrag(){
        if(Mathf.Abs(hDir)<.4f || changeDir){
            rb.drag = drag;
        }
        else{
            rb.drag = 0f;
        }
    }

    //the jumping logic (and extra jumping logic)
    private void Jump(){
        if(!onGround){
            --extraJumps;
            //One thing I can do here is reduce the amount you wall jump by each time you wall jump
        }
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up*jumpHeight, ForceMode2D.Impulse);
        
    }

    //checks if player is on the ground
    private void CheckCollision(){
        onGround = Physics2D.Raycast(transform.position + rayCastOffset, Vector2.down, rayCastLength, ground) || Physics2D.Raycast(transform.position - rayCastOffset, Vector2.down, rayCastLength, ground);
        if(transform.localScale.x >= 0){
            if(Physics2D.Raycast(transform.position + attackOffset, Vector2.right, rayCastLength, ground)){
                whichToAttack = Physics2D.Raycast(transform.position + attackOffset, Vector2.right, rayCastLength, ground).transform.gameObject;
            }

        }
        else if(transform.localScale.x < 0){
            if(Physics2D.Raycast(transform.position - attackOffset, Vector2.left, rayCastLength, ground)){
                whichToAttack = Physics2D.Raycast(transform.position - attackOffset, Vector2.left, rayCastLength, ground).transform.gameObject;
            }
        }
        else{
            whichToAttack = null;
        }
        //Debug.Log(whichToAttack);
    }

    //used to see where colliders are while in the editor
    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + rayCastOffset, transform.position + rayCastOffset + Vector3.down * rayCastLength);
        Gizmos.DrawLine(transform.position - rayCastOffset, transform.position - rayCastOffset + Vector3.down * rayCastLength);
        Gizmos.color = Color.red;
        if(transform.localScale.x >= 0){
            Gizmos.DrawLine(transform.position + attackOffset, transform.position + attackOffset + Vector3.right * rayCastLength);
        }
        if(transform.localScale.x < 0){
            Gizmos.DrawLine(transform.position - attackOffset, transform.position - attackOffset + Vector3.left * rayCastLength);
        }
    }
    
    //applies drag in the air
    private void ApplyAirDrag(){
        rb.drag = airDrag;
    }

    //makes the player fall at different speeds based on their height
    private void FallMultiplier(){
        if(rb.velocity.y < 0){
            rb.gravityScale = fallMultiplier;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump")){
            rb.gravityScale = lowJumpFallMultiplier;
        }
        else{
            rb.gravityScale = 1f;
        }
        
    }

    //used to reset wall jumping
    private void SetWallJumpingToFalse(){
        wallJumping = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("FreezePowerUp")){
            MainManager.Instance.freezePowerUpCount++;
            Destroy(other.gameObject);
            
        }
    }
}
