using UnityEngine;
using System.Collections;

namespace RPG {
  public class ArrowBlocker : MonoBehaviour {

    public GameObject Player;

    // Use this for initialization
    void Start() {
      Player = GameObject.Find("Player");
      Physics2D.IgnoreCollision(Player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update() {

    }



  }

}