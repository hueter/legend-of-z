using UnityEngine;
using System.Collections;


namespace RPG
{
    public class GanonThrownSpearScript : MonoBehaviour
    {
        private float SpearThrownStartTime;
        public float SpearSpeed;  //set via inspector 
        public Character Link;
        public Character Ganon;
        Vector2 CurrentDirection;
        Rigidbody2D SpearRigidBody;
        Ganon_Enemy_Manager GanonEnemyManger;
        private float StartTime;
        private bool SpearAlreadyHitPlayer;
        public AudioClip GanonLaugh;
        public AudioSource Sound;

        // Use this for initialization
        void Start()
        {
            SpearAlreadyHitPlayer = false;
            Ganon = GameObject.Find("Ganon").GetComponent<Character>();
            SpearRigidBody = GetComponent<Rigidbody2D>();
            Link = GameObject.Find("Player").GetComponent<Character>();
            SpearThrownStartTime = Time.time;
            CurrentDirection = FindDirectionOfLink();
            GanonEnemyManger = GameObject.FindGameObjectWithTag("Ganon").GetComponent<Ganon_Enemy_Manager>();
            StartTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            
            MoveSpear(CurrentDirection);
        }
        /***************************************************************************************************************
         *   FindDirectionOfLink() finds a line from Spear spawn point to link and returns it
         ****************************************************************************************************************/
        private Vector2 FindDirectionOfLink()
        {
            Vector2 fromPosition = transform.position;
            Vector2 toPosition = Link.transform.position;
            Vector2 Linksdirection = toPosition - fromPosition;
            return Linksdirection;
        }

        /***************************************************************************************************************
         *   MoveSpear() is called via update function and moves spear along the current Direction
         ****************************************************************************************************************/
        private void MoveSpear(Vector2 Direction)
        {
            SpearRigidBody.velocity = Direction.normalized * SpearSpeed ;
        }

        /***************************************************************************************************************
         *   OnTriggerEnter2d() handles collisions of the spear. If the spear has been bouncing around for a long time
         *   it does make a small adjustment because it is possible the spear is stuck and won't return to Ganon. It also 
         *   uses tags to determine which direction to richochet in. If the player is hit a vector back to Ganon is used to return 
         *   to Ganon and Ganon's laugh is played. If a collision with Ganon, the spear is destoryed and Ganon's Manager script bools are adjusted
         ****************************************************************************************************************/
        void OnTriggerEnter2D(Collider2D other)
        {
                //if spear is stuck in same pattern give a little adjustment
                if(StartTime + 10.0f < Time.time )
                {
                     CurrentDirection.x += .5f;
                     CurrentDirection.y += .4f;
                }
            
                
                if (other.gameObject.tag == "GanonWallUp")
                {
                    CurrentDirection = Vector2.Reflect(CurrentDirection, Vector2.up);
                }
                else if (other.gameObject.tag == "GanonWallRight")
                {
                    CurrentDirection = Vector2.Reflect(CurrentDirection, Vector2.right);
                }
                else if (other.gameObject.tag == "GanonWallDown")
                {
                    CurrentDirection = Vector2.Reflect(CurrentDirection, Vector2.down);
                }
                else if(other.gameObject.tag == "GanonWallLeft")
                {
                    CurrentDirection = Vector2.Reflect(CurrentDirection, Vector2.left);
                }
                //Ganon doesn't catch spear if spear hasn't traveled at player or gannon does catch if hit player already
                else if (((other.gameObject.tag == "Ganon") && ((SpearThrownStartTime + .5f) < Time.time)) || ((other.gameObject.tag == "Ganon") && ((SpearAlreadyHitPlayer == true))))
                {
                    GanonEnemyManger.GanonHasSpear = true;
                    GanonEnemyManger.NextAvailableSpearThrowTime = Time.time + 1f;
                    Destroy(this.gameObject);
                }
                //if spear hit player, they take damage and spear returns to ganon
                else if (other.gameObject.tag == "Player")
                {
                    Sound.clip = GanonLaugh;
                    Sound.Play();
                    SpearAlreadyHitPlayer = true;    

                    other.gameObject.SendMessage("TakeDamage", Ganon, SendMessageOptions.DontRequireReceiver);
                    Vector2 fromPosition = Link.transform.position;
                    Vector2 toPosition = Ganon.transform.position;
                    CurrentDirection = (toPosition - fromPosition);
                
                }


        }
    }
}