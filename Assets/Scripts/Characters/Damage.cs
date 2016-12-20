using UnityEngine;
using System.Collections;


namespace RPG {
  public class Damage : MonoBehaviour {

    public int countdown;
    public Character Char;

    // Use this for initialization
    void Start() {
      countdown = 1;
      //Debug.Log("object created");
      //Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update() {

      if (countdown == 0) {
        Destroy(this.gameObject);
      } else {
        countdown--;
      }
    }

    void OnCollisionEnter2D(Collision2D collision) {
      //Debug.Log(collision.gameObject);
      if (collision.gameObject.tag != "NPC") {
        collision.gameObject.SendMessage("TakeDamage", Char, SendMessageOptions.DontRequireReceiver);
      }
    }
  }
}