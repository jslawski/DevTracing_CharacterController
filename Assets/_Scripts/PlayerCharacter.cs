using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    public Transform debugBoxCast;

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

        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        Debug.LogError("State: " + this.currentState.GetType().ToString());
    }

    private void FixedUpdate()
    {   
        this.currentState.FixedUpdateState();

        this._playerTransform.Translate(this._moveVector);

        this.HandleCollision();

        this.UpdateIsGrounded();
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

    private void UpdateIsGrounded()
    {
        this.isGrounded = false;
    
        Collider[] colliders = Physics.OverlapBox(this._playerCollider.bounds.center, this._playerCollider.bounds.extents, Quaternion.identity, this.collisionLayerMask);
    
        foreach (Collider collider in colliders) 
        {
            if (collider.bounds.max.y >= this._playerCollider.bounds.min.y)
            {
                this.isGrounded = true;
            }
        }
    }    
    
    private void HandleCollision()
    {
        Collider[] colliders = Physics.OverlapBox(this._playerCollider.bounds.center, this._playerCollider.bounds.extents, Quaternion.identity, this.collisionLayerMask);

        foreach (Collider collidedObject in colliders)
        {
            float left = this._playerCollider.bounds.min.x - collidedObject.bounds.max.x;
            float right = this._playerCollider.bounds.max.x - collidedObject.bounds.min.x;
            float top = collidedObject.bounds.max.y - this._playerCollider.bounds.min.y;
            float bottom = collidedObject.bounds.min.y - this._playerCollider.bounds.max.y;

            this.ResolveCollision(left, right, top, bottom);
        }
    }

    private void ResolveCollision(float left, float right, float top, float bottom)
    {
        float minimumValue = float.PositiveInfinity;
        Vector3 penetrationVector = Vector3.zero;

        if (Mathf.Abs(left) < minimumValue)
        {            
            minimumValue = Mathf.Abs(left);
            if (this._moveVector.x < 0.0f)
            {
                penetrationVector = new Vector3(-left - this._moveVector.x, 0.0f);
            }
            else
            {
                penetrationVector = new Vector3(-left, 0.0f);
            }
        }
        if (Mathf.Abs(right) < minimumValue)
        {
            minimumValue = Mathf.Abs(right);
            if (this._moveVector.x > 0.0f)
            {
                penetrationVector = new Vector3(-right - this._moveVector.x, 0.0f);
            }
            else
            {
                penetrationVector = new Vector3(-right, 0.0f);
            }
        }
        if (Mathf.Abs(top) < minimumValue)
        {
            minimumValue = Mathf.Abs(top);
            if (this._moveVector.y < 0.0f)
            {
                penetrationVector = new Vector3(0.0f, top - this._moveVector.y);
            }
            else
            {
                penetrationVector = new Vector3(0.0f, top);
            }
        }
        if (Mathf.Abs(bottom) < minimumValue)
        {
            minimumValue = Mathf.Abs(bottom);
            if (this._moveVector.y > 0.0f)
            {
                penetrationVector = new Vector3(0.0f, bottom - this._moveVector.y);
            }
            else
            {
                penetrationVector = new Vector3(0.0f, bottom);
            }
        }

         this._playerTransform.Translate(penetrationVector);
    }    
}

