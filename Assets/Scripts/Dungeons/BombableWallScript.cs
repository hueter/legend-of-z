using UnityEngine;
using System.Collections;

public class BombableWallScript : MonoBehaviour {

  // Use this for initialization
  void Start() {
  }

  // Update is called once per frame
  void Update() {

  }

  void OnTriggerEnter2D(Collider2D coll) {
    if (coll.gameObject.tag == "BombDamageBox") {
      DestroyObject(coll.gameObject);
      DestroyObject(this.gameObject);

    }
  }
}