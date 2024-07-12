using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, 0, target.position.z) + offset;
        }
    }
    public void FollowBall(Transform ball)
    {
        target = ball;
        StartCoroutine(ReturnToPlayerAfterDelay(3.2f));
    }

    private IEnumerator ReturnToPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        target = player;
    }
}
