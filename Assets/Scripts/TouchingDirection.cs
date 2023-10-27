using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    public ContactFilter2D castFilter;
    [SerializeField] private float groundDistance = 0.05f;
    [SerializeField] private float wallDistance = 0.2f;
    [SerializeField] private float ceilingDistance = 0.05f;

    [SerializeField] bool _isGrounded = true;
    [SerializeField] bool _isOnWall = false;
    [SerializeField] bool _isOnCeiling = false;

    CapsuleCollider2D touchingCol;
    Animator animator;
    RaycastHit2D[] groundHit = new RaycastHit2D[5];
    RaycastHit2D[] wallHit = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHit = new RaycastHit2D[5];
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    public bool IsGrounded { 
        get { return _isGrounded; }
        private set { 

            animator.SetBool(AnimationStrings.isGrounded, value);
            _isGrounded = value;
        } }

    public bool IsOnWall
    {
        get { return _isOnWall; }
        private set
        {

            animator.SetBool(AnimationStrings.isOnWall, value);
            _isOnWall = value;
        }
    }

    public bool IsOnCeiling
    {
        get { return _isOnCeiling; }
        private set
        {

            animator.SetBool(AnimationStrings.isOnCeiling, value);
            _isOnCeiling = value;
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHit, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHit, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHit, ceilingDistance) > 0;
    }
}
