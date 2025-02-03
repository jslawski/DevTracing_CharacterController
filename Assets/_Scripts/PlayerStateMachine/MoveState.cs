using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : PlayerState
{
    protected Vector3 _moveDirection = Vector3.zero;

    protected Vector3 _moveVector = Vector3.zero;

    protected float _moveSpeed = 4.5f;

    private float spriteOffset = 0.06f;

    public MoveState(Vector3 startingDirection)
    {
        this._moveDirection = startingDirection;
    }

    public override void Enter(PlayerCharacter character)
    {
        base.Enter(character);

        this.character.playerControls.PlayerMap.MoveLeft.canceled += this.MoveLeftOrRightCanceled;
        this.character.playerControls.PlayerMap.MoveLeft.performed += this.MoveLeftPerformed;
        this.character.playerControls.PlayerMap.MoveRight.performed += this.MoveRightPerformed;
        this.character.playerControls.PlayerMap.MoveRight.canceled += this.MoveLeftOrRightCanceled;
    }

    public override void Exit()
    {
        base.Exit();

        this.character.playerControls.PlayerMap.MoveLeft.canceled -= this.MoveLeftOrRightCanceled;
        this.character.playerControls.PlayerMap.MoveLeft.performed -= this.MoveLeftPerformed;
        this.character.playerControls.PlayerMap.MoveRight.performed -= this.MoveRightPerformed;
        this.character.playerControls.PlayerMap.MoveRight.canceled -= this.MoveLeftOrRightCanceled;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        this.UpdateMoveVector();
        this.UpdateSprite();
    }

    protected virtual void UpdateMoveVector() { }

    private void UpdateSprite()
    {
        if (this._moveDirection == Vector3.left)
        {
            this.character.spriteRenderer.flipX = true;
            this.character.spriteRenderer.gameObject.transform.localPosition = new Vector3(this.spriteOffset, 0.0f, 0.0f);

        }

        if (this._moveDirection == Vector3.right)
        {
            this.character.spriteRenderer.flipX = false;
            this.character.spriteRenderer.gameObject.transform.localPosition = new Vector3(-this.spriteOffset, 0.0f, 0.0f);
        }
    }

    private void MoveLeftPerformed(InputAction.CallbackContext context)
    {
        this._moveDirection = Vector3.left;
    }

    private void MoveRightPerformed(InputAction.CallbackContext context)
    {
        this._moveDirection = Vector3.right;
    }

    private void MoveLeftOrRightCanceled(InputAction.CallbackContext context)
    {
        if (this.character.playerControls.PlayerMap.MoveLeft.inProgress == true)
        {
            this._moveDirection = Vector3.left;
        }
        else if (this.character.playerControls.PlayerMap.MoveRight.inProgress == true)
        {
            this._moveDirection = Vector3.right;
        }
        else
        {
            this.character.ChangeState(new IdleState());
        }
    }
}
