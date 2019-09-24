using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject target;
    private Animator anim;
    public GameManager gameManager;
    private const float chasingDelay = 2.5f;
    public AudioManager audio;
    private Vector3 wanderPoint;
    private float timer = 0;
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
        Invoke("Chase", chasingDelay); 
    }

    void Chase()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(target.transform.position)) < 45f && distance < 8f)
        {
            FaceTarget();
            agent.SetDestination(target.transform.position);
            if (distance <= agent.stoppingDistance)
            {
                
                if (SeekForTarget())
                {
                    anim.SetBool("isAttacking", true);
                    gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                    gameManager.EndGame();
                }      
            }
            
        }
        else
        {
            Wander();
        }
    }

    bool SeekForTarget()
    {
        RaycastHit hit;
        Vector3 me = new Vector3(transform.position.x, 1, transform.position.z);
        if (Physics.Raycast(me, transform.forward, out hit))
        {
            if (hit.collider.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
 
    public void Scream()
    {
        audio.Play("MonsterRoar");
    }

    private Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * 10f) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, 10f, -1);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }

    void Wander()
    {
        timer += Time.deltaTime;
        if(timer >= 2f)
        {
            wanderPoint = RandomWanderPoint();
            agent.SetDestination(wanderPoint);
            timer = 0;
        }
    }

}
