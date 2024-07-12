using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float RotationSpeed;
    [SerializeField] Camera cam;
    [SerializeField] Button kickButton;
    [SerializeField] float checkRadius = 0.7f;

    private float x, z;

    Rigidbody rb;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Movement();
        ShowKickButton();
    }

    void GetInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
    }
    void Movement()
    {
        Vector3 targetVector = new Vector3(x, 0, z);
        Vector3 movementVector = MoveTowardTarget(targetVector);
        RotateTowardMovementVector(movementVector);
        animator.SetBool("Running", x != 0 || z != 0);
    }
    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        float speedMove = speed * Time.deltaTime;
        

        targetVector = Quaternion.Euler(0, cam.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        Vector3 targetPosition = transform.position + targetVector * speedMove;
        transform.position = targetPosition;
        return targetVector;
    }
    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) 
        { 
            return; 
        }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }
    private void ShowKickButton()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);
        bool ballNearby = false;
        foreach(Collider collider in hitColliders)
        {
            if (collider.CompareTag("Ball"))
            {
                ballNearby = true;
                break;
            }
        }
        kickButton.gameObject.SetActive(ballNearby);
    }
    public void Kick()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Ball"))
            {
                collider.gameObject.GetComponent<BallController>().KickBall();
                cam.GetComponent<CameraController>().FollowBall(collider.gameObject.transform);
            }
        }
        
    }
    public void AutoKick()
    {
        BallController[] allBalls = FindObjectsOfType<BallController>();
        float furthestDistance = 0f;
        foreach(BallController ball in allBalls)
        {
            if (!ball.isGoal)
            {
                float ballDistance = Vector3.Distance(ball.transform.position, transform.position);
                if (ballDistance > furthestDistance)
                {
                    furthestDistance = ballDistance;
                }
            }
        }
        foreach (BallController ball in allBalls)
        {
            if (!ball.isGoal)
            {
                float ballDistance = Vector3.Distance(ball.transform.position, transform.position);
                if (ballDistance >= furthestDistance)
                {
                    ball.KickBall();
                    cam.GetComponent<CameraController>().FollowBall(ball.gameObject.transform);
                }
            }
        }
    }
}
