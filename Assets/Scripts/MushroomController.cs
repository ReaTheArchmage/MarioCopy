using UnityEngine;

public class MushroomController : EntityController
{
    struct mushroomState {
        public bool is_in_animation, is_moving;
    }
    mushroomState state;
    
    [SerializeField] bool canMove = true;
    [SerializeField] string playerTag = "Player";
    [SerializeField]
    private float animationSpeed;
    [SerializeField] private GameObject mushroomObj;
    [SerializeField] private float mushshroomSpeed;
    private Vector2 mushroomPos, initialMPos;
    private float delta = 0.0f;
    private bool animationEnded;

    void Awake() {
        direction.x = 1;
        mushroomPos = mushroomObj.transform.position;
        initialMPos = mushroomPos;
    }

    void FixedUpdate() {
        
        delta = Time.deltaTime;
        
        if (body != null) {
            if (state.is_in_animation && !animationEnded) AnimateMushroom();
            else if (canMove && state.is_moving)  Move(CycleDirection(delta, 3.0f) * mushshroomSpeed);
        }
    }

    void AnimateMushroom() {

        if (mushroomPos.y >= initialMPos.y + 1.0f)
        {
            animationEnded = true;
            state.is_moving = true;
            state.is_in_animation = false;
            body.simulated = true;
        }
        else if (!animationEnded)
        {
            mushroomPos.y += delta * animationSpeed;
            mushroomObj.transform.position = mushroomPos;    
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.gameObject.CompareTag(playerTag) && !animationEnded)
        {   
            state.is_in_animation = true;
        }
    }
}
