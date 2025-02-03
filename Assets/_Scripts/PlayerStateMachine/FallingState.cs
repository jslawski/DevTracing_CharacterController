using UnityEngine;
using UnityEngine.InputSystem;

public class FallingState : MoveState
{
    private float _midAirMoveSpeed = 4.5f;
    private float _fallSpeed = 8.5f;

    private bool _jumpInitiated = false;
    private float _currentJumpBuffer = 0.0f;
    private float _maxJumpBuffer = 0.2f;

    public FallingState(Vector3 startingDirection) : base(startingDirection)
    {
        this._moveDirection = startingDirection;
    }

    public override void Enter(PlayerCharacter character)
    {
        base.Enter(character);

        this.character.isGrounded = false;

        this.character.playerControls.PlayerMap.Jump.performed += this.InitiateJumpBuffer;
    }

    public override void Exit()
    {
        base.Exit();

        this.character.playerControls.PlayerMap.Jump.performed -= this.InitiateJumpBuffer;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (this.character.isGrounded == true)
        {
            if (this._jumpInitiated == true)
            {
                this.character.ChangeState(new JumpState(this._moveDirection));
            }        
            else if (this._moveDirection.x == 0.0f)
            {
                this.character.ChangeState(new IdleState());
            }
            else
            {
                this.character.ChangeState(new RunState(this._moveDirection)); 
            }
        }

        this.UpdateJumpBuffer();
    }

    protected override void UpdateMoveVector()
    {
        float horizontalValue = this._moveDirection.x  * this._midAirMoveSpeed;// * Time.fixedDeltaTime;
        float verticalValue = -this._fallSpeed;// * Time.fixedDeltaTime;
        this._moveVector = new Vector3(horizontalValue, verticalValue, this._moveVector.z);
        
        this.character.UpdatePlayerVelocity(this._moveVector);
    }

    private void UpdateJumpBuffer()
    {
        if (this._jumpInitiated)
        {
            if (this._currentJumpBuffer < this._maxJumpBuffer)
            {
                this._currentJumpBuffer += Time.deltaTime;
            }
            else
            {
                this._jumpInitiated = false;
            }
        }
    }

    private void InitiateJumpBuffer(InputAction.CallbackContext context)
    {
        this._jumpInitiated = true;
        this._currentJumpBuffer = 0.0f;
    }
}
