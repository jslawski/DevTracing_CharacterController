using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    private Transform _playerTransform;

    private Collider _playerCollider;

    private Vector3 _moveVector;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    public PlayerControls playerControls;

    public PlayerState currentState;

    public bool isGrounded = false;

    public LayerMask collisionLayerMask;

    private void Awake()
    {
        this._playerTransform = GetComponent<Transform>();
        this._playerCollider = GetComponentInChildren<Collider>();
        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        this.playerControls = new PlayerControls();

        this.ChangeState(new IdleState());
    }

    private void OnEnable()
    {
        this.playerControls.Enable();
    }

    private void OnDisable()
    {
        this.playerControls.Disable();
    }

    private void Update()
    {
        this.currentState.UpdateState();

        Debug.LogError("State: " + this.currentState.GetType().ToString());
    }

    private void FixedUpdate()
    {       
        this.currentState.FixedUpdateState();

        this.HandleWallCollision();
        this.HandleGroundCollision();


        this._playerTransform.Translate(this._moveVector);

       //this.UpdateIsGrounded();
    }

    public void UpdatePlayerMoveVector(Vector3 moveDirection)
    {
        this._moveVector = moveDirection;
    }

    public void ChangeState(PlayerState newState)
    {
        if (this.currentState != null)
        {
            this.currentState.Exit();
        }

        this.currentState = newState;
        this.currentState.Enter(this);
    }

    private void HandleWallCollision()
    {    
        RaycastHit hitInfo;
        if (Physics.BoxCast(this._playerTransform.position, this._playerCollider.bounds.extents * 0.9f, this._moveVector.normalized, out hitInfo, Quaternion.identity, Mathf.Abs(this._moveVector.x)) == true)
        {
            float xDifference = Mathf.Abs(hitInfo.point.x - this._playerTransform.position.x) - this._playerCollider.bounds.extents.x;

            if (this._moveVector.x > 0.0f)
            {
                this._moveVector = new Vector3(xDifference, this._moveVector.y, this._moveVector.z);
            }
            else if (this._moveVector.x < 0.0f)
            {
                this._moveVector = new Vector3(-xDifference, this._moveVector.y, this._moveVector.z);
            }
        }
    }

    private void HandleGroundCollision()
    {
        Vector3 startingPoint = this._playerTransform.position;
        Vector3 endPoint = this._playerTransform.position + (Vector3.down * Mathf.Abs(this._moveVector.y));

        Debug.DrawLine(startingPoint, endPoint, Color.cyan, 10.0f);

        RaycastHit hitInfo;
        if (Physics.BoxCast(this._playerTransform.position, this._playerCollider.bounds.extents, Vector3.down, out hitInfo, Quaternion.identity, Mathf.Abs(this._moveVector.y)) == true)
        {
            float yDifference = Mathf.Abs(hitInfo.point.y - this._playerTransform.position.y) - this._playerCollider.bounds.extents.y;

            this._moveVector = new Vector3(this._moveVector.x, -yDifference, this._moveVector.z);

            this.isGrounded = true;
        }
    }

    private void UpdateIsGrounded()
    {    
        this.isGrounded = Physics.BoxCast(this._playerTransform.position, this._playerCollider.bounds.extents, Vector3.down, Quaternion.identity, this._playerCollider.bounds.extents.y);

        //this.isGrounded = Physics.Raycast(this._playerTransform.position, Vector3.down, this._playerCollider.bounds.extents.y);

        //Vector3 startingPoint = this._playerTransform.position;
        //Vector3 endPoint = this._playerTransform.position + (Vector3.down * this._playerCollider.bounds.extents.y);

        //Debug.DrawLine(startingPoint, endPoint, Color.red);
    }
}

