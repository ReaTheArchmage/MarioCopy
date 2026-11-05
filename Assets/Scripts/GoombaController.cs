using UnityEngine;

public class GoombaController : EntityController
{
    private float delta = 0.0f;
    public float speed = 4.0f;
    bool is_damage_anim = false;
    float damage_anim_timer = 0.0f;
    Vector3 init_scale = new Vector2();

    void Awake()
    {
        init_scale = gameObject.transform.localScale;
    }

    void Update() {

        delta = Time.deltaTime;
        Move(CycleDirection(delta) * speed);


        if (is_damage_anim)
        {
            damage_anim_timer += delta;
            if (damage_anim_timer >= 0.5f)
            {
                gameObject.transform.localScale = init_scale;
                is_damage_anim = false;
                damage_anim_timer = 0.0f;
            }
        }
    }

    string[] tags = {"Player", "Wall", "Enemy"};
    void OnCollisionEnter2D(Collision2D collision) {

        foreach (string tag in tags)
        {
            if (collision.gameObject.CompareTag(tag))
            {   
                if (tag == tags[0])
                {
                    gameObject.transform.localScale *= 1.2f;
                    is_damage_anim = true;
                }

                direction.x = -direction.x;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(tags[1])) {
                direction.x = -direction.x;
        }
    }
}