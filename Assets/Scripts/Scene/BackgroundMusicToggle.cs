using UnityEngine;
using System.Collections;

namespace RPG {
  public class BackgroundMusicToggle : ChangeBackgroundMusic {

    private void OnTriggerEnter2D(Collider2D MusicChangeTrigger) {
      // When a player enters an exit box and they didn't "just" arrive on the scene
      if (MusicChangeTrigger.gameObject.CompareTag("Player")) {

        Music.clip = NewMusic;
        Music.Play();
      }
    }
    private void OnTriggerExit2D(Collider2D ExitBox) {
      RevertBackToOldMusic();
    }
  }
}
