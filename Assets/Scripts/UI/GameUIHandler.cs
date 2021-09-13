using UnityEngine;
using UnityEngine.UI;

public class GameUIHandler : MonoBehaviour
{
    [SerializeField]
    private Text score;
    [SerializeField]
    private Text timeLeft;
    [SerializeField]
    private Text headState;


    private static readonly Color POOR_SIGNAL_COLOR = new Color(253,88,0);


    void Start()
    {
        HeadsetManager.HeadsetDisconnected += ChangeStateTextOnDisconnect;
        HeadsetManager.HeadsetConnected += ChangeStateTextOnConnect;
        HeadsetManager.UpdatePoorSignalEvent += ChangeStateTextOnPoorSignal;
        AstronautManager.UpdateScoreEvent += OnScoreChanged;
        AstronautManager.UpdateTimeLeftEvent += OnTimeLeftChanged;
    }

    private void ChangeStateTextOnDisconnect()
    {
        headState.text = "Disconnected";
        headState.color = Color.red;
    }

    private void ChangeStateTextOnConnect()
    {
        headState.text = "Connected";
        headState.color = Color.green;
    }

    private void ChangeStateTextOnPoorSignal(int value)
    {
        if (value == 0)
        {
            ChangeStateTextOnConnect();
            return;
        }
        headState.text = "Weak Signal";
        headState.color = POOR_SIGNAL_COLOR;
    }

    private void OnScoreChanged(int score)
    {
        this.score.text = $"Score: {score}";
    }

    private void OnTimeLeftChanged(int timeLeft)
    {
        this.timeLeft.text = $"Time Left: {timeLeft / 60}:{ (timeLeft % 60) / 10}{ (timeLeft % 60) % 10}";
    }
}
