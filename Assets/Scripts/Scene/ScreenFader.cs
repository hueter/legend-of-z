using UnityEngine;
using System.Collections;

namespace RPG {

  public class ScreenFader : MonoBehaviour {
    Animator Anim;
    bool isFading = false; //variable used in animation controller

    // Use this for initialization
    void Start() {
      Anim = GetComponent<Animator>();
    }
    //IEnumerator is a behind the scences enumarator. While fading animation, return a null referance to what routine is next.
    public IEnumerator FadeToClear() {
      isFading = true;
      Anim.SetTrigger("FadeIn");

      while (isFading) {
        yield return null;//yield makes other threads/routines pause while it is running
      }
      AnimationComplete();
    }

    //IEnumerator is a behind the scences enumarator. While fading animation, return a null referance to what routine is next.
    public IEnumerator FadeToBlack() {
      isFading = true;
      Anim.SetTrigger("FadeOut");

      while (isFading) {
        yield return null;//yield makes other threads/routines pause while it is running
      }
      AnimationComplete();
    }

    //animation complete is a trigger function set at the end of animaiton, via the animation window
    protected void AnimationComplete() {
      isFading = false;
    }
  }
}