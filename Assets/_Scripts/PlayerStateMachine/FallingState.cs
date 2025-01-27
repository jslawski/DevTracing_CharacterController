using UnityEngine;

public class FallingState : MoveState
{
    private float _midAirMoveSpeed = 4.5f;
    private float _fallSpeed = 8.5f;

    public FallingState(Vector3 startingDirection) : base(startingDirection)
    {
        this._moveDirection = startingDirection;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (this.character.isGrounded == true)
        {
            this.character.ChangeState(new IdleState());
        }
    }

    protected override void UpdateMoveVector()
    {
        float horizontalValue = this._moveDirection.x * this._midAirMoveSpeed * Time.fixedDeltaTime;
        float verticalValue = -this._fallSpeed * Time.fixedDeltaTime;
        this._moveVector = new Vector3(horizontalValue, verticalValue, this._moveVector.z);
        
        this.character.UpdatePlayerMoveVector(this._moveVector);
    }
}
