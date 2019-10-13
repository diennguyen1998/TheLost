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
    public HeadLookController headControl;
    public GameObject targetToLook;
    private bool found = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        wanderPoint = RandomWanderPoint();
        headControl.target = targetToLook.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (found)
        {
            //headControl.target = targetToLook.transform.position;
            Attack();
        }
        else
        {
            Invoke("Chase", chasingDelay);
        }
    }

    void Chase()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(target.transform.position)) < 60f && distance < 6f)
        {
            FaceTarget();
            //headControl.target = target.transform.position;
            agent.SetDestination(target.transform.position);
            if (distance <= agent.stoppingDistance)
            {
                if (SeekForTarget())
                {
                    found = true;
                }
            }
            
        }
        else
        {
            //headControl.target = targetToLook.transform.position;
            anim.SetBool("isWalking", false);
            Wander();
        }
    }

    void Attack()
    {
        anim.SetBool("isAttacking", true);
        gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        gameManager.EndGame();
    }

    bool SeekForTarget()
    {
        RaycastHit hit;
        Vector3 me = new Vector3(transform.position.x, 1, transform.position.z);
        if (Physics.Raycast(me, transform.forward, out hit, Mathf.Infinity))
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
        if (timer >= 3f)
        {
            anim.SetBool("isWalking", true);
            wanderPoint = RandomWanderPoint();
            if(Vector3.Distance(transform.position, wanderPoint) > 5f)
            {
                agent.SetDestination(wanderPoint);
                timer = 0;
            }
        }
    }

}
