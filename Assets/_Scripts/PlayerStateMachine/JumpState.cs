using UnityEngine;
using UnityEngine.InputSystem;

public class JumpState : MoveState
{

    private float _initialJumpHeight = 0.45f;
    private float _jumpSpeed = 4.22f;
    private float _midAirMoveSpeed = 4.5f;

    private float _currentJumpTime = 0.0f;
    private float _maxJumpTime = 0.289f;

    private bool _isHanging = false;

    private int _currentHangingFrames = 0;
    private int _totalHangingFrames = 5;

    public JumpState(Vector3 startingDirection) :base(startingDirection)
    {
        this._moveDirection = startingDirection;
    }

    private Vector3 GetVelocityVector()
    {
        if (this._isHanging == true)
        {
            return new Vector3(this._moveDirection.x * this._midAirMoveSpeed, 0.0f, 0.0f);
        }
        else
        {
            return new Vector3(this._moveDirection.x * this._midAirMoveSpeed, this._jumpSpeed, 0.0f);
        }
    }

    public override void Enter(PlayerCharacter character)
    {
        base.Enter(character);

        this.character.playerControls.PlayerMap.Jump.canceled += this.ChangeToFalling;

        this.character.IncrementPlayerRigidbodyPosition(new Vector3(0.0f, this._initialJumpHeight, 0.0f));

        this.character.isGrounded = false;
    }

    public override void Exit()
    {
        base.Exit();

        this.character.playerControls.PlayerMap.Jump.canceled -= this.ChangeToFalling;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        this.character.UpdatePlayerVelocity(this.GetVelocityVector());

        if (this._currentJumpTime >= this._maxJumpTime)
        {
            this._isHanging = true;    

            if (this._currentHangingFrames >= this._totalHangingFrames)
            {
                this.character.ChangeState(new FallingState(this._moveDirection));
            }
            else
            {
                this.character.UpdatePlayerVelocity(this.GetVelocityVector());
            }
        }

        this._currentJumpTime += Time.deltaTime;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (this._isHanging == true)
        {
            this._currentHangingFrames++;
        }
    }

    private void ChangeToFalling(InputAction.CallbackContext context)
    {
        this.character.ChangeState(new FallingState(this._moveDirection));
    }

    protected override void MoveLeftOrRightCanceled(InputAction.CallbackContext context)
    {
        if (this.character.playerControls.PlayerMap.MoveLeft.inProgress == true)
        {
            this._moveDirection = Vector3.left;
        }
        else if (this.character.playerControls.PlayerMap.MoveRight.inProgress == true)
        {
            this._moveDirection = Vector3.right;
        }
    }
}
