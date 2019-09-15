using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject target;
    private const float chasingDelay = 2.5f;
    private float lookRadius = 15f;
    private Animator anim;
    public GameManager gameManager;
    private float wanderRadius = 7f;
    private Vector3 wanderPoint;
    private bool isAware = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        wanderPoint = RandomWanderPoint();
    }

    // Update is called once per frame
    void Update()
    {
        FaceTarget();
        Invoke("Chase", chasingDelay);
        /*if (isAware)
        {
            
        }
        else
        {
            Wander();
        }*/
    }

    void Chase()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance <= lookRadius)
        {
            agent.SetDestination(target.transform.position);
            agent.speed = 3.5f;
            if (distance <= agent.stoppingDistance)
            {
                anim.SetBool("isAttacking", true);
                gameManager.EndGame();
            }
            else
            {
                anim.SetBool("isAttacking", false);
            }
        }
        
    }

    void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void Wander()
    {
        if(Vector3.Distance(transform.position, wanderPoint) < 0.5f)
        {
            wanderPoint = RandomWanderPoint();
        }
        else
        {
            Debug.Log("Wander");
            agent.SetDestination(wanderPoint);
        }
    }

    private Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, wanderRadius, -1);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }
}
