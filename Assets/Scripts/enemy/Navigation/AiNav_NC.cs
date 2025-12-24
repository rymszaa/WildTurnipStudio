using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AiNav_NC : MonoBehaviour
{
    public Transform[] points;

    public float agroRange = 5f;
    public float speedBoost = 10f;
    public float agroFillTime = 2f;
    public float agroLoseTime = 10f;
    public Image agroIcon;


    private int currentIndex = 0;
    private NavMeshAgent agent;
    private Transform currentTarget;
    private float baseSpeed;
    private Transform player;
    private float agro = 0f;
    private bool chase = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentTarget = points[0];
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
            currentIndex = (currentIndex +1)%points.Length;
            currentTarget = points[currentIndex];
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
            agent.speed = baseSpeed;
            currentIndex = GetClosestPatrolPoint();
            agent.SetDestination(points[currentIndex].position);
            chase = false;
        }
    }

    int GetClosestPatrolPoint()
    {
        int closestIndex = 0;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < points.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, points[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

}
