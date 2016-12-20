using UnityEngine;
using System.Collections;

namespace RPG {
  public class Enemy4DirectionPatrol : MonoBehaviour {
    public float moveSpeed = 2f;
    private Rigidbody2D EnemyRigidBody;
    private float walkTime = 0f;
    public float idleTime = 2f;
    private float walkCounter;
    private float idleCounter;
    private Animator Anim;
    private int walkDirection;
    private int walkBackToStartDirection;
    private bool walkingFromStartPos = true;
    public float upPatrolDistance = 4f;
    public float rightPatrolDistance = 8f;
    public float downPatrolDistance = 2f;
    public float leftPatrolDistance = 1f;
    public bool aggroed = false;


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

      if (!aggroed)
      {
        movement(walkDirection);
            if (Anim.GetBool("isWalking") == true)
            {
             decreaseWalkCounter();
            }
            else
            {
            notWalkingSetup();
            }
      }
    }

    /****************************************************************************
  * Choose Direction simply returns a random int between 0-3, this int is used
  * to determine what direction the gameobject will move. 0 is for up, 1 right
  * 2 down and 3 left. 
  * 
  * It also sets the animator controller bool isWalking to true and resets
  * walkCounter.
  * 
  * It also tracks the direction back via the walkBackToStartDirection.
  * 
  * *************************************************************************/
    protected void ChooseDirection() {
      Anim.SetBool("isAttacking", false);
      walkDirection = Random.Range(0, 4);//returns 0, 1, 2 or 3 
      Anim.SetBool("isWalking", true);
      walkingFromStartPos = true;

      if (walkDirection == 0) {
        walkBackToStartDirection = 2;
        walkCounter = upPatrolDistance;
      } else if (walkDirection == 1) {
        walkBackToStartDirection = 3;
        walkCounter = rightPatrolDistance;
      } else if (walkDirection == 2) {
        walkBackToStartDirection = 0;
        walkCounter = downPatrolDistance;
      } else {
        walkBackToStartDirection = 1;
        walkCounter = leftPatrolDistance;
      }

      walkTime = walkCounter;
    }


    protected void decreaseWalkCounter() {
      walkCounter -= Time.deltaTime;

      if (walkCounter < 0) {

        Anim.SetBool("isWalking", false);
        idleCounter = idleTime;
      }
    }

    /***************************************************************************************************************
    * notWalkingSetup() decreases idleCounter and stops all the NPC's movment.
    * If walkingFromStartPos it will reverse the walk direction and the NPC will walk back to start.
    * If not walking from the start posistion (IE at the start position) callls choose direction to reset the patrol 
    ****************************************************************************************************************/
    protected void notWalkingSetup() {

      idleCounter -= Time.deltaTime;
      EnemyRigidBody.velocity = Vector2.zero;

      if (idleCounter < 0) {
        if (walkingFromStartPos) {
          Anim.SetBool("isWalking", true);
          walkCounter = walkTime;
          walkDirection = walkBackToStartDirection;
          walkingFromStartPos = false;
        } else {
          ChooseDirection();
        }
      }
    }

    /***************************************************************************************************************
    * Movement takes a int arguement walkDirection which dictates which way the NPC will move. The NPC's rigidbody is 
    * moved via vector2 and moveSpeed. The Anim.SetFloat set the correct direction for the animation controller so the
    * correct animation will show with the correct direction
    ****************************************************************************************************************/
    protected void movement(int walkDirection) {
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
    }
  }
}
