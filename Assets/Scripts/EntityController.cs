using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D body;
    float dirChangeTimer = 0.0f;
    public bool directionSwitch = false;
    public Vector2 direction;
    
    public void Move(Vector2 direction) {
        body.AddForce(direction * 3);
    }

    public Vector2 CycleDirection(float delta, float duration = 6.0f) {
        
        dirChangeTimer += delta;
        if (dirChangeTimer >= duration)
        {
            directionSwitch = !directionSwitch;
            direction.x = (directionSwitch) ? 1 : -1;
            dirChangeTimer = 0.0f;
        }

        return direction;
    }
}
