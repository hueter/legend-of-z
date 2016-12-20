using UnityEngine;
using System.Collections;

namespace RPG {

  public class TentacleAttack : MonoBehaviour {

    public ForestBoss_Manager boss;
    private float lifeTimer;

    // Use this for initialization
    void Start() {
      boss = GameObject.Find("ForestBoss").GetComponent<ForestBoss_Manager>();
      lifeTimer = 90;
    }

    // Update is called once per frame
    void Update() {
      //This timer is the time of the animation to finish. Destroys itself after time is finished
      if (lifeTimer <= 0) {
        destroyTentacle();
      } else {
        lifeTimer--;
      }
    }

    void OnCollisionEnter2D(Collision2D collision) {
      //Debug.Log(collision.gameObject);
      if (collision.gameObject.tag == "Player") {
        //same old TakeDamage sequence based on ForestBoss's damage
        collision.gameObject.SendMessage("TakeDamage", boss, SendMessageOptions.DontRequireReceiver);
      }
    }

    //Destroys tentacle
    void destroyTentacle() {
      Destroy(this.gameObject);
    }
  }
}