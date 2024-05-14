using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public static int scoreCount;

  

    private void Update()
    {
        scoreText.text = "Score: " + Mathf.Round(scoreCount);
    }

    public static void ResetScore()
    {
        scoreCount = 0;
    }
}
