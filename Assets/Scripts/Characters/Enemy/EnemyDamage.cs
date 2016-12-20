using UnityEngine;
using System.Collections;


namespace RPG {
  public class EnemyDamage : MonoBehaviour {

    public int countdown;
    public Character Char;

    // Use this for initialization
    void Start() {
      countdown = 5;
 
    }

    // Update is called once per frame
    void Update() {

      if (countdown == 0) {
        Destroy(this.gameObject);
      } else {
        countdown--;
      }
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.tag == "Player") {
        
        // var damageDone = GameObject.Find("GameManager").GetComponent<SimpleGameManager>().damage;
        collision.gameObject.SendMessage("TakeDamage", Char);
      }
    }
  }
}