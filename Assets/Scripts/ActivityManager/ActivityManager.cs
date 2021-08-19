using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivityManager : MonoBehaviour
{
    float time = 0f;
    protected bool HasReachedProtocol
    {
        get
        {
            time += Time.deltaTime;
            if(time > 0.5f)
            {
                Debug.Log($"LiveProtocol: {HeadsetManager.LiveProtocol}\nDifficulty: {DifficultyManager.Difficulty}");
                time = 0;
            }
            return HeadsetManager.LiveProtocol >= DifficultyManager.Difficulty;
        }
    }

    protected abstract void UpdateBehaviour(bool state);

    private void Update()
    {
        UpdateBehaviour(HasReachedProtocol);
    }
}
