using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public class State {
        public bool is_moving;
        public bool is_jumping;
        public bool is_powered_up;
    }
    public State state = new State();

    [SerializeField] MarioStats originalStats;
    MarioStats stats;
    public Rigidbody2D playerBody;
    public SpriteRenderer sprite;
    public Animator animator;
    public AudioSource[] sources;

    void Awake() {

        stats = Instantiate(originalStats);
        playerBody = GetComponent<Rigidbody2D>();
        init_scale = gameObject.transform.localScale;
    }

    void Update() {

        stats.delta = Time.deltaTime;
        SetState();
        UpdateAnimations();
        CheckPowerup(10.0f);

        if (stats.is_finish_line)
        {
            AnimateEnding();
        }
    }

    void FixedUpdate() {

        if (!stats.is_finish_line)
        {
            ApplyGravity();
            PlayerMove();
            PlayerJump();
        }
    }

    void SetState() {

        state.is_moving = (stats.inputMove != Vector2.zero) ? true : false;
        state.is_jumping = (Input.GetKeyDown(KeyCode.Space) && stats.is_on_floor) ? true : false;
    }

    void UpdateAnimations() {

        if (stats.inputMove.x == -1f)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }

        string targetAnim;

        if (!stats.is_on_floor)
        {
            targetAnim = "jump";
        }
        else if (stats.inputMove.x != 0f)
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

        if (!stats.is_on_floor)
        {
            stats.jump_timer += stats.delta;
            if (stats.jump_timer >= stats.jumpComedownDelay)
            {
                stats.inputMove.y = -1;
                stats.jump_timer = 0.0f;
            }
            if (stats.jump_timer == 0.0f && !stats.is_on_floor)
            {
                stats.jump_timer = 0.0f;
            }
        }
        else
        {
            stats.jump_timer = 0.0f;
        }
    }

    void PlayerMove() {

        if (state.is_moving)
        {
            playerBody.AddForce(
                stats.inputMove * stats.speedMultiplier, ForceMode2D.Impulse
            );
        }
    }

    void PlayerJump() {

        if (state.is_jumping)
        {
            stats.inputMove.y = 1;
            playerBody.AddForce(
                stats.inputMove * stats.jumpMultiplier, ForceMode2D.Impulse
            );

            sources[0].Play(); // Audio trigger
        }
    }

    public void OnMove(InputAction.CallbackContext context) {

        Vector2 tmp_context = context.ReadValue<Vector2>();
        stats.inputMove.x = tmp_context.x;
    }

    void AnimateEnding() {

        gameObject.transform.position = stats.anim_position;

        if (stats.anim_position.y >= 0.5f)
        {
            stats.anim_position.y -= stats.delta * stats.anim_speed;
        }
        else
        {
            stats.anim_position.x += stats.delta * stats.anim_speed;
        }
        if (stats.anim_position.x >= 185.0f)
        {
            SceneManager.LoadScene(2);
            stats.anim_position = Vector2.zero;
        }
    }

    public int coinAmt = 0;

    public void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.CompareTag("Powerup"))
        {
            state.is_powered_up = true;
            Destroy(collision.gameObject);
            healthCounter++;
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            coinAmt++;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            DamageToHealth();
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            stats.is_finish_line = true;
        }

        if (collision.gameObject.CompareTag("Death"))
        {
            sources[1].Play();
        }
    }

    public void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Floor"))
        {
            stats.is_on_floor = false;
        }
    }

    public void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Floor"))
        {
            stats.is_on_floor = true;
        }
    }

    // HEALTH CONTROL:

    public int healthCounter = 6;

    void DamageToHealth() {

        playerBody.AddForce(
            -stats.inputMove * stats.jumpMultiplier * 2, ForceMode2D.Impulse);

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
            powerup_timer += stats.delta;
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
}