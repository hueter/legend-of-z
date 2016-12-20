using UnityEngine;
using System.Collections;

namespace RPG {
  public class Ganon_Enemy_Manager : Character {
    public AudioClip LightningSound;
    public AudioClip ThrowSpearSound;
    public AudioClip HitButDidntTakeDamage;
    public AudioClip SpawnStatueSound;
    public AudioClip GanonGetHitSound;
    public AudioSource[] BossAudios;
    public AudioSource BossAttackAudio;
    public AudioSource BossHitAudio;
    public AudioSource BossExtraAudio;
    private AudioSource PlayerAudio;

    //Statue and Spear RigidBodies for spawning
    public Rigidbody2D EyeStatueZero;
    public Rigidbody2D EyeStatueOne;
    public Rigidbody2D EyeStatueTwo;
    public Rigidbody2D EyeStatueThree;
    public Rigidbody2D EyeStatueFour;
    public Rigidbody2D EyeStatueFive;
    public Rigidbody2D Spear;
    public Transform LinksTransfom;
    public Rigidbody2D GanonLigtningWall;


    //reference to player
    public GameObject Player;
    private Player_Manager PM;

    // flags
    public bool defeated = false;
    public bool IntroComplete = false;

    //Ganon's Animator
    private Animator Anim;

    //used to time attacks in routine()
    public float NextAvailableSpearThrowTime;
    public float NextAvailableStatueSpawnTime;
    public float NextAvailableLightningSummon;
    public bool GanonHasSpear;
    public bool IsSummoningLightning = false;
    public Vector3[] array = new Vector3[6];

    void Awake() {
      // Assign the Animator Component.
      CharacterAnimator = GetComponent<Animator>();
    }

    void Start() {
      // sets vectors to spawn statues at 0 for statue0's posistion ect, ect
      array[0] = new Vector3(21.29f, -272.23f, 0);
      array[1] = new Vector3(39.82f, -262.48f, 0);
      array[2] = new Vector3(36.63f, -275.01f, 0);
      array[3] = new Vector3(24.62f, -275.01f, 0);
      array[4] = new Vector3(21.49f, -262.58f, 0);
      array[5] = new Vector3(39.73f, -271.68f, 0);


      BossAudios = GetComponents<AudioSource>();
      BossAttackAudio = BossAudios[0];
      BossHitAudio = BossAudios[1];
      BossExtraAudio = BossAudios[2];

      //Set the basic stats. Also attach the appropriate scripts. These can be changed in the inspector for individual enemies
      GanonHasSpear = true;

      Anim = GetComponent<Animator>();
      NextAvailableLightningSummon = Time.time;
      NextAvailableStatueSpawnTime = Time.time + 1.5f;
      NextAvailableSpearThrowTime = Time.time + 1.8f;
      //currentDamage = damage;
      damage = 1;
      currentHealth = maxHealth;

      //Player stuff
      Player = GameObject.Find("Player");
      PM = Player.GetComponent<Player_Manager>();
      PlayerAudio = PM.PlayerExternalAudio;

    }

    void Update() {
      //This is used for the routine To check direction of player compared to enemy
      SpearStatusUpdateAnimatorCheck();

      if (Player && IntroComplete && !defeated) {
        routine();
      }
    }

    /***************************************************************************************************************
    *  Routine()  Times Ganon's attacks and tracks if he has his spear. Must have spear to spawn statues and  
    *  and throw spear
    ****************************************************************************************************************/

    public void routine() {
      if ((GanonHasSpear) && (Player.transform.position.y > -258) && (IsSummoningLightning == false) && (NextAvailableLightningSummon < Time.time)) {
        IsSummoningLightning = true;
        StartLightningSummonAnimation();
        NextAvailableLightningSummon = Time.time + 6f;
      } else {
        if ((GanonHasSpear) && (NextAvailableStatueSpawnTime < Time.time)) {
          MakeEyeStatueAppear();
          NextAvailableStatueSpawnTime = Time.time + 5f;
        } else if ((GanonHasSpear) && (NextAvailableSpearThrowTime < Time.time)) {
          StartThrowSpearAnimation();
        }
      }
    }

    /***************************************************************************************************************
     * TakeDamage() Only plays a sound when Ganon is hit since he can't be hit by link's attacks
     *  
     ****************************************************************************************************************/

    public void TakeDamage(Character Char) {
      BossHitAudio.clip = HitButDidntTakeDamage;
      BossHitAudio.Play();
      return;
    }
    /***************************************************************************************************************
    *  TakeFireballDamage() is called form fireball script. It subtracts one from heath, plays hit sound. If 0 hit points
    *  calls death().
    ****************************************************************************************************************/
    public void TakeFireballDamage(Character Char) {

      currentHealth -= Char.damage;
      StartCoroutine(Flasher());
      Hit(Char.transform, Char.joltAmount);

      BossHitAudio.clip = GanonGetHitSound;
      BossHitAudio.Play();

      if (currentHealth <= 0) {
        Death();
        return;
      }
    }
    /***************************************************************************************************************
    *  Death() calls drop loot and calls a function to destory all of Ganon's spawned gameobjects, then self destructs
    ****************************************************************************************************************/
    private void Death() {
      // Display health as 0.
      defeated = true;
      currentHealth = 0;
      GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().Stop();
      StartCoroutine(DeathScream());
    }


    IEnumerator DeathScream() {
      PlayerAudio.clip = DieSound;
      PlayerAudio.Play();
      yield return new WaitForSeconds(DieSound.length);
      // Put getting loot code here like money or health
      dropLoot();

      DestoryAllGanonFightGameObjects();
      Destroy(this.gameObject);
    }

    /// <summary>
    /// Everything you want to happen when the GameObject takes damage but doesnt die.
    /// </summary>
    private void Hit(Transform otherCharacter, float joltAmount) {
      return;
    }

    // This gives the enemy a flashing effect when they get hit
    IEnumerator Flasher() {
      var render = GetComponent<SpriteRenderer>();
      var normal_color = render.color;
      render.material.color = Color.black;
      yield return new WaitForSeconds(.1f);
      render.material.color = normal_color;
      yield return new WaitForSeconds(.1f);
    }

    void dropLoot() {
      GameObject.Instantiate(Resources.Load("TriforcePiece"), new Vector2(30.72f, -264.33f), Quaternion.identity);
      GameObject.Instantiate(Resources.Load("EndCinematic"), new Vector2(30.72f, -256.13f), Quaternion.identity);
    }
    /***************************************************************************************************************
     *  StartThrowSpearAnimation() plays the sound and starts the throw animation. Also sets hasSpear bool to false.
     ****************************************************************************************************************/
    private void StartThrowSpearAnimation() {
      BossAttackAudio.clip = ThrowSpearSound;
      BossAttackAudio.Play();
      Anim.SetBool("isThrowing", true);
      GanonHasSpear = false;
      Anim.SetBool("hasSpear", false);
    }

    /***************************************************************************************************************
     *MakeEyeStatueAppear() gets a random number 0-5. The random int is used to spawn a statue in a certain spot. Each spot
     * has it's on statue because Unity was being difficult and it allows a simple check to see if there excists a statue in a given spot before spawning
     * it. 
     ****************************************************************************************************************/
    public void MakeEyeStatueAppear() {
      int randomLocation = Random.Range(0, 6);

      BossExtraAudio.clip = SpawnStatueSound;

      if (randomLocation == 0) {
        GameObject EyeStatue0 = GameObject.Find("EyeStatueZero(Clone)");
        if (EyeStatue0 == null) {
          BossExtraAudio.Play();
          Rigidbody2D.Instantiate(EyeStatueZero, array[0], transform.rotation);
        }
      } else if (randomLocation == 1) {
        GameObject EyeStatue1 = GameObject.Find("EyeStatueOne(Clone)");
        if (EyeStatue1 == null) {
          BossExtraAudio.Play();
          Rigidbody2D.Instantiate(EyeStatueZero, array[1], transform.rotation);
        }
      } else if (randomLocation == 2) {
        GameObject EyeStatue2 = GameObject.Find("EyeStatueTwo(Clone)");
        if (EyeStatue2 == null) {
          BossExtraAudio.Play();
          Rigidbody2D.Instantiate(EyeStatueZero, array[2], transform.rotation);
        }
      } else if (randomLocation == 3) {
        GameObject EyeStatue3 = GameObject.Find("EyeStatueThree(Clone)");
        if (EyeStatue3 == null) {
          BossExtraAudio.Play();
          Rigidbody2D.Instantiate(EyeStatueZero, array[3], transform.rotation);
        }
      } else if (randomLocation == 4) {
        GameObject EyeStatue4 = GameObject.Find("EyeStatueFour(Clone)");
        if (EyeStatue4 == null) {
          BossExtraAudio.Play();
          Rigidbody2D.Instantiate(EyeStatueZero, array[4], transform.rotation);
        }
      } else if (randomLocation == 5) {
        GameObject EyeStatue5 = GameObject.Find("EyeStatueFive(Clone)");
        if (EyeStatue5 == null) {
          BossExtraAudio.Play();
          Rigidbody2D.Instantiate(EyeStatueZero, array[5], transform.rotation);
        }
      }
    }

    /***************************************************************************************************************
    *  MakeThrownSpearAppear() Sanity checks if spear is already thrown or bouncing about. If no spear is found, it makes
    *  one spawn at the end of ganon's throwing hand. This function is called from the animator. Lastly Animator bool value isThrowing is set to false
    ****************************************************************************************************************/
    public void MakeThrownSpearAppear() {
      GameObject SpearAlreadyThrown = GameObject.FindWithTag("GanonSpear");

      //checks if there is already a thrown spear in the scence, if null makes one
      if (SpearAlreadyThrown == null) {

        Rigidbody2D.Instantiate(Spear, new Vector3(26.69f, -260.74f, 0), transform.rotation);
      }
      Anim.SetBool("isThrowing", false);
    }

    /***************************************************************************************************************
     *  SpearStatusUpdateAnimatorCheck simply is a sanity check to ensure the animator and script are synced
    ****************************************************************************************************************/
    public void SpearStatusUpdateAnimatorCheck() {
      if (GanonHasSpear) {
        Anim.SetBool("hasSpear", true);
      } else {
        Anim.SetBool("hasSpear", false);
      }
    }

    /***************************************************************************************************************
    * DestoryAllGanonFightGameObjects searches for gameobject by name, the search returns something it is destroyed
    ****************************************************************************************************************/
    public void DestoryAllGanonFightGameObjects() {


      GameObject EyeStatue0 = GameObject.Find("EyeStatueZero(Clone)");
      if (EyeStatue0 != null) {
        Destroy(EyeStatue0);
      }

      GameObject EyeStatue1 = GameObject.Find("EyeStatueOne(Clone)");
      if (EyeStatue1 != null) {
        Destroy(EyeStatue1);
      }

      GameObject EyeStatue2 = GameObject.Find("EyeStatueTwo(Clone)");
      if (EyeStatue2 != null) {
        Destroy(EyeStatue2);
      }

      GameObject EyeStatue3 = GameObject.Find("EyeStatueThree(Clone)");
      if (EyeStatue3 != null) {
        Destroy(EyeStatue3);
      }

      GameObject EyeStatue4 = GameObject.Find("EyeStatueFour(Clone)");
      if (EyeStatue4 != null) {
        Destroy(EyeStatue4);
      }

      GameObject EyeStatue5 = GameObject.Find("EyeStatueFive(Clone)");
      if (EyeStatue5 != null) {
        Destroy(EyeStatue5);
      }

      GameObject Spear = GameObject.Find("Ganon'sThrownSpear(Clone)");
      if (Spear != null) {
        Destroy(Spear);
      }

    }

    private void StartLightningSummonAnimation() {
      Anim.SetBool("isSummoningLightning", true);
      NextAvailableLightningSummon = Time.time + 3.5f;
    }

    public void MakeLightningWallAppear() {

      BossAttackAudio.clip = LightningSound;
      BossAttackAudio.Play();

      Rigidbody2D.Instantiate(GanonLigtningWall, new Vector3(30.23f, -251.21f, 0), transform.rotation);
      Anim.SetBool("isSummoningLightning", false);
      IsSummoningLightning = false;
    }
  }
}

