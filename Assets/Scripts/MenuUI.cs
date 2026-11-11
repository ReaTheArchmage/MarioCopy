using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{

    [SerializeField] GameObject reviews;
    public float rev_scroll_speed = 20f;
    Vector2 initPos;
    Vector2 changingPos;
    RectTransform rect;

    float maxX = -500.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rect = reviews.GetComponent<RectTransform>();
        initPos = rect.position; 
    }

    // Update is called once per frame
    void Update()
    {
        changingPos.x -= Time.deltaTime * rev_scroll_speed;
        if (changingPos.x <= maxX)
        {
            changingPos.x = 1000f;    
        }

        reviews.transform.position = changingPos;
    }

    public void LaunchLevelOne() {
        SceneManager.LoadScene(1);
    }
    public void LaunchLevelTwo() {
        SceneManager.LoadScene(2);
    }
    public void Quit() {
        Application.Quit();
    }
}
