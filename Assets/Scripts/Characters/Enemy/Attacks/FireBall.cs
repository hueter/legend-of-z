using UnityEngine;
using System.Collections;

namespace RPG {

  public class FireBall : MonoBehaviour {


    public Character Char;
    public Vector3 playerLocation;

    // Use this for initialization
    void Start() {
      Char = GameObject.Find("ForestBoss").GetComponent<ForestBoss_Manager>();
    }

    // If location of fireBall, is not at last known player location....keep moving it
    void Update() {
      if (transform.position != playerLocation) {
        //Debug.Log("ball should be moving");
        transform.position = Vector2.MoveTowards(transform.position, playerLocation, 0.1f);
      } else {
        Destroy(this.gameObject);
      }
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.tag == "Player") {
        collision.gameObject.SendMessage("TakeDamage", Char, SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);
      }
    }
  }

}