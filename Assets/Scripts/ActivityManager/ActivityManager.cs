using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivityManager : MonoBehaviour
{
    protected bool HasReachedProtocol
    {
        get
        {
            return HeadsetManager.LiveProtocol >= DifficultyManager.Difficulty;
        }
    }

    protected abstract void UpdateBehaviour(bool state);
    


    private void Update()
    {
        UpdateBehaviour(HasReachedProtocol);
    }
}
