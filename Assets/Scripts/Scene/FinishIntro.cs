using UnityEngine;
using System.Collections;

namespace RPG {
  public class FinishIntro : MonoBehaviour {

    private AudioSource[] PlayerAudios;
    private AudioSource PlayerInternalAudio;
    private AudioSource PlayerExternalAudio;
    public AudioClip GirlScream;
    public AudioClip GanonLaugh;
    private GameObject Z;
    private GameObject ExitBlocker;

    public ScreenFader SF;

    // Use this for initialization
    void Start() {
      PlayerAudios = GameObject.Find("Player").GetComponents<AudioSource>();
      PlayerInternalAudio = PlayerAudios[0];
      PlayerExternalAudio = PlayerAudios[1];
      SF = GameObject.FindGameObjectWithTag("Fader").GetComponent<ScreenFader>();
      Z = GameObject.Find("Z");
      ExitBlocker = GameObject.Find("DoNotExitHouse");
    }

    // Update is called once per frame
    void Update() {
    }

    private IEnumerator OnTriggerEnter2D(Collider2D EventTrigger) {
      // When a player enters an exit box and they didn't "just" arrive on the scene
        if (EventTrigger.gameObject.CompareTag("Player")) {

        yield return StartCoroutine(SF.FadeToBlack());

        PlayerInternalAudio.clip = GanonLaugh;
        PlayerInternalAudio.Play();

        PlayerExternalAudio.clip = GirlScream;
        PlayerExternalAudio.Play();

        Destroy(Z);

        yield return StartCoroutine(SF.FadeToClear());

        Destroy(ExitBlocker);
        Destroy(this.gameObject);
      }
    }
  }
}