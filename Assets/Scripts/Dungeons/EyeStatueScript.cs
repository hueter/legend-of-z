using UnityEngine;
using System.Collections;

namespace RPG {
  public class EyeStatueScript : MonoBehaviour {

    public Rigidbody2D Fireball;
    public Character Link;
    private Ganon_Enemy_Manager G;
    Animator anim;
    float CanShootFireBallAgainTime;
    public float TimeBetweenFireBalls;

    // Use this for initialization
    void Start() {
      anim = GetComponent<Animator>();
      Link = GameObject.Find("Player").GetComponent<Character>();
      G = GameObject.Find("Ganon").GetComponent<Ganon_Enemy_Manager>();
    }

    // Update is called once per frame
    void Update() {
      if (G.IntroComplete && !G.defeated) {
        //math to set eye animation pointing towards player which is passed to animator controller
        Vector3 vectorToTarget = (Link.gameObject.transform.position - transform.position);
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        anim.SetFloat("LinksAngle", angle);

        if (CanShootFireBallAgainTime < Time.time) {
          spawnFireBall();
          CanShootFireBallAgainTime = Time.time + TimeBetweenFireBalls + Random.Range(.0f, 2.9f);
        }
        if (G.defeated) {
          Destroy(this.gameObject);
        }
      }
    }

    private void spawnFireBall() {

      Rigidbody2D.Instantiate(Fireball, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
    }

    void OnTriggerEnter2D(Collider2D coll) {
      if (coll.gameObject.tag == "BombDamageBox") {
        DestroyObject(coll.gameObject);
        GameObject.Instantiate(Resources.Load("Heart"), transform.position, Quaternion.identity);
        DestroyObject(this.gameObject);
      }
    }
  }
}