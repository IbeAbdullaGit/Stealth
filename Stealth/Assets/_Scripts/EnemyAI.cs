using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Transform target;
    NavMeshAgent NavMeshAgent;
    public float alertness= 5f;


    private float dirX;
   
    public float moveSpeed = 2.5f;
    private Rigidbody rb;
    private bool facingRight = false;
    private Vector3 localScale;

    bool playerInSight = false;

    public Transform[] waypoints;
    int waypointIndex = 0;
    Vector3 targetMain;

    FieldOfView FOV;

    DetectionMeter detectionMeter;

    public WaitForSeconds delay = new WaitForSeconds(1f);
    public WaitForSeconds distract = new WaitForSeconds(5f);

    public bool distracted = false;

    SoundManager soundInstance;
    bool playSoundOnce = false;
 
    Vector3 lastPosition;

     float hearRadius;
     bool canHear;

    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        FOV = GetComponent<FieldOfView>();
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
      
        lastPosition = transform.position;

        //moveSpeed = 2.5f;

       
        dirX = -1f;

        UpdateDestination();

        //detectionMeter = GameObject.FindGameObjectWithTag("GameController").GetComponent<DetectionMeter>();
        soundInstance = GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundManager>().instance;

        //make hear radius same as view radius?
        hearRadius = GetComponent<FieldOfView>().radius;
       
    }
    void UpdateDestination()
    {
        targetMain = waypoints[waypointIndex].position;
        NavMeshAgent.SetDestination(targetMain);
        //going to a new place now, should turn
        // //add pause when switching between points
        // NavMeshAgent.isStopped = true;
       
        // NavMeshAgent.isStopped = false;
        StartCoroutine(WalkPause());
    }
    void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
        
    }
   
    private IEnumerator WalkPause()
    {
        NavMeshAgent.isStopped = true;
        //wait
        //Debug.Log("Waiting");
        yield return delay;
        //after waiting
        NavMeshAgent.isStopped = false;    
        //Debug.Log("Done waiting");  
          
    }
    public IEnumerator Distracted(GameObject distraction)
    {
        Debug.Log("Starting Distraction");
        //stop current coroutines
        StopCoroutine(WalkPause());

        //flag to indicate distraction
        distracted = true;
        
        //stop any coroutine that might be conflicting with this one, we want enemy to immediately walk over
        //StopCoroutine(WalkPause());
        //make sure they can walk
         NavMeshAgent.isStopped=false;
         //give them destination to go to
        NavMeshAgent.SetDestination(distraction.transform.position);
       
        //enemy should not do anything else but just move, rest of AI is turned off
        
        yield return distract;

        //afterwards, destroy the distraction
        //Destroy(distraction); //Disable if objectpooling
        distraction.SetActive(false); //Used for object pooling, a bit buggy. Prefer if we use this and fix. Enable object pooling under Player's script.

        //set destination back to normal
        targetMain = waypoints[waypointIndex].position;
        NavMeshAgent.SetDestination(targetMain);

        distracted = false;
    }
  

    // Update is called once per frame
    void Update()
    {
       if (!distracted)
       {
            //vision test then hearing test
            playerInSight = inSight();
            HearingCheck();
           // Debug.Log(Vector3.Distance(transform.position, targetMain));
           //need more leniency here
            if (Vector3.Distance(transform.position, targetMain) <1.5f )
            {
                //detectionMeter.Meter();
                IterateWaypointIndex();
                UpdateDestination();
                
                //Debug.Log("Switching path");
            }
            //Debug.Log(NavMeshAgent.pathStatus);
            
            //reset path
            /* if (NavMeshAgent.isStopped)
            {
                NavMeshAgent.enabled = false;
                NavMeshAgent.enabled = true;
                IterateWaypointIndex();
                UpdateDestination();
            }  */
       }
       /* //check for moving
        if (transform.position != lastPosition)
        {
            //dont play for now
            //soundInstance.PlaySound(SoundManager.Sound.EnemyMove, transform.position);
        }
        else
            
        lastPosition = transform.position; */
        //get unstuck
       /* if (!NavMeshAgent.hasPath && NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete) {
             Debug.Log("Character stuck");
             NavMeshAgent.enabled = false;
             NavMeshAgent.enabled = true;
             Debug.Log("navmesh re enabled");
             // navmesh agent will start moving again
        }    */
        
    }
 
    void CheckWhereToFace(){
        
        if (dirX >0)
            {
                facingRight = true;
            }
            else{
             facingRight = false;
            }

        if (((facingRight) && (localScale.x <0)) || ((!facingRight) && (localScale.x >0)))
        {
            localScale.x *= -1f;
        }
        transform.localScale = localScale;
        

    }

      private void HearingCheck()
    {
        float hearDistance = Vector3.Distance(transform.position, target.position);
        //check if player is moving
        if (target.GetComponent<PlayerController>().state != PlayerController.MovementState.idle && hearDistance < hearRadius)
        {
            Debug.Log("Can hear you");
            //use this for now, can hear
            canHear = true;

        }
        else{
            //dont want to get stuck "hearing"
            canHear = false;
        }
    }
    bool inSight()
    {
       //if we can see the player or hear the player
        if(FOV.canSeePlayer || canHear)
        {
            //Debug.Log("Enemy AI sees player");
           //stop coroutine, so enemy can move
           StopCoroutine(WalkPause());
           //make sure enemy can move
           NavMeshAgent.isStopped = false; 

            targetMain = target.position;
            NavMeshAgent.SetDestination(target.position);
            Debug.DrawLine(transform.position, target.position, Color.red);

            //play sound
            if (!playSoundOnce)
            {
                soundInstance.PlaySound(SoundManager.Sound.EnemyDetect, transform.position);
                playSoundOnce = true;
            }
            return true;
        }
        else{
            //reset sound
            playSoundOnce = false;
        }
        targetMain = waypoints[waypointIndex].position;
        NavMeshAgent.SetDestination(targetMain);
        return false;
    }

   
}
