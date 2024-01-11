using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public UI ui;

    private Rigidbody rb;
    private CharacterController characterController;
    public Animator animator;
    public SphereCollider thiscollider;
    public Transform ball;
    public Rigidbody ballrb;
    public LineRenderer lineRenderer;
    public GameObject objectToRotate;


    // movement
    private Vector3 startPosition;
    private Vector3 touchPosition;
    private bool noTouches;
    public bool swipesBlocked;

    public float horizontal;
    public float vertical;

    public float power;
    public float speed;
    private float speedIndex = 1;
    public float rotationSpeed;
    public float leftBound;
    public float rightBound;
    public float forwardBound;
    public float forwardBound2;
    private Vector3 moveDirection = Vector3.zero;

    public float gravity;
    public bool ground;
    public bool walk;
    public bool punch;
    public bool ballactivated;

    // a1
    public bool specialPunch;
    public bool privatespecialpunch;

    // punch
    public bool ballflying;
    public Transform targetdot;
    private Vector3 targetposition;

    // effects
    public ParticleSystem punchEffect;
    public GameObject skill;


    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        characterController = transform.GetComponent<CharacterController>();
        skill.SetActive(false);
    }
        private void Update()
    {
        CheckSwipeInput();

        if (Application.isEditor)
        {               
            if (!swipesBlocked && !ui.menuScreen.activeSelf)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    noTouches = false;
                    startPosition = Input.mousePosition;
                    Debug.Log("starttouch");
                    swipesBlocked = false;

                }
                if (Input.GetMouseButton(0))
                {
                    if (startPosition == Vector3.zero)
                    {
                        noTouches = false;
                        startPosition = Input.mousePosition;
                    }
                    Debug.Log("touchcontinues");

                    touchPosition = Input.mousePosition;
                    float xDelta = touchPosition.x - startPosition.x;
                    float yDelta = touchPosition.y - startPosition.y;
                    if (Mathf.Abs(yDelta) > Mathf.Abs(xDelta))
                    {
                        if (yDelta > 0)
                        {
                            swipeUp();
                        }
                        else
                        {
                            swipeDown();
                        }
                    }
                    else
                    {
                        if (xDelta > 0)
                        {
                            swiperight();

                        }
                        else
                        {
                            swipeleft();
                        }
                    }
                }
            }
            else if (swipesBlocked)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (swipesBlocked && !ui.arrowmoving.activeSelf && !ballflying)
                    {
                        swipesBlocked = false;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("touch stops");
                startPosition = Vector3.zero;
                if (!swipesBlocked && !ui.menuScreen.activeSelf)
                {
                    horizontal = 0;
                    vertical = 0;
                }
                noTouches = true;

            }
        }

        // лінія напрямку
        if (!swipesBlocked && !ui.menuScreen.activeSelf && !ballflying)
        {
            speedIndex = 1;


            // анімація
            if (vertical != 0 || horizontal != 0)
            {
                walk = true;
            }
            else
            {
                walk = false;
            }
            animator.SetBool("walk", walk);
            animator.SetBool("punch", punch);


            // мяч
            if (ballactivated && !ballflying)
            {
                Vector3 direction = ball.position - transform.position;
                direction.Normalize();
                targetposition = ball.position + direction * 5f;
                targetposition.y = 1;

                lineRenderer.SetPosition(0, ball.position);
                lineRenderer.SetPosition(1, targetposition);


                lineRenderer.enabled = true;
            }

        }
        else if (privatespecialpunch && ballflying && !ui.menuScreen.activeSelf) // якщо був удар з бонусом
        {
            speedIndex = 0;
            swipesBlocked = false;

            Vector3 lateralForceVector = Vector3.right * horizontal * 5;
            ballrb.AddForce(lateralForceVector, ForceMode.Acceleration);


            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, ball.position);


            lineRenderer.enabled = true;
        }
        else
        {
            speedIndex = 0;
        }

        MoveToTarget();

        // tutorial check
        if ((horizontal != 0 || vertical != 0) && ui.movementTip == 0)
        {
            ui.movementTip = 1;
            PlayerPrefs.SetFloat("movementTip", ui.movementTip);
            PlayerPrefs.Save();
        }
    }

    void CheckSwipeInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!swipesBlocked && !ui.menuScreen.activeSelf)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    noTouches = false;

                    startPosition = touch.position;
                    swipesBlocked = false;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    if (startPosition == Vector3.zero)
                    {
                        noTouches = false;
                        startPosition = touch.position;
                    }

                    touchPosition = touch.position;
                    float xDelta = touchPosition.x - startPosition.x;
                    float yDelta = touchPosition.y - startPosition.y;
                    if (Mathf.Abs(yDelta) > Mathf.Abs(xDelta))
                    {
                        if (yDelta > 0)
                        {
                            swipeUp();
                        }
                        else
                        {
                            swipeDown();
                        }
                    }
                    else
                    {
                        if (xDelta > 0)
                        {
                            swiperight();

                        }
                        else
                        {
                            swipeleft();
                        }
                    }

                }
            }
            else if (swipesBlocked)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    if (swipesBlocked && !ui.arrowmoving.activeSelf && !ballflying)
                    {
                        swipesBlocked = false;
                    }
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("touch stops");
                startPosition = Vector3.zero;
                if (!swipesBlocked && !ui.menuScreen.activeSelf)
                {
                    horizontal = 0;
                    vertical = 0;
                }
                noTouches = true;
            }
        }
    }

   public void punching()
    {
        punchEffect.Clear();
        punchEffect.Play();

        ui.sounds[3].Play();
        ui.currentshots += 1;
        animator.Play("Punch");
        float x = ui.arrowmovingchild.transform.localPosition.x;
        if (x < -205)
        {
        power = 10;
        }
        else if (x > -205 && x < -150)
        {
            power = 9;
        }
        else if (x > -150 && x < -95)
        {
            power = 8;
        }
        else if (x > -95 && x < -50)
        {
            power = 7;
        }
        else if (x > -50 && x < 15)
        {
            power = 6;
        }
        else if (x > 15 && x < 70)
        {
            power = 5;
        }
        else if (x > 70 && x < 130)
        {
            power = 4;
        }
        else if (x > 130 && x < 180)
        {
            power = 3;
        }
        else if (x > 180)
        {
            power = 2;
        }

        ballrb.velocity = Vector3.zero;
        ballrb.angularVelocity = Vector3.zero;
        ballrb.AddForce(targetdot.transform.forward * power, ForceMode.Impulse);

        ballflying = true;
        swipesBlocked = true;

        if (specialPunch)
        {
            ui.a1bought = 0;
            ui.a1check();
            PlayerPrefs.SetInt("a1bought", ui.a1bought);
            PlayerPrefs.Save();
            privatespecialpunch = true;
        }
        else
        {
            ui.arrows.SetActive(false);
        }
    }
    void MoveToTarget()
    {
        if (!ballflying || (ballflying && privatespecialpunch))
        {
            Debug.Log("moving");
            if (ground && moveDirection.y < 0)
            {
                moveDirection.y = 0f;
            }
            Vector3 move = new Vector3(horizontal * speed * speedIndex, 0, vertical * speed * speedIndex);

            characterController.Move(move);

            Vector3 ballPositionToLook = new Vector3(ball.position.x, ball.position.y + 0.5f, ball.position.z);
            objectToRotate.transform.LookAt(ballPositionToLook);

            moveDirection.y += gravity * Time.deltaTime;
            characterController.Move(moveDirection);

            if (!ballflying)
            {
                transform.localPosition = new Vector3(
             Mathf.Clamp(transform.localPosition.x, ball.transform.localPosition.x - 1f, ball.transform.localPosition.x + 1f),
             transform.localPosition.y,
            Mathf.Clamp(transform.localPosition.z, ball.transform.localPosition.z - 1f, ball.transform.localPosition.z + 1f)
            );
                if (!ballactivated)
                {
                    activateBar();
                }
            }
        }
    }

    public void activateBar()
    {
        ballactivated = true;
        ui.activeShot();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ground = true;
        }
        if (collision.gameObject.tag == "Ball")
        {
            Physics.IgnoreCollision(collision.collider, thiscollider);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            ground = false;
        }
    }

    public void swiperight()
    {
        vertical = 0;
        horizontal = 1;
    }
    public void swipeleft()
    {
        vertical = 0;
        horizontal = -1;
    }
    public void swipeUp()
    {
        horizontal = 0;
        vertical = 1;
    }
    public void swipeDown()
    {
        horizontal = 0;
        vertical = -1;
    }
}
