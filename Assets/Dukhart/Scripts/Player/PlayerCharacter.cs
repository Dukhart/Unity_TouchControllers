using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] Button_TouchController buttonController;
    [SerializeField] JoyStick_TouchController joystickController;
    [SerializeField] Camera playerCamera;
    CharacterController characterController;
    Animator animator;

    [SerializeField] float speed = 5;
    public float Speed { 
        get {
            return speed;
        } 
    }
    [SerializeField] float jumpPower = 0.1f;
    public float JumpPower
    {
        get
        {
            return jumpPower;
        }
    }
    [SerializeField, Range(-9.81f,9.81f)] float gravityModifier = 0;
    public float GravityModifier
    {
        get
        {
            return gravityModifier;
        }
    }
    bool canJump = false;
    Vector3 playerVelocity = Vector3.zero;
    // movement
    Vector2 inputAxis = Vector2.zero;
    private void Awake()
    {
        // get components
        animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogError("Player Character no animator found in children!");
        characterController = GetComponent<CharacterController>();
        if (characterController == null) Debug.LogError("PlayerCharacter is missing CharacterController!");
    }
    private void OnEnable()
    {
        // subscribe to input events
        if (buttonController == null) Debug.LogError("Player has no buttonController");
        else buttonController.OnButton += OnButtonPressed;
        if (joystickController == null) Debug.LogError("Player has no joystickController");
        else joystickController.OnMoveJoyStick += OnAxisInput;
    }
    private void OnDisable()
    {
        // unsubscribe to input events
        if (buttonController == null) Debug.LogWarning("Player has no buttonController");
        else buttonController.OnButton -= OnButtonPressed;
        if (joystickController == null) Debug.LogWarning("Player has no joystickController");
        else joystickController.OnMoveJoyStick -= OnAxisInput;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // reset velocity
        if (canJump && playerVelocity.y < 0)
        {
            playerVelocity = Vector3.zero;
        }
        // move character
        ApplyMovement();
    }
    // detect collisions
    void OnControllerColliderHit(ControllerColliderHit collision)
    {
        // check if we hit the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            // reset can jump
            canJump = true;
            playerVelocity = Vector3.zero;
        }
    }
    // move the character
    void ApplyMovement()
    {
        //transform input into camera space
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        // calc gravity
        playerVelocity.y += (Physics.gravity.y - GravityModifier) * Time.deltaTime;
        // calc movement vector
        Vector3 direction = (forward * inputAxis.y + right * inputAxis.x) * Speed;
        direction.y = 0; // zero out y
        direction *= Time.deltaTime; // factor for time
        // apply the movement
        characterController.Move(direction + playerVelocity);
        // face move direction
        if (inputAxis != Vector2.zero)
        {
            gameObject.transform.forward = direction.normalized;
        }
        // tell the animator how fast we are moving
        animator.SetFloat("Speed", direction.normalized.magnitude);
    }
    // character will jump
    void Jump()
    {
        if (canJump)
        {
            Debug.Log("Jump");
            // add upward velocity
            playerVelocity.y += Mathf.Sqrt(2 * JumpPower * -(Physics.gravity.y - GravityModifier));
            // add forward momentum based on input direction
            playerVelocity.x = inputAxis.x * JumpPower;
            playerVelocity.z = inputAxis.y * JumpPower;
            // tell the animator we jumped
            animator.SetTrigger("Jump");
            // prevent jumping until we hit the ground
            canJump = false;
        }
    }
    // catch button events from the button controller
    void OnButtonPressed(string ID)
    {
        Debug.Log("Player Catching Press" + ID);
        switch (ID)
        {
            case "X":
                ExecutePrimaryAbility();
                break;
            case "A":
            case "B":
            case "C":
                ExecuteActiveAbility(ID);
                break;
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
                ExecuteSpellAbility(ID);
                break;
            default:
                break;
        }
    }
    // catch joystick axis
    void OnAxisInput(Vector2 axis)
    {
        inputAxis = axis;
    }
    // Example button use
    void ExecutePrimaryAbility()
    {
        Debug.Log("Primary Ability");
        Jump();
    }
    // Example button use
    void ExecuteActiveAbility (string ID)
    {
        Debug.Log("Ability " + ID);
    }
    // Example button use
    void ExecuteSpellAbility(string ID)
    {
        Debug.Log("Spell " + ID);
    }
}