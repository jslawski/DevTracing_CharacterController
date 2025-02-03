using UnityEngine;
using UnityEngine.InputSystem;

public class RunState : MoveState
{
    public RunState(Vector3 startingDirection) : base(startingDirection)
    {
        this._moveDirection = startingDirection;
    }

    public override void Enter(PlayerCharacter character)
    {
        base.Enter(character);

        this.character.playerControls.PlayerMap.Jump.performed += this.ChangeToJump;
    }

    public override void Exit()
    {
        base.Exit();

        this.character.playerControls.PlayerMap.Jump.performed -= this.ChangeToJump;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (this.character.isGrounded == false)
        {
            this.character.ChangeState(new FallingState(this._moveDirection));
        }
    }

    protected override void UpdateMoveVector()
    {
        this._moveVector = this._moveDirection * this._moveSpeed;// * Time.fixedDeltaTime;
        this.character.UpdatePlayerVelocity(this._moveVector);
    }

    private void ChangeToJump(InputAction.CallbackContext context)
    {
        
    }
}
