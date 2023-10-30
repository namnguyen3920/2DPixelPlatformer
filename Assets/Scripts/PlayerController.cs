using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]
public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    TouchingDirection touchingDirection;

    [SerializeField] public float walkSpeed = 5.0f;
    [SerializeField] private float runSpeed = 9.0f;
    [SerializeField] private float onAirSpeed = 2.0f;
    [SerializeField] private float jumpImpulse = 6.0f;
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isRunning = false;
    [SerializeField] private bool _isFacingRight = true;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
    }

    private float CurrentMoveSpeed{ get{
            if (CanMove) {
                if (IsMoving && !touchingDirection.IsOnWall)
                {
                    if (touchingDirection.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else return walkSpeed;
                    }
                    else
                    {
                        return onAirSpeed;
                    }
                }
                else return 0; //stand still
            } else
            {
                return 0;  //lock movement
            }
        }
        
    }
    public bool IsMoving { get{
        return _isMoving;
    } private set{
        _isMoving = value;
        animator.SetBool(AnimationStrings.isMoving, value);
    } }

    public bool IsRunning { get{
        return _isRunning;
    } private set{
        _isRunning = value;
        animator.SetBool(AnimationStrings.isRunning, value);
    } }

    public bool IsFacingRight { get{
        return _isFacingRight;
    } private set{
        if(_isFacingRight != value){
            //flip the local scale
            transform.localScale *= new Vector2(-1, 1);
        }
        _isFacingRight = value;
    } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        SetFacingDirection(moveInput);
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0f && !IsFacingRight){
            IsFacingRight = true;
        }else if (moveInput.x < 0f && IsFacingRight){
            IsFacingRight = false;
        }
    }

    public bool CanMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        } }
    public void OnMove (InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;
        
    }
    public void OnRun (InputAction.CallbackContext context){
        if(context.started){
            IsRunning = true;
        }else if (context.canceled){
            IsRunning = false;
        }
    }
    
    public void OnJump (InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGrounded && CanMove) {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack (InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);

        }
    }
}
