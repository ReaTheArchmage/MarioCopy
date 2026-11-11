using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject camera;
    [SerializeField] PlayerMovement player;
    private float dist_from_camera;

    void MoveCamera(float cam_offset_x = 17.0f, float max_dist_from_cam = 6.0f) {

        dist_from_camera = camera.transform.position.magnitude - player.transform.position.magnitude;

        Vector3 new_pos = camera.transform.position;
        new_pos.x += 17.0f;

        if (dist_from_camera <= -max_dist_from_cam && camera.transform.position != new_pos)
        {
            camera.transform.position = new_pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }
}
