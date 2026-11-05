using UnityEngine;

public class Weakspot : MonoBehaviour
{

    [SerializeField] GameObject enemy;
    void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(enemy);
        }
    }
}
