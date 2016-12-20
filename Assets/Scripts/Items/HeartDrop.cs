using UnityEngine;
using System.Collections;

namespace RPG {

  public class HeartDrop : MonoBehaviour {

    public AudioSource ItemAudio;
    public AudioClip HeartSound;

    void Start() {
      ItemAudio = GameObject.Find("Player").GetComponent<Player_Manager>().PlayerInternalAudio;
    }

    protected void OnCollisionEnter2D(Collision2D collision) {

      //Debug.Log(collision.gameObject);
      if (collision.gameObject.tag == "Player") {
        ItemAudio.clip = HeartSound;
        ItemAudio.Play();
        collision.gameObject.SendMessage("AddHealth", 1);
        // Debug.Log("Health picked up");
        Destroy(this.gameObject);
      }
    }
  }
}