using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG {
  public class MinotaurBossIntro : MonoBehaviour {

    private GameObject Player;
    private Player_Manager PM;
    public GameObject Boss;
    private Minotaur_Enemy_Manager MEM;
    public ChangeBackgroundMusic BossMusicTrigger;
    private CameraFollow MainCamera;
    private GameObject BossText;
    private Text BossTextText;

    //new stuff
    public bool triggered = false;

    // Use this for initialization
    void Start() {
      Boss = GameObject.Find("Minotaur");
      MEM = Boss.GetComponent<Minotaur_Enemy_Manager>();
      BossMusicTrigger = GameObject.Find("BossMusicTrigger").GetComponent<ChangeBackgroundMusic>();
      MainCamera = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
      Player = GameObject.Find("Player");
      PM = Player.GetComponent<Player_Manager>();
      BossText = GameObject.Find("BossText");
      BossTextText = BossText.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
      if (MEM.defeated) {
        BossMusicTrigger.RevertBackToOldMusic();
        Destroy(this.gameObject);
      }
    }

    IEnumerator StartFight() {
      yield return new WaitForSeconds(1);
      BossTextText.text = "Minos";
      yield return new WaitForSeconds(3);
      //Destroy(BossText);
      BossTextText.text = null;
      MainCamera.Target = Player.transform;
      PM.canAttack = true;
      PM.canMove = true;
      yield return new WaitForSeconds(2);
      MEM.IntroComplete = true;
    }

    private void OnTriggerEnter2D(Collider2D PlayerCollider) {
      if (PlayerCollider.gameObject.CompareTag("Player") && !triggered) {
        // camera goes up to boss
        PM.canAttack = false;
        PM.canMove = false;
        MainCamera.Target = Boss.transform;
        StartCoroutine(StartFight());
        triggered = true;
      }
    }
  }
}
