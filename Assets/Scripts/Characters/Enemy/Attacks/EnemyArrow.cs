using UnityEngine;
using System.Collections;


namespace RPG {

  public class EnemyArrow : MonoBehaviour {

    public float speed = 20f;
    public Character Char;
    public Animator anim;

    // Use this for initialization
    void Start() {

      if (GameObject.Find("Wall") != null) {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.Find("Wall").GetComponent<Collider2D>());
      }
      Rigidbody2D ArrowRigidBody = GetComponent<Rigidbody2D>();

      float angle;

      /****************************************************************************************
       * This code will change the rotation of the arrow sprite to match the players direction.
       * The arrow will also be given a velocity based on the the direction and speed
       * 
       * We use the Player's animator parameters lastMove_x and lastMove_y
       * 
       * Since down is the base sprite direction, it doesnt require anything besides a 
       * velocity 
       * *****************************************************************************/
      if (anim.GetFloat("direction_y") > 0) {
        ArrowRigidBody.velocity = Vector2.up * speed;
        angle = Mathf.Atan2(Vector2.up.y, Vector2.up.x - 90) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
      } else if (anim.GetFloat("direction_y") < 0) {
        ArrowRigidBody.velocity = Vector2.down * speed;

      } else if (anim.GetFloat("direction_x") > 0) {
        ArrowRigidBody.velocity = Vector2.right * speed;
        angle = Mathf.Atan2(Vector2.right.y + 90, Vector2.right.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
      } else {
        ArrowRigidBody.velocity = Vector2.left * speed;
        angle = Mathf.Atan2(Vector2.left.y - 90, Vector2.left.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
      }
    }

    // Update is called once per frame
    void Update() {
    }

    /*****************************************************************
     * We need the arrow to destroy itself upon impact with another collider 
     * 
     * This will also be the place where we do damage to the enemy it hits
     * 
     * ************************************************************/
    protected void OnCollisionEnter2D(Collision2D collision) {
      Destroy(this.gameObject);

      if (collision.gameObject.tag != "NPC") {
        collision.gameObject.SendMessage("TakeDamage", Char, SendMessageOptions.DontRequireReceiver);
      }
    }
  }
}