using UnityEngine;
using System.Collections;

namespace RPG {

  public class MagicMissle : MonoBehaviour {


    public Character Char;

    // Use this for initialization
    void Start() { 
      Char = GameObject.Find("ForestBoss").GetComponent<ForestBoss_Manager>();
      Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.Find("Wall").GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update() {

    }

    protected void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.tag == "Player") {
        collision.gameObject.SendMessage("TakeDamage", Char, SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);
      } else {
        Destroy(this.gameObject);
      }
    }
  }

}