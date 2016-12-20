using UnityEngine;

namespace RPG {
  public class RupeeDrop : MonoBehaviour {
    public RupeeCount RupeeHUD;
    public AudioClip RupeeSound;
    public AudioSource ItemAudio;

    // Use this for initialization
    void Start() {
      RupeeHUD = GameObject.Find("RupeeCount").GetComponent<RupeeCount>();
      ItemAudio = GameObject.Find("Player").GetComponent<Player_Manager>().PlayerInternalAudio;
   }

    // Update is called once per frame
    void Update() {
    }

    void OnCollisionEnter2D(Collision2D collision) {
      //Debug.Log(collision.gameObject);
      if (collision.gameObject.tag == "Player") {
        RupeeHUD.SendMessage("increaseCount", 1);
        ItemAudio.clip = RupeeSound;
        ItemAudio.Play();
        // Debug.Log("rupee picked up");
        Destroy(this.gameObject);
      }
    }
  }
}