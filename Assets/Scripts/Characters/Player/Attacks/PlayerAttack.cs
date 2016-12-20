using UnityEngine;
using System.Collections;

namespace RPG {
  public class PlayerAttack : MonoBehaviour {

    Animator Anim;
    public GameObject DamageBox;
    public GameObject BombDamageBox;
    public Player_Manager PlayerManager;
    public GameObject Player;
    //Arrow variables
    public Rigidbody2D Arrow;
    public Rigidbody2D Bomb;
    public Vector3 playerPos;
    public Vector3 adjustment;

    //Sound stuff
    public AudioClip HitSound;
    public AudioSource PlayerAudio;

    // used to limit the speed of attacks
    public float nextSwordStrike;
    public float nextBowAttack;
    public float nextBombAttack;

    public Vector3 bombPosition;

    // Use this for initialization
    void Start() {
      Anim = GetComponent<Animator>();
      PlayerManager = GetComponent<Player_Manager>();
      PlayerAudio = PlayerManager.PlayerInternalAudio;
      nextSwordStrike = Time.time;
      nextBowAttack = Time.time;
      nextBombAttack = Time.time;
    }

    // Update is called once per frame
    void Update() {
      Attack();
      bowAttack();
      bombAttack();
    }

    void Attack() {
      playerPos = transform.position;

      //Space is the Attack key. If it is pressed, the animator needs to enable isAttacking 
      if (Input.GetKeyDown(PlayerManager.SwordAttack) && PlayerManager.canSwordAttack && PlayerManager.canAttack && (Time.time >= nextSwordStrike)) {
        Anim.SetBool("isAttacking", true);

        // you have to wait 0.45 seconds til the next sword strike
        nextSwordStrike = Time.time + 0.40f;

        //we need to make sure the hitbox is created in the direction that the Player is currently facing.
        // we take the parameters from the animator and then adjust with .5 float so that the box lines up properly
        if (Anim.GetFloat("lastMove_x") != 0) {
          if (Anim.GetFloat("lastMove_x") > 0)
            adjustment.x = Anim.GetFloat("lastMove_x") + .5f;
          else
            adjustment.x = Anim.GetFloat("lastMove_x") - .5f;
          adjustment.y = 0;
        } else if (Anim.GetFloat("lastMove_y") != 0) {

          if (Anim.GetFloat("lastMove_y") > 0)
            adjustment.y = Anim.GetFloat("lastMove_y") + .5f;
          else
            adjustment.y = Anim.GetFloat("lastMove_y") - .5f;
          adjustment.x = 0;
        }

        //Debug.Log(adjustment);

        // play attack sound
        PlayerAudio.clip = PlayerManager.SwordAttackSound;
        PlayerAudio.Play();

        //Create and instantiate the damage box along with its appropriate coordinates. Then we make sure that the Player and the hitbox can't collide
        GameObject damage = (GameObject)Instantiate(DamageBox, new Vector3(playerPos.x + adjustment.x, playerPos.y + adjustment.y, playerPos.z), transform.rotation);
        damage.name = "DamageBox";
        damage.GetComponent<Damage>().Char = GetComponent<Character>();

        Physics2D.IgnoreCollision(damage.GetComponent<Collider2D>(), GetComponent<Collider2D>());
      } else {
        Anim.SetBool("isAttacking", false);
      }
    }


    void bowAttack() {
      if (Input.GetKeyDown(PlayerManager.BowAttack) && PlayerManager.canBowAttack && PlayerManager.canAttack && (Time.time >= nextBowAttack)) {
        Anim.SetBool("isBowAttacking", true);
        // Debug.Log(Anim.GetFloat("lastMove_x"));
        // Debug.Log(Anim.GetFloat("lastMove_y"));
        // play attack sound
        PlayerAudio.clip = PlayerManager.BowAttackSound;
        PlayerAudio.Play();
        nextBowAttack = Time.time + 0.45f;

        //we need to make sure the hitbox is created in the direction that the Player is currently facing.
        // we take the parameters from the animator and then adjust with .5 float for single directional movement
        //and if diagonal movement detected adjust by .59 on cartarsian graph so that the box lines up properly
        if ((Anim.GetFloat("lastMove_x") > .7) && (Anim.GetFloat("lastMove_y") > .7)) {
          adjustment.x = Anim.GetFloat("lastMove_x") - .59f;
          adjustment.y = Anim.GetFloat("lastMove_y") - .59f;
        } else if ((Anim.GetFloat("lastMove_x") < -.7) && (Anim.GetFloat("lastMove_y") > .7)) {
          adjustment.x = Anim.GetFloat("lastMove_x") + .59f;
          adjustment.y = Anim.GetFloat("lastMove_y") - .59f;
        } else if ((Anim.GetFloat("lastMove_x") < -.7) && (Anim.GetFloat("lastMove_y") < -.7)) {
          adjustment.x = Anim.GetFloat("lastMove_x") + .59f;
          adjustment.y = Anim.GetFloat("lastMove_y") + .59f;
        } else if ((Anim.GetFloat("lastMove_x") > .7) && (Anim.GetFloat("lastMove_y") < -.7)) {
          adjustment.x = Anim.GetFloat("lastMove_x") - .59f;
          adjustment.y = Anim.GetFloat("lastMove_y") + .59f;
        } else if (Anim.GetFloat("lastMove_x") != 0) {
          if (Anim.GetFloat("lastMove_x") > 0)
            adjustment.x = Anim.GetFloat("lastMove_x") + .5f;
          else
            adjustment.x = Anim.GetFloat("lastMove_x") - .5f;
          adjustment.y = 0;
        } else if (Anim.GetFloat("lastMove_y") != 0) {

          if (Anim.GetFloat("lastMove_y") > 0)
            adjustment.y = Anim.GetFloat("lastMove_y") + .5f;
          else
            adjustment.y = Anim.GetFloat("lastMove_y") - .5f;
          adjustment.x = 0;
        }

        //Debug.Log(adjustment);

        //Create and instantiate the damage box along with its appropriate coordinates. Then we make sure that the Player and the hitbox can't collide
        Rigidbody2D arrowInstance = Instantiate(Arrow, new Vector3(playerPos.x + adjustment.x, playerPos.y + adjustment.y, playerPos.z), transform.rotation) as Rigidbody2D;
        Physics2D.IgnoreCollision(arrowInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
      } else {
        Anim.SetBool("isBowAttacking", false);
      }
    }
    void bombAttack()
    {
            if (Input.GetKeyDown(PlayerManager.BombAttack) && PlayerManager.canBombAttack && PlayerManager.canAttack && (Time.time >= nextBombAttack))
            {

                if ((Anim.GetFloat("lastMove_x") > .7) && (Anim.GetFloat("lastMove_y") > .7))
                {
                    adjustment.x = Anim.GetFloat("lastMove_x") - .59f;
                    adjustment.y = Anim.GetFloat("lastMove_y") - .59f;
                }
                else if ((Anim.GetFloat("lastMove_x") < -.7) && (Anim.GetFloat("lastMove_y") > .7))
                {
                    adjustment.x = Anim.GetFloat("lastMove_x") + .59f;
                    adjustment.y = Anim.GetFloat("lastMove_y") - .59f;
                }
                else if ((Anim.GetFloat("lastMove_x") < -.7) && (Anim.GetFloat("lastMove_y") < -.7))
                {
                    adjustment.x = Anim.GetFloat("lastMove_x") + .59f;
                    adjustment.y = Anim.GetFloat("lastMove_y") + .59f;
                }
                else if ((Anim.GetFloat("lastMove_x") > .7) && (Anim.GetFloat("lastMove_y") < -.7))
                {
                    adjustment.x = Anim.GetFloat("lastMove_x") - .59f;
                    adjustment.y = Anim.GetFloat("lastMove_y") + .59f;
                }
                else if (Anim.GetFloat("lastMove_x") != 0)
                {
                    if (Anim.GetFloat("lastMove_x") > 0)
                        adjustment.x = Anim.GetFloat("lastMove_x") + .5f;
                    else
                        adjustment.x = Anim.GetFloat("lastMove_x") - .5f;
                    adjustment.y = 0;
                }
                else if (Anim.GetFloat("lastMove_y") != 0)
                {

                    if (Anim.GetFloat("lastMove_y") > 0)
                        adjustment.y = Anim.GetFloat("lastMove_y") + .5f;
                    else
                        adjustment.y = Anim.GetFloat("lastMove_y") - .5f;
                    adjustment.x = 0;
                }

                PlayerAudio.clip = PlayerManager.BombAttackSound;
                PlayerAudio.Play();
                nextBombAttack = Time.time + 3.45f;
                bombPosition = new Vector3(playerPos.x + adjustment.x, playerPos.y + adjustment.y, playerPos.z);
                Rigidbody2D.Instantiate(Bomb, bombPosition , transform.rotation);

            }
    }

        public void bombExplode()
        {
            PlayerAudio.clip = PlayerManager.BombExplodeSound;
            PlayerAudio.Play();
            GameObject damage = (GameObject)Instantiate(BombDamageBox, bombPosition, transform.rotation);
            damage.GetComponent<Damage>().Char = GetComponent<Character>();
        }
    }
}