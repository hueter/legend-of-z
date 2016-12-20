using UnityEngine;
using System.Collections.Generic;

namespace RPG {
  public class EnemyChase : MonoBehaviour {

    [Tooltip("How far the mob can see before aggroing on the CharacterType")]
    public float aggroDistance = 5f;
    private Animator Anim;
    private Character Char;
    private GameObject Player;
    private float xAxisDistanceToPlayer;
    private float yAxisDistanceToPlayer;
    //used for chase/Attack scripts
    private float distance;

    void Start() {
      // Get the Character component.
      Anim = GetComponent<Animator>();
      Char = GetComponent<Character>();
      Player = GameObject.Find("Player");
    }

    void Update() {

    }

    //<summary>
    //Character will move towards Player based on movement speed. Also sets animator to correct animation
    public void Chase() {
      // IF this Character is able to move.
      if (GetComponent<Character>().canMove) {


        Anim.SetBool("isWalking", true);

        //stop Attack animation when chasing
        Anim.SetBool("isAttacking", false);

        //used to determine what way enemy should be facing
        xAxisDistanceToPlayer = Player.transform.position.x - transform.position.x;
        yAxisDistanceToPlayer = Player.transform.position.y - transform.position.y;
        //This makes sure that the Character animations stay in the correct direction while chasing

        if (Mathf.Abs(xAxisDistanceToPlayer) > (Mathf.Abs(yAxisDistanceToPlayer))) {

          if (Player.transform.position.x - transform.position.x > 0) {
            Anim.SetFloat("direction_x", 1.0f);
            Anim.SetFloat("direction_y", 0f);
          } else {
            Anim.SetFloat("direction_x", -1.0f);
            Anim.SetFloat("direction_y", 0f);
          }

          transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, Time.deltaTime * Char.currentMoveSpeed);
        } else {

          if (Player.transform.position.y - transform.position.y > 0) {
            Anim.SetFloat("direction_y", 1.0f);
            Anim.SetFloat("direction_x", 0f);
          } else {
            Anim.SetFloat("direction_y", -1.0f);
            Anim.SetFloat("direction_x", 0f);
          }

          transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, Time.deltaTime * Char.currentMoveSpeed);
        }
      }
    }
  }
}
