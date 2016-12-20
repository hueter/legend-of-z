using UnityEngine;
using System.Collections;

namespace RPG {
  public class PGanonMissile : MonoBehaviour {

    public Character Char;
    public Player_Manager PM;
    public PhantomManager PhM;

    public GameObject Player;
    public GameObject ganon;
    private GameObject currentTarget;

    public float distance;
    public float speed;
 
    // Use this for initialization
    void Start() {
      Char = GameObject.Find("PhantomGanon").GetComponent<PhantomManager>();
      Player = GameObject.Find("Player");
      PM = Player.GetComponent<Player_Manager>();
      ganon = GameObject.Find("PhantomGanon");
      PhM = ganon.GetComponent<PhantomManager>();
      currentTarget = Player;
      speed = 0.1f;
    }

    // Update is called once per frame
    void Update() {
      moveTowards();
      distance = Vector3.Distance(transform.position, Player.transform.position);

      if (Input.GetKeyDown(KeyCode.Space) && distance < 3.0f) {
        changeTarget();
        speed = 1f;
      }
    }

    /*
     * Missile moves towards current target
     * */
    public void moveTowards() {
      transform.position = Vector2.MoveTowards(transform.position, currentTarget.transform.position, speed);
    }

    /*
     * Changes target between player and phantomGanon making sure that the collision between
     * ganon and the missle gets activated/deactivated appropriately so that he can be hit/not hit
     * */
    public void changeTarget() {
      PM.PlayerExternalAudio.clip = PhM.ReflectSound;
      PM.PlayerExternalAudio.Play();
      if (currentTarget == Player) {
        currentTarget = ganon;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), Char.GetComponent<Collider2D>(), false);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), Player.GetComponent<Collider2D>());
      } else {
        currentTarget = Player;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), Player.GetComponent<Collider2D>(), false);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), Char.GetComponent<Collider2D>());
      }
    }

    /*
     * Causes damage to player if hit. 
     * Causes Ganon to fall down if hit
     * Causes missile to change current target if colliding with
     * Damagebox or DamageBoxEnemy
     * 
     * */
    protected void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.tag == "Player") {
        collision.gameObject.SendMessage("TakeDamage", Char, SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);
      } else if (collision.gameObject.name == "PhantomGanon") {
        PM.PlayerExternalAudio.clip = PhM.Tappedsound;
        PM.PlayerExternalAudio.Play();
        Char.SendMessage("ShieldFlasher", SendMessageOptions.DontRequireReceiver);
        collision.gameObject.SendMessage("fallDown", SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);
      }

    }
  }
}