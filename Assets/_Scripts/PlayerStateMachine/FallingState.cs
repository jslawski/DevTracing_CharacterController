using UnityEngine;

public class FallingState : MoveState
{
    private float _midAirMoveSpeed = 4.5f;
    private float _fallSpeed = 8.5f;

    public FallingState(Vector3 startingDirection) : base(startingDirection)
    {
        this._moveDirection = startingDirection;
    }

    public override void Enter(PlayerCharacter character)
    {
        base.Enter(character);

        this.character.isGrounded = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (this.character.isGrounded == true)
        {
            if (this._moveDirection.x == 0.0f)
            {
                this.character.ChangeState(new IdleState());
            }
            else
            {
                this.character.ChangeState(new RunState(this._moveDirection)); 
            }
        }
    }

    protected override void UpdateMoveVector()
    {
        float horizontalValue = this._moveDirection.x  * this._midAirMoveSpeed;// * Time.fixedDeltaTime;
        float verticalValue = -this._fallSpeed;// * Time.fixedDeltaTime;
        this._moveVector = new Vector3(horizontalValue, verticalValue, this._moveVector.z);
        
        this.character.UpdatePlayerVelocity(this._moveVector);
    }
}
