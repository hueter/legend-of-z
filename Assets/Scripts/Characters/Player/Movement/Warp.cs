using UnityEngine;
using System.Collections;

namespace RPG {
    public class Warp : MonoBehaviour {

        public Transform WarpTarget;
        private Animator Anim;


        IEnumerator OnTriggerEnter2D(Collider2D other) {

            GameObject Player = GameObject.Find("Player");
            Anim = Player.GetComponent<Animator>();

            if (other.gameObject.CompareTag("Player")) //prevents non Player game objects from warping
            {
                Player.GetComponent<PlayerMovement>().enabled = false; //Turns off Player movement by disabling script involved


                ScreenFader sf = GameObject.FindGameObjectWithTag("Fader").GetComponent<ScreenFader>();


                yield return StartCoroutine(sf.FadeToBlack()); //starts the FadeToBlack() in ScreenFader.cs Script and all other routines pause(yield)

                //Debug.Log("An object collided with a warp Target");

                //This means the object that collides with it will then move to the warp targets position
                // Player runs to Door -> warps Player to doors warp Target

                other.gameObject.transform.position = WarpTarget.position;
                Anim.SetBool("isWalking", false);

                yield return StartCoroutine(sf.FadeToClear()); //starts the FadeToClear() in ScreenFader.cs Script and all other routines pause(yield)

                Player.GetComponent<PlayerMovement>().enabled = true; //Turns on Player movement by enabling script involved
            }
        }
    }
}