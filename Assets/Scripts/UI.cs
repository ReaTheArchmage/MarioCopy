using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
  [SerializeField] GameObject timerLabel;
  [SerializeField] GameObject livesLabel;
  [SerializeField] PlayerMovement coinAmt;

  int coinAmount = 0;

  TextMeshProUGUI timerText;
  float timer = 400f; 
  [SerializeField] GameObject coinsLabel; 
  TextMeshProUGUI coinsText;
  TextMeshProUGUI livesText; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        timerText = timerLabel.GetComponent<TextMeshProUGUI>();
        coinsText = coinsLabel.GetComponent<TextMeshProUGUI>();
        livesText = livesLabel.GetComponent<TextMeshProUGUI>();

    }

    void Update()
    {
        timer -= Time.deltaTime;
        int int_timer = (int)timer;
        coinAmount = coinAmt.coinAmt;

        livesText.text = coinAmt.healthCounter.ToString();
        timerText.text = int_timer.ToString();
        coinsText.text = "x" + coinAmount.ToString();
    }
}
