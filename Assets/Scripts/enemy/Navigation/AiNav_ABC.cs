using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AiNav_ABC : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float agroRange = 5f;
    public float speedBoost = 10f;
    public float agroFillTime = 2f;
    public float agroLoseTime = 10f;
    public Image agroIcon;

    private bool chase = false;
    private NavMeshAgent agent;
    private Transform currentTarget;
    private float baseSpeed;
    private Transform player;
    private float agro = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentTarget = pointA;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.SetDestination(currentTarget.position);
        baseSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        CheckAgro();
        if (chase)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
        
    }
    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
            agent.SetDestination(currentTarget.position);
        }
        if (agro >= 0.8f)
        {
           chase = true;
            agent.speed = baseSpeed + speedBoost;
        }
    }
    void CheckAgro()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= agroRange)
        {
            agro += Time.deltaTime / agroFillTime;

        }
        else
        {
            agro -= Time.deltaTime / agroLoseTime;
        }
        agro = Mathf.Clamp01(agro);
        agroIcon.fillAmount = agro;
        
    }
    void Chase()
    {
        agent.SetDestination(player.position);

        if (agro <= 0f) 
        {
            chase = false;
            agent.speed = baseSpeed;
            if (Vector3.Distance(transform.position, pointA.position) < Vector3.Distance(transform.position, pointB.position))
            {
                agent.SetDestination(pointA.position);
            }else
            {
                agent.SetDestination(pointB.position);
            }
            
        }
    }
}
