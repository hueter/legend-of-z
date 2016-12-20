using UnityEngine;
using System.Collections;

namespace RPG {
  public class ChangeBackgroundMusic : MonoBehaviour {

    protected AudioSource Music;
    public AudioClip DefaultMusic;
    public AudioClip NewMusic;
    public bool triggered;

    // Use this for initialization
    void Start() {
      Music = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
      DefaultMusic = Music.clip;
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnTriggerEnter2D(Collider2D MusicChangeTrigger) {
      // When a player enters an exit box and they didn't "just" arrive on the scene
      if (!triggered) {
        if (MusicChangeTrigger.gameObject.CompareTag("Player")) {

          Music.clip = NewMusic;
          Music.Play();
          triggered = true;
        }
      }
    }

    public void RevertBackToOldMusic() {
      Music.clip = DefaultMusic;
      Music.Play();
    }
  }
}

