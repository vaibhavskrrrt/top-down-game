using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// takes input and handles movement of player 

public class PlayerController : MonoBehaviour
{
    public float moveSpeed= 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate(){
        if (canMove){
        // if movement input is not 0, try to move

        if(movementInput !=Vector2.zero){
            bool success = TryMove(movementInput);
            if (!success ){
                success = TryMove(new Vector2(movementInput.x,0));

           
                

            }

             if (!success && movementInput.y>0){
                success = TryMove(new Vector2(0,movementInput.y));
            }

            animator.SetBool("IsMoving", success);
        } else{
            animator.SetBool("IsMoving", false);
        
         
              }
              //set the direction of sprite to  movrment direction
              if(movementInput.x<0){
              spriteRenderer.flipX = true;
             
                                 }
              else if (movementInput.x >0){
              spriteRenderer.flipX = false;
              
                                 }
    }


    }
    private bool TryMove(Vector2 direction) {
        if (direction != Vector2.zero){
         // check for  potential collisions
          int count = rb.Cast(
            direction,// x and y values b/w -1 to 1 that represent the direction from the body to look for collisions
            movementFilter,//the settings that determine where a collision can occur on such as layer to collide with
            castCollisions,// List of colisions to store the found collisions into after thhe Cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset);// the amount is cast equal to the movement plus an offset
          
          if(count==0){
          rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
          return true;
                      } 
          else {
            //can't move if there's no direction to move in
            return false;
          } 
    } else {
        return false;
    }}

    void OnMove(InputValue movementValue){
        movementInput = movementValue.Get<Vector2>();

    }


    void OnFire(){
                  animator.SetTrigger("swordAttack"); 
                 }

    public void SwordAttack(){
        LockMovement();
        if(spriteRenderer.flipX == true){
        swordAttack.AttackLeft();
        }
        else{

        swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack(){
        UnlockMovement();
        swordAttack.StopAttack();
    }
    public void LockMovement(){
        canMove = false;

    }
    public void UnlockMovement(){
        canMove = true;

    }
}
