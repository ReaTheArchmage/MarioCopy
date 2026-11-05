using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private AudioSource[] sources;
    [SerializeField] private SpriteRenderer sprite;

    [SerializeField] private float speedMultiplier, jumpMultiplier;
    [SerializeField] private float jumpComedownDelay;
    [SerializeField] private Animator animator;

    private Rigidbody2D playerBody = null;
    private Vector2 inputMove;
    struct moveState {
        public bool is_moving, is_jumping, external, is_powered_up;
    }
    moveState state;
    private bool is_on_floor = false;
    private bool is_finish_line = false;
    private bool is_killing_goomba = false;
    private float jump_timer = 0.0f;
    private float delta = 0.0f;
    private Vector2 anim_position = new Vector2(177.0f, 7.6f);
    float anim_speed = 4.0f;

    void Awake() {

        playerBody = GetComponent<Rigidbody2D>();
        init_scale = gameObject.transform.localScale;
    }

    void Update() {

        delta = Time.deltaTime;
        SetState();
        UpdateAnimations();
        CheckPowerup(10.0f);
        MoveCamera();
        if (is_finish_line)
        {
            AnimateEnding();
        }
    }

    void FixedUpdate() {

        if (!is_finish_line)
        {

            ApplyGravity();
            PlayerJump();
            PlayerMove();
        }
    }

    void SetState() {

        state.is_moving = (inputMove != Vector2.zero) ? true : false;
        state.is_jumping = (Input.GetKeyDown(KeyCode.Space) && is_on_floor) ? true : false;
    }

void UpdateAnimations() {

    if (inputMove.x == -1f)
    {
        sprite.flipX = true;
    }
    else
    {
        sprite.flipX = false;        
    }

    string targetAnim;

    if (!is_on_floor)
    {
        targetAnim = "jump";
    }
    else if (inputMove.x != 0f)
    {
        targetAnim = "anim";
    }
    else
    {
        targetAnim = "idle";
    }

    if (!animator.GetCurrentAnimatorStateInfo(0).IsName(targetAnim))
    {
        animator.Play(targetAnim);
    }
}

    void ApplyGravity() {

        if (!is_on_floor)
        {
            jump_timer += delta;
            if (jump_timer >= jumpComedownDelay)
            {
                inputMove.y = -1;
                jump_timer = 0.0f;
            }
            if (jump_timer == 0.0f && !is_on_floor)
            {
                jump_timer = 0.0f;
            }
        }
        else
        {
            jump_timer = 0.0f;
        }
    }

    void PlayerMove() {

        if (state.is_moving)
        {
            playerBody.AddForce(
                inputMove * speedMultiplier, ForceMode2D.Impulse
            );
        }
    }

    void PlayerJump() {

        if (state.is_jumping)
        {
            inputMove.y = 1;
            playerBody.AddForce(
                inputMove * jumpMultiplier, ForceMode2D.Impulse
            );

            sources[0].Play();
        }
    }

    public void OnMove(InputAction.CallbackContext context) {

        Vector2 tmp_context = context.ReadValue<Vector2>();
        inputMove.x = tmp_context.x;
    }

    void AnimateEnding() {

        gameObject.transform.position = anim_position;

        if (anim_position.y >= 0.5f)
        {
            anim_position.y -= delta * anim_speed;
        }
        else
        {
            anim_position.x += delta * anim_speed;
        }
        if (anim_position.x >= 185.0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            anim_position = Vector2.zero;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.CompareTag("Powerup"))
        {
            state.is_powered_up = true;
            Destroy(collision.gameObject);
            healthCounter++;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            DamageToHealth();
        }        

        if (collision.gameObject.CompareTag("Finish"))
        {
            is_finish_line = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Floor"))
        {
            is_on_floor = false;
        }
    }

    public void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Floor"))
        {
            is_on_floor = true;
        }
    }

    // HEALTH CONTROL:

    private int healthCounter = 6;

    void DamageToHealth() {

        playerBody.AddForce(
            -inputMove * jumpMultiplier * 2, ForceMode2D.Impulse);

        healthCounter--;
        if (healthCounter <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // POWER UP CONTROL:

    private float powerup_timer = 0.0f;
    Vector3 init_scale;

    void CheckPowerup(float duration) {
        Vector3 new_scale = init_scale * 2;

        if (state.is_powered_up == true) {
            powerup_timer += delta;
            gameObject.transform.localScale = new_scale;
        }
        CheckPowerupEnd(duration);
    }

    void CheckPowerupEnd(float duration) {

        if (powerup_timer >= duration)
        {
            gameObject.transform.localScale = init_scale;
            powerup_timer = 0.0f;
            state.is_powered_up = false;
        }
    }

    void AnimatePowerup() {
        gameObject.transform.localScale = init_scale;
    }

    // CAMERA CONTROL:

    [SerializeField] GameObject camera;
    private float dist_from_camera;

    void MoveCamera(float cam_offset_x = 17.0f, float max_dist_from_cam = 6.0f) {

        dist_from_camera = camera.transform.position.magnitude - gameObject.transform.position.magnitude;

        Vector3 new_pos = camera.transform.position;
        new_pos.x += 17.0f;

        if (dist_from_camera <= -max_dist_from_cam && camera.transform.position != new_pos)
        {
            camera.transform.position = new_pos;
        }
    }
}