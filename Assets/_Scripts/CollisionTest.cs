using UnityEngine;

public struct MinkowskiDifference
{
    public Vector2 xValues;
    public Vector2 yValues;
}

public class CollisionTest : MonoBehaviour
{
    public Collider playerCollider;
    public Collider otherCollider;

    public MinkowskiDifference currentDifference;

    float top = 0.0f;
    float bottom = 0.0f;
    float left = 0.0f;
    float right = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.currentDifference = new MinkowskiDifference();
        this.currentDifference.xValues = new Vector2();
        this.currentDifference.yValues = new Vector2();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.CalculateDifference();

        if (this.left <= 0.0f && this.right > 0.0f && this.bottom <= 0.0f && this.top >= 0.0f)
        {
            Debug.LogError("Colliding!");

            DisplayPenetrationVector();
        }
    }

    private void CalculateDifference()
    {
        this.left = this.playerCollider.bounds.min.x - this.otherCollider.bounds.max.x;
        this.right = this.playerCollider.bounds.max.x - this.otherCollider.bounds.min.x;
        this.top = this.otherCollider.bounds.max.y - this.playerCollider.bounds.min.y;
        this.bottom = this.otherCollider.bounds.min.y - this.playerCollider.bounds.max.y;

        this.currentDifference.yValues.x = this.otherCollider.bounds.max.y - this.playerCollider.bounds.min.y;
        this.currentDifference.yValues.y = this.playerCollider.bounds.max.y + this.otherCollider.bounds.max.y;
    }

    private void DisplayPenetrationVector()
    {
        float minimumValue = Mathf.Abs(this.left);
        Vector3 penetrationVector = new Vector3(-this.left, 0.0f);

        Vector3 debugCollisionPoint = new Vector3(this.playerCollider.bounds.min.x, 0.0f, 0.0f);

        if (Mathf.Abs(this.right) < minimumValue)
        {
            minimumValue = Mathf.Abs(this.right);
            penetrationVector = new Vector3(-this.right, 0.0f);
            debugCollisionPoint = new Vector3(this.playerCollider.bounds.max.x, 0.0f, 0.0f);
        }
        if (Mathf.Abs(this.top) < minimumValue)
        { 
            minimumValue = Mathf.Abs(this.top);
            penetrationVector = new Vector3(0.0f, this.top);
            debugCollisionPoint = new Vector3(0.0f, this.playerCollider.bounds.min.y, 0.0f);
        }
        if (Mathf.Abs(this.bottom) < minimumValue)
        {
            minimumValue = Mathf.Abs(this.bottom);
            penetrationVector = new Vector3(0.0f, this.bottom);
            debugCollisionPoint = new Vector3(0.0f, this.playerCollider.bounds.max.y, 0.0f);
        }

        Debug.DrawLine(debugCollisionPoint, penetrationVector + debugCollisionPoint, Color.cyan);
    }
}
