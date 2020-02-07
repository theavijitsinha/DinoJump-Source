using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Runnable
{
    public TextMeshProUGUI textComponent = null;

    private int score = 0;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            string scoreText = string.Format("Score: {0:D5}", Score);
            textComponent.text = scoreText;
        }
    }

    private Coroutine updateScoreCoroutine = null;

    protected override void RunComponent()
    {
        Score = 0;
        updateScoreCoroutine = StartCoroutine(updateScore());
    }

    protected override void StopComponent()
    {
        StopCoroutine(updateScoreCoroutine);
    }

    private IEnumerator updateScore()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            Score++;
        }
    }
}
