using UnityEngine;
using System.Collections;

namespace RPG {
  public class BreakPot : MonoBehaviour {

    public AudioClip BreakPotSound;
    private Player_Manager PM;

    // Use this for initialization
    void Start() {
      PM = GameObject.Find("Player").GetComponent<Player_Manager>();
    }

    // Update is called once per frame
    void Update() {
   
    }
    
    protected void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.name == "DamageBox") {
        GameObject.Instantiate(Resources.Load("Heart"), transform.position, Quaternion.identity);
        PM.PlayerExternalAudio.clip = BreakPotSound;
        PM.PlayerExternalAudio.Play();
        Destroy(this.gameObject);
      }
    }
  }
}