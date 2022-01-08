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
    private bool onGround;
    private bool FacingRight = true;

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
    private bool canJump => Input.GetButtonDown("Jump") && (onGround || extraJumps > 0);
    private bool wallJumping;
    [SerializeField]
    private float xWallForce;
    [SerializeField]
    private float yWallForce;
    public float wallJumpTime;
    private float resetYWallForce;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        resetYWallForce = yWallForce;
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
        hDir = GetInput().x;
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
        if(Input.GetButtonDown("Jump") && wallSliding == true){
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
            yWallForce/=1.1f;
        }
        if(wallJumping){
            rb.AddForce(new Vector2(xWallForce * -hDir, yWallForce));
            //decrease yWallForce a little bit each time you walljump
            
        }

        if(canJump && !wallJumping){
            Jump();
        }
    }
    private static Vector2 GetInput(){
        return new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
    }
    
    private void MoveChar(){
        rb.AddForce(new Vector2(hDir,0f)*movementAcc);
        if(Mathf.Abs(rb.velocity.x)>maxMoveSpeed){
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x)*maxMoveSpeed, rb.velocity.y);
        }
    }

    private void Flip(){
        FacingRight = !FacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void ApplyDrag(){
        if(Mathf.Abs(hDir)<.4f || changeDir){
            rb.drag = drag;
        }
        else{
            rb.drag = 0f;
        }
    }

    private void Jump(){
        if(!onGround){
            --extraJumps;
            //One thing I can do here is reduce the amount you wall jump by each time you wall jump
        }
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up*jumpHeight, ForceMode2D.Impulse);
    }

    private void CheckCollision(){
        onGround = Physics2D.Raycast(transform.position + rayCastOffset, Vector2.down, rayCastLength, ground) || Physics2D.Raycast(transform.position - rayCastOffset, Vector2.down, rayCastLength, ground);
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + rayCastOffset, transform.position + rayCastOffset + Vector3.down * rayCastLength);
        Gizmos.DrawLine(transform.position - rayCastOffset, transform.position - rayCastOffset + Vector3.down * rayCastLength);
    }

    private void ApplyAirDrag(){
        rb.drag = airDrag;
    }

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

    private void SetWallJumpingToFalse(){
        wallJumping = false;
    }
}
