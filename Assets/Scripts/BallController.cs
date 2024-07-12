using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] Transform goal1, goal2;
    [SerializeField] float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Transform FindNearestGoal()
    {
        float distanceToGoal1 = Vector3.Distance(transform.position, goal1.position);
        float distanceToGoal2 = Vector3.Distance(transform.position, goal2.position);

        return distanceToGoal1 < distanceToGoal2 ? goal1 : goal2;
    }
    public void KickBall()
    {
        Transform goal = FindNearestGoal();
        StartCoroutine(MoveToGoal(goal));
    }
    IEnumerator MoveToGoal(Transform goal)
    {
        while (Vector3.Distance(transform.position, goal.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, goal.position, speed * Time.deltaTime);
            yield return null;
        }
    }
}
