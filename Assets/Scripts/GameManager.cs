using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Runnable> runnables = new List<Runnable>();
    public TextMeshProUGUI titleText = null;
    public TextMeshProUGUI gameOverText = null;
    public float gameSpeedMultiplierMax = 0.0f;
    public float gameSpeedIncrementRate = 0.0f;

    public float GameSpeedMultiplier { get; private set; } = 1.0f;

    private Coroutine gameSpeedCoroutine = null;

    private enum GameState
    {
        TitleScreen,
        GameRunning,
        GameOver,
    }

    private GameState _state = GameState.TitleScreen;
    private GameState State
    {
        get => _state;
        set
        {
            if (_state == value)
            {
                return;
            }
            switch (value)
            {
                case GameState.TitleScreen:
                    Debug.LogError("Invalid GameState transition: Cannot transition to TitleScreen");
                    break;
                case GameState.GameRunning:
                    foreach (Runnable runnable in runnables)
                    {
                        runnable.GameRunning = true;
                    }
                    titleText.gameObject.SetActive(false);
                    gameOverText.gameObject.SetActive(false);
                    gameSpeedCoroutine = StartCoroutine(incrementGameSpeed());
                    _state = value;
                    break;
                case GameState.GameOver:
                    if (_state == GameState.GameRunning)
                    {
                        foreach (Runnable runnable in runnables)
                        {
                            runnable.GameRunning = false;
                        }
                        gameOverText.gameObject.SetActive(true);
                        StopCoroutine(gameSpeedCoroutine);
                        GameSpeedMultiplier = 1.0f;
                        _state = value;
                    }
                    else
                    {
                        Debug.LogError("Invalid GameState transition: Can only transition to GameOver from GameRunning");
                    }
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !(State == GameState.GameRunning))
        {
            State = GameState.GameRunning;
        }
    }

    public void GameOver()
    {
        State = GameState.GameOver;
    }

    private IEnumerator incrementGameSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            GameSpeedMultiplier = Mathf.Clamp(GameSpeedMultiplier + gameSpeedIncrementRate, 0.0f, gameSpeedMultiplierMax);
        }
    }
}
