using UnityEngine;
using System.Collections;

namespace RPG {
  public class PositionSpawner : MonoBehaviour {

    public GameObject spawnedEnemy;
    public Vector3 spawnCoords;
    public string specifiedName;
    // Use this for initialization
    void Start() {
      
    }

    // Update is called once per frame
    void Update() {


    }

    protected void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.name == "Player") {
        //spawn the enemy at specified coordinates then destroy self
        GameObject newEnemy = (GameObject)Instantiate(spawnedEnemy, spawnCoords, Quaternion.identity);
        newEnemy.name = specifiedName;
        Destroy(this.gameObject);
      }
    }

  }
}