using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject target;
    private const float chasingDelay = 2.5f;
    private float lookRadius = 10f;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    { 
        FaceTarget();
        Invoke("Chase", chasingDelay);
    }

    void Chase()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance <= lookRadius)
        {
            //anim.SetBool("isScreaming", false);
            agent.SetDestination(target.transform.position);
            agent.speed = 3.5f;
            if (distance <= agent.stoppingDistance)
            {
                anim.SetBool("isAttacking", true);
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
}
