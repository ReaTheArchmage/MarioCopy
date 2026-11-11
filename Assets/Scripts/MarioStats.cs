using UnityEngine;

[CreateAssetMenu(fileName = "MarioStats", menuName = "Stats/MarioStats")]
public class MarioStats : ScriptableObject
{
    [Header("Movement Tuning")]
    public float speedMultiplier = 10f;
    public float jumpMultiplier = 10f;
    public float jumpComedownDelay = 0.2f;

    [Header("Runtime Values")]
    public float delta;
    public Vector2 inputMove;
    public bool is_on_floor;
    public bool is_finish_line;
    public float jump_timer;

    [Header("Ending Animation")]
    public Vector2 anim_position;
    public float anim_speed = 50f;

    public bool is_killing_goomba;
}
