using UnityEngine;
using System.Collections;

namespace RPG {

  public class PlayerMovement : MonoBehaviour {

    //Variables of types Rigidbody and Animator 
    private Rigidbody2D RBody;
    private Animator Anim;

    //Player Manager
    private Player_Manager PlayerManager;

    //Variable for Player movement speed

    public float playerSpeed;
    Vector2 movementVector;


    

    // Use this for initialization
    void Start() {

      //These variables line up with the components created for the actual Player object in the Unity hierarchy  

      PlayerManager = GetComponent<Player_Manager>();
      RBody = GetComponent<Rigidbody2D>();
      Anim = GetComponent<Animator>();
      
      // set initial speed to default and current speed to default
      playerSpeed = PlayerManager.defaultMoveSpeed * Time.deltaTime;
      PlayerManager.currentMoveSpeed = PlayerManager.defaultMoveSpeed + 1f;

    }

    // Update is called once per frame
    void Update() {
        
      if (PlayerManager.canMove) {
        //Gets Player input. GetAxisRaw returns true or false, GetAxis allows floating point precision which we dont need for this level of movement
        movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementVector.Normalize(); // magnitude set at 1 always so diagonal movement isn't faster
        // set updated speed to currentSpeed
        playerSpeed = PlayerManager.currentMoveSpeed * Time.deltaTime;


        if (Anim) {

          //Checks to see if movement vector is equal to zero. If not zero, means we set the animator to walking else dont set to walking
          if (movementVector != Vector2.zero) {
            Anim.SetBool("isWalking", true);

            //Updates the direction so that we don't snap back to original position 
            Anim.SetFloat("input_x", movementVector.x);
            Anim.SetFloat("input_y", movementVector.y);
            Anim.SetFloat("lastMove_x", movementVector.x);
            Anim.SetFloat("lastMove_y", movementVector.y);
          } else {
            Anim.SetBool("isWalking", false);
          }

        }
      }

    }
    void FixedUpdate() {
      //Sets amount of movement. Position + the direction the Player is pressing via input * the deltaTime    Time.deltaTime;
      if (PlayerManager.canMove) {
        RBody.MovePosition(RBody.position + (movementVector * playerSpeed));
      }
    }
  }
}