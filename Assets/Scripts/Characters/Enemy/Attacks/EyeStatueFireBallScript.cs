using UnityEngine;
using System.Collections;
namespace RPG {
  public class EyeStatueFireBallScript : MonoBehaviour {


    public Character Link;
    Vector2 CurrentDirection;
    Rigidbody2D FireBallRigidBody;
    public float FireBallSpeed;  //set via inspector
    private Ganon_Enemy_Manager G;


    // Use this for initialization
    void Start() {
      G = GameObject.Find("Ganon").GetComponent<Ganon_Enemy_Manager>();
      Link = GameObject.Find("Player").GetComponent<Character>();
      FireBallRigidBody = GetComponent<Rigidbody2D>();
      CurrentDirection = FindDirectionOfLink();
    }

    // Update is called once per frame
    void Update() {
      if (G.IntroComplete & !G.defeated) {
        MoveFireBall(CurrentDirection.normalized);
      }
      if (G.defeated) {
        Destroy(this.gameObject);
      }
    }


    private Vector2 FindDirectionOfLink() {
      Vector2 fromPosition = transform.position;
      Vector2 toPosition = Link.transform.position;
      Vector2 Linksdirection = toPosition - fromPosition;
      return Linksdirection;
    }

    private void MoveFireBall(Vector2 Direction) {

      FireBallRigidBody.velocity = Direction * FireBallSpeed;
    }


    void OnTriggerEnter2D(Collider2D other) {

      //Debug.Log("in on trigger in fireball script tag ==" + other.gameObject);

      if (other.gameObject.tag == "Player") {
        other.gameObject.SendMessage("TakeDamage", Link, SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);
      } else if ((other.gameObject.tag == "EyeStatue") || (other.gameObject.tag == "GanonWallRight") || (other.gameObject.tag == "GanonWallUp") || (other.gameObject.tag == "GanonWallDown") || (other.gameObject.tag == "GanonWallLeft")) {
        return;
      } else if (other.gameObject.tag == "Ganon") {
        other.gameObject.SendMessage("TakeFireballDamage", Link, SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);
      } else {
        Destroy(this.gameObject);
      }
    }
  }
}