using UnityEngine;
using System.Collections;


namespace RPG {

  public class KnightMovementScript : MonoBehaviour {
    //might not ever be used again comment
    public float moveSpeed = 2f;
    private Rigidbody2D EnemyRigidBody;
    public float walkTime = 2f;
    public float idleTime = 0f;
    private float walkCounter;
    private float idleCounter;
    private Animator Anim;
    private int walkDirection;

    // Use this for initialization
    void Start() {

      //get rigidbody attched to game object to move gameobject
      EnemyRigidBody = GetComponent<Rigidbody2D>();

      //get animation controller for game object
      Anim = GetComponent<Animator>();


      walkCounter = walkTime;
      idleCounter = idleTime;
      ChooseDirection();

    }

    // Update is called once per frame
    void Update() {

      //switch based on walkDirection which is randomly generated via chooseDirection() 
      switch (walkDirection) {
        case 0://movement up
          EnemyRigidBody.velocity = new Vector2(0, moveSpeed);
          Anim.SetFloat("direction_x", 0f);
          Anim.SetFloat("direction_y", 1.0f);
          break;

        case 1://movement right
          EnemyRigidBody.velocity = new Vector2(moveSpeed, 0);
          Anim.SetFloat("direction_x", 1.0f);
          Anim.SetFloat("direction_y", 0f);
          break;

        case 2://movement down
          EnemyRigidBody.velocity = new Vector2(0, -moveSpeed);
          Anim.SetFloat("direction_x", 0.0f);
          Anim.SetFloat("direction_y", -1.0f);
          break;

        case 3://movement left
          EnemyRigidBody.velocity = new Vector2(-moveSpeed, 0);
          Anim.SetFloat("direction_y", 0.0f);
          Anim.SetFloat("direction_x", -1.0f);
          break;

      }

      if (Anim.GetBool("isWalking") == true) {

        walkCounter -= Time.deltaTime;

        if (walkCounter < 0) {

          Anim.SetBool("isWalking", false);
          idleCounter = idleTime;
        }

      } else {

        Anim.SetBool("isWalking", false);
        idleCounter -= Time.deltaTime;
        EnemyRigidBody.velocity = Vector2.zero;

        if (idleCounter < 0) {
          Attack();
          //After Attack animation plays, choose direction is called from the animator controller

        }
      }
    }

    /****************************************************************************
  * Choose Direction simply returns a random int between 0-3, this int is used
  * to determine what direction the gameobject will move. 0 is for up, 1 right
  * 2 down and 3 left. 
  * 
  * It also set the animator controller bool isWalking to true and resets
  * walkCounter
  * *************************************************************************/
    public void ChooseDirection() {
      Anim.SetBool("isAttacking", false);
      walkDirection = Random.Range(0, 4);
      Anim.SetBool("isWalking", true);
      walkCounter = walkTime;
    }

    public void Attack() {

      Anim.SetBool("isAttacking", true);

    }
  }

}