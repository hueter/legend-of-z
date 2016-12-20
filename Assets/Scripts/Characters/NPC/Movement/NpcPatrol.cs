using UnityEngine;
using System.Collections;

namespace RPG {

  public class NpcPatrol : MonoBehaviour {
    Animator Anim;

    //track speed and whether walking is currently happening
    public float walkSpeed = 2.0f;
    public bool walking;

    //Two units that track frames in walking/idle 
    private int unitsToWalk = 120;
    private int unitsIdle = 120;

    //NPC moves along x axis for now, this is used to track which was last so it alternates
    public float lastDirection = -1.0f;

    //Vector3 is used because that the method of movement (transform.Translate()) requires it
    Vector3 walkAmount;

    // Use this for initialization
    void Start() {
      walking = false;
      Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
      //patrol();
    }

    /****************************************************************************
     * Sets walking movement based on speed, direction, and time(this is passed to
     * transform later). Counters track while walking and idle and will alternate/reset
     * counters when appropriate. We use the Animator components parameters direction_x
     * ,direction_y so that the animations happen when facing the correct direction. 
     * The isWalking parameter is just like the Player one and makes sure the animator
     * switches appropriately between idle and walking states
     * *************************************************************************/
    public void patrol() {
      walkAmount.x = lastDirection * walkSpeed * Time.deltaTime;
      //Debug.Log(lastDirection);

      if (walking) {

        transform.Translate(walkAmount);

        unitsToWalk--;
        //Debug.Log(unitsToWalk);
      } else {
        if (unitsIdle != 0) {
          Anim.SetBool("isWalking", false);
          unitsIdle--;
        } else {
          unitsIdle = Random.Range(100, 200);
          lastDirection = -lastDirection;
          Anim.SetBool("isWalking", true);
          Anim.SetFloat("direction_y", 0);
          Anim.SetFloat("direction_x", lastDirection);
          unitsToWalk = 120;
          walking = true;
        }
      }

      if (unitsToWalk <= 0) {
        walking = false;
        Anim.SetBool("isWalking", false);
      }
    }
  }
}