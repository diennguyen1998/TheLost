using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;

    [SerializeField] private float walkSpeed, runSpeed;
    [SerializeField] private float runBuildUpSpeed;
    [SerializeField] private KeyCode runKey;

    private float movementSpeed;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    private CharacterController charController;

    //[SerializeField] private AnimationCurve jumpFallOff;
    //[SerializeField] private float jumpMultiplier;
    //[SerializeField] private KeyCode jumpKey;
    StaminaBar staminaBar;
    private float drainAmount = 0.4f;

    private bool isJumping;

    private Animator animator;

    public AudioManager audio;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        staminaBar = GetComponent<StaminaBar>();
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void RunAnim(bool isRun)
    {
        animator.SetBool("isRunning", isRun);
    }

    private void WalkAnim(bool isWalk, string dir)
    {
        animator.SetBool(dir, isWalk);
    }

    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(verticalInputName);

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;


        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        if ((vertInput != 0 || horizInput != 0) && OnSlope())
        {
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);
        }
            

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || isJumping == true)
        {
            if (Input.GetKey(KeyCode.S))
            {
                WalkAnim(true, "isWalkingBackward");
                WalkAnim(false, "isWalkingLeft");
                WalkAnim(false, "isWalkingRight");
            }
            else if (Input.GetKey(KeyCode.A))
            {
                WalkAnim(true, "isWalkingLeft");
                WalkAnim(false, "isWalkingRight");
            }
            else if (Input.GetKey(KeyCode.D))
            {
                WalkAnim(true, "isWalkingRight");
                WalkAnim(false, "isWalkingLeft");
            }
            else
            {
                WalkAnim(true, "isWalking");
                WalkAnim(false, "isWalkingBackward");
                WalkAnim(false, "isWalkingLeft");
                WalkAnim(false, "isWalkingRight");
            }   
        }
        else
        {
            WalkAnim(false, "isWalking");
            WalkAnim(false, "isWalkingBackward");
            WalkAnim(false, "isWalkingLeft");
            WalkAnim(false, "isWalkingRight");
        }

        SetMovementSpeed();
        //JumpInput();
    }

    private void SetMovementSpeed()
    {
        if (Input.GetKey(runKey) && Input.GetKey(KeyCode.W) && staminaBar.currentStamina >= 1f)
        {
            RunAnim(true);
            runSpeed = 10;
            staminaBar.DrainStamina(drainAmount);
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
        }

        else if (Input.GetKey(KeyCode.S))
        {
            runSpeed = 2;
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
        }

        else
        {
            RunAnim(false);
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
        }
            
    }


    private bool OnSlope()
    {
        if (isJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
            {
                print("OnSlope");
                return true;
            }

        return false;
    }

    /*private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }


    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;
        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }*/

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public bool IsJump()
    {
        return isJumping;
    }

    void LeftStepSound()
    {
        audio.Play("FootStepLeft");
    }

    void RightStepSound()
    {
        audio.Play("FootStepRight");
    }
}