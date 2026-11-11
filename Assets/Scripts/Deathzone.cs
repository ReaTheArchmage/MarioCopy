using UnityEngine;
using UnityEngine.SceneManagement;

public class Deathzone : MonoBehaviour
{
    [SerializeField] AudioSource death;
    bool restart = false;
    float deathTimer = 0.0f;
    void Update() {
        if (restart == true)
        {

            death.Play();
            deathTimer += Time.deltaTime;
            if (deathTimer >= 3.0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                restart = false;
                deathTimer = 0.0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            restart = true;
        }
    }
}
