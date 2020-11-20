using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ScoreValueText;
    public Text HighScoreValueText;
    public Slider TimeSlider;
    public Image[] Health;
    public GameObject TitlePanel;
    public GameObject GameOverPanel;
    public GameObject HealthPrefab;
    public Transform HealthsTransform;

    private void Awake()
    {
        HighScoreValueText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    public void UpdateUI(GameStateSnapshot currentTickSnapshot)
    {
        SetScore(currentTickSnapshot.CurrentScore);
        SetHighScore(currentTickSnapshot.CurrentScore);
        SetHealth(currentTickSnapshot.CurrentHealth, currentTickSnapshot.MaxHealth);
        SetTime(currentTickSnapshot.TimeLeft, currentTickSnapshot.TimeLimit);
    }

    public void SetScore(int score)
    {
        ScoreValueText.text = score.ToString();
    }

    public void SetHighScore(int highScore)
    {
        int prevHighScore = PlayerPrefs.GetInt("HighScore", 0);
        int higherScore = Mathf.Max(prevHighScore, highScore);
        PlayerPrefs.SetInt("HighScore", higherScore);
        HighScoreValueText.text = higherScore.ToString();
    }

    public void SetHealth(int healthLeft, int maxHealth)
    {
        for(int i = 0; i < Health.Length; i++)
        {
            Health[i].gameObject.SetActive(i < healthLeft);
        }
    }

    public void SetTime(float timeLeft, float timeLimit)
    {
        TimeSlider.value = timeLeft / timeLimit;
    }

    public void ShowTitle(bool val)
    {
        TitlePanel.SetActive(val);
    }

    public void ShowGameOver(bool val)
    {
        GameOverPanel.SetActive(val);
    }
}
