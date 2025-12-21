using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AiNav_ABC : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float agroRange = 5f;
    public float speedBoost = 10f; 
    public Image agroIcon;

    private NavMeshAgent agent;
    private Transform currentTarget;
    private float baseSpeed;
    private Transform player;
    private float agro = 0f;
    private float agroFillTime = 2f;
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
        Patrol();
        CheckAgro();
        Chase();
    }
    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
            agent.SetDestination(currentTarget.position);
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
            agro -= Time.deltaTime / agroFillTime;
        }
        agro = Mathf.Clamp01(agro);
        agroIcon.fillAmount = agro;
        
    }
    void Chase()
    {
        if(agro>=0.8f)
        {
            agent.SetDestination(player.position);
            agent.speed = baseSpeed+ speedBoost;
        }
        else if (agro <= 0f) 
        {
            agent.speed = baseSpeed;
        }
    }
}
