using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] Vector2 deathkick = new Vector2(5f,5f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    float defaultGravity;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    bool isAlive = true;
    float defaultRunSpeed;
    float defaultJumpSpeed;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        defaultGravity = myRigidbody.gravityScale;
        myFeetCollider= GetComponent<BoxCollider2D>();
        defaultRunSpeed = runSpeed;
        defaultJumpSpeed = jumpSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive) return;
        
        Run();
        FlipSprite();
        ClimbLadder();
        Swim();
        Die();
    }

    void OnMove(InputValue value){
        if(!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value){
        if(!isAlive) return;
        if(value.isPressed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
        
    }

    void OnFire(InputValue value){
        if(!isAlive) return;
        if(myAnimator.GetBool("isClimbing")) return;
        Instantiate(bullet,gun.position,transform.rotation);
    }
    void Run(){
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed,myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning",playerHasHorizontalSpeed);
    }
    void FlipSprite(){
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed){
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x),1f);
        }
    }
    void ClimbLadder(){
        myAnimator.SetBool("isClimbing",myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")));
        if(!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))){
            myRigidbody.gravityScale = defaultGravity;
            return;
        }
        myRigidbody.gravityScale = 0;
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x,moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        bool playerHasVerticallSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            myAnimator.SetBool("isClimbing",playerHasVerticallSpeed);
        }
        
    }

    void Swim(){
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Water"))){
            runSpeed = defaultRunSpeed/2;
            jumpSpeed = defaultJumpSpeed/1.2f;
            myRigidbody.gravityScale = 1.5f;
        }
        else{
            runSpeed = defaultRunSpeed;
            jumpSpeed = defaultJumpSpeed;
            myRigidbody.gravityScale = 2.5f;
        }
    }
    void Die(){
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies" , "Hazard"))){
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathkick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        
    }
}
