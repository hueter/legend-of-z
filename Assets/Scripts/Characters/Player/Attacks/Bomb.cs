using UnityEngine;
using System.Collections;

namespace RPG {
  public class Bomb : MonoBehaviour {

    private GameObject BombDamageBox;
    private GameObject Player;
    private AudioSource Fuse;

    // Use this for initialization
    void Start() {
      Player = GameObject.FindWithTag("Player");
      Player = GameObject.Find("Player");
      Fuse = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void DestroyBomb() {
      DestroyObject(this.gameObject);
    }

    public void Explode() {
      Fuse.Stop();
      Player.GetComponent<PlayerAttack>().bombExplode();
    }
  }
}