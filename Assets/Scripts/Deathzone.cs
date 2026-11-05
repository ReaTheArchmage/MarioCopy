using UnityEngine;
using UnityEngine.SceneManagement;

public class Deathzone : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
