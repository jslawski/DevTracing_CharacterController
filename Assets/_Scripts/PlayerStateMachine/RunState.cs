using UnityEngine;
using UnityEngine.InputSystem;

public class RunState : MoveState
{
    public RunState(Vector3 startingDirection) : base(startingDirection)
    {
        this._moveDirection = startingDirection;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (this.character.isGrounded == false)
        {
            this.character.ChangeState(new FallingState(this._moveDirection));
        }
    }

    protected override void UpdateMoveVector()
    {
        this._moveVector = this._moveDirection * _moveSpeed * Time.fixedDeltaTime;
        this.character.UpdatePlayerMoveVector(this._moveVector);
    }
}
