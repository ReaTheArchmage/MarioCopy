using UnityEngine;

public class GoombaController : EntityController
{
    private float delta = 0.0f;
    public float speed = 4.0f;

    void Awake() {
        
    }
    void FixedUpdate() {

        delta = Time.deltaTime;
        Move(CycleDirection(delta) * speed);
    }

    string[] tags = {"Player", "Wall", "Enemy"};
    void OnCollisionEnter2D(Collision2D collision) {
        
        foreach (string tag in tags)
        {
            if (collision.gameObject.CompareTag(tag))
            {   
                direction.x = -direction.x;
            }
        }
    }
}
