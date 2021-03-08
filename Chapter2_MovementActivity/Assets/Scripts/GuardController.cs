using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GuardController : MonoBehaviour
{
    //how well the player can be seen
    public Transform player;
    private float fovDist = 20f;
    private float fovAngle = 45f;

    private enum State { Patrol, Investigate, Chase};

    private State curState = State.Patrol;
    private Vector3 lastPlaceSeen; //where player was last seen

    //chase settings
    public float chasingSpeed = 2f;
    public float chasingRotSpeed = 2f;
    public float chasingAccuracy = 2f;

    //patrol settings
    public float patrolDistance = 10f;
    private float patrolWait = 5f;
    private float patrolTimePassed = 0f;

    //Investigation/knock
    public float knockRadius = 20f;

    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        patrolTimePassed = patrolWait;
        lastPlaceSeen = transform.position;
    }

    private void Update()
    {
        chasingSpeed += 0.1f;

        if (Input.GetKey("space"))
        {
            StartCoroutine(PlayKnock());
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, knockRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Guard"))
            {
                hitColliders[i].GetComponent<GuardController>().InvestigatePoint(transform.position);
            }
            else
            {
                //do nothing                                                                                                                                                                                                                                                                
            }
        }

        State tempState = curState;

        if (ICanSee(player))
        {
            //Debug.Log("Saw player at" + player.position);
            curState = State.Chase;
        }
        else
        {
            //Debug.Log("Can't see player");
            if(curState == State.Chase)
            {
                curState = State.Investigate;
            }
        }

        switch (curState)
        {
            case State.Patrol: // start partolling
                Patrol();
                break;
            case State.Investigate:
                Investigate();
                break;
            case State.Chase:
                Chase();
                break;
        }

        if (tempState != curState)
        {
            Debug.Log("Guard current state: " + curState);
        }
    }

    private void Patrol()
    {
        patrolTimePassed += Time.deltaTime;

        if(patrolTimePassed > patrolWait)
        {
            patrolTimePassed = 0f;

            Vector3 patrollingPoint = lastPlaceSeen;
            patrollingPoint += new Vector3(Random.Range(-patrolDistance, patrolDistance), 0f, Random.Range(-patrolDistance, patrolDistance));

            navMeshAgent.SetDestination(patrollingPoint);
        }
    }

    private void Investigate()
    {
        if(transform.position == lastPlaceSeen)
        {
            curState = State.Patrol;
        }
        else
        {
            navMeshAgent.SetDestination(lastPlaceSeen);
            Debug.Log("Guard state: " + curState + " point " + lastPlaceSeen);
        }
    }

    private void Chase()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();

        Vector3 direction = player.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * chasingSpeed);

        if(direction.magnitude > chasingAccuracy)
        {
            //stop chasing
            transform.Translate(0, 0, Time.deltaTime * chasingSpeed);
        }
    }

    private bool ICanSee(Transform player)
    {
        Vector3 direction = player.position - this.transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        RaycastHit hit;

        if(Physics.Raycast(this.transform.position, direction, out hit) && hit.collider.CompareTag("Player")
            && direction.magnitude < fovDist && angle < fovAngle)
        {
            return true;
        }

        if(Vector3.Distance(transform.position, player.transform.position) < 1.5)
        {
            Destroy(player.gameObject);
            StartCoroutine(WaitToReload());
        }

        return false;
    }

    IEnumerator WaitToReload()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void InvestigatePoint(Vector3 point)
    {
        lastPlaceSeen = point;
        curState = State.Investigate;
    }

    IEnumerator PlayKnock()
    {
        AudioSource audio = GetComponent<AudioSource>();

        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
    }
}
