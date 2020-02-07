using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Runnable : MonoBehaviour
{
    private bool _gameRunning = false;
    public bool GameRunning {
        get => _gameRunning;
        set {
            if (_gameRunning == value)
            {
                return;
            }
            IEnumerator updateGameRunning(bool newValue)
            {
                yield return new WaitForEndOfFrame();
                _gameRunning = newValue;
            }
            StartCoroutine(updateGameRunning(value));
            switch (value)
            {
                case true:
                    RunComponent();
                    break;
                case false:
                    StopComponent();
                    break;
            }
        }
    }

    protected abstract void RunComponent();
    protected abstract void StopComponent();
}
