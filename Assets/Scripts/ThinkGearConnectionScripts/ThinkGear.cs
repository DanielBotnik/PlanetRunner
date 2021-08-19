using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
public class ThinkGear : MonoBehaviour
{
    const string pluginName = "com.botnik.unity.MyPlugin";

    private static AndroidJavaClass pluginClass;
    private static AndroidJavaObject pluginObject;

    private static AndroidJavaClass PluginClass
    {
        get
        {
            if (pluginClass == null)
                pluginClass = new AndroidJavaClass(pluginName);
            return pluginClass;
        }
    }
    private static AndroidJavaObject PluginObject
    {
        get
        {
            if (pluginObject == null)
                pluginObject = PluginClass.CallStatic<AndroidJavaObject>("getInstance");
            return pluginObject;
        }
    }


    public delegate void UpdateValueDelegate(int value);
    public delegate void UpdateStateDelegate();
    public delegate void UpdateStateChange(State state);
    public delegate void UpdateSignalChange(Signal signal);
    public delegate void UpdateBrainWavesChange(List<Brainwave> brainwaves);

    // Three Main Events, State Change, Signal Change and BrainWavesChange

    public event UpdateStateChange UpdateStateChangeEvent;
    public event UpdateSignalChange UpdateSignalChangeEvent;
    public event UpdateBrainWavesChange UpdateBrainWavesChangeEvent;
    
    //State Events, one for each State

    public event UpdateStateDelegate UpdateUnknownStateEvent;
    public event UpdateStateDelegate UpdateIdleStateEvent;
    public event UpdateStateDelegate UpdateConnectingStateEvent;
    public event UpdateStateDelegate UpdateConnectedStateEvent;
    public event UpdateStateDelegate UpdateNotFoundStateEvent;
    public event UpdateStateDelegate UpdateNotPairedStateEvent;
    public event UpdateStateDelegate UpdateDisconnectedStateEvent;
    
    // Signal Events, one for each Signal 

    public event UpdateValueDelegate UpdateUnknownSignalEvent;
    public event UpdateValueDelegate UpdateStateChangeSignalEvent;
    public event UpdateValueDelegate UpdatePoorSignalEvent;
    public event UpdateValueDelegate UpdateAttentionEvent;
    public event UpdateValueDelegate UpdateMeditationEvent;
    public event UpdateValueDelegate UpdateBlinkEvent;
    public event UpdateValueDelegate UpdateSleepStageEvent;
    public event UpdateValueDelegate UpdateLowBatteryEvent;
    public event UpdateValueDelegate UpdateRawCountEvent;
    public event UpdateValueDelegate UpdateRawDataEvent;
    public event UpdateValueDelegate UpdateHeartRateEvent;
    public event UpdateValueDelegate UpdateRawMultiEventl;
    public event UpdateValueDelegate UpdateEEGPowerEvent;

    //BrainWaves Events, one for each Brainwave

    public event UpdateValueDelegate UpdateDeltaEvent;
    public event UpdateValueDelegate UpdateThetaEvent;
    public event UpdateValueDelegate UpdateLowAlphaEvent;
    public event UpdateValueDelegate UpdateHighAlphaEvent;
    public event UpdateValueDelegate UpdateLowBetaEvent;
    public event UpdateValueDelegate UpdateHighBetaEvent;
    public event UpdateValueDelegate UpdateLowGammaEvent;
    public event UpdateValueDelegate UpdateMidGammaEvent;

    private void Start()
    {
        UpdateStateChangeEvent += MainStateChange;
        UpdateSignalChangeEvent += MainSignalChange;
        UpdateBrainWavesChangeEvent += MainBrainWavesChange;
        StartMonitoring();
    }

    private void OnStateChangeCall(string message)
    {
        State state = JsonConvert.DeserializeObject<State>(message);
        UpdateStateChangeEvent?.Invoke(state);
    }

    private void OnSignalChangeCall(string message)
    {
        Signal signal = JsonConvert.DeserializeObject<Signal>(message);
        UpdateSignalChangeEvent?.Invoke(signal);
    }

    private void OnBrainWavesChangeCall(string message)
    {
        List<Brainwave> brainwaves = JsonConvert.DeserializeObject<List<Brainwave>>(message);
        UpdateBrainWavesChangeEvent?.Invoke(brainwaves);
    }

    private void MainStateChange(State state)
    {
        switch ((StateType)state.type)
        {
            case StateType.UNKNOWN:
                UpdateUnknownStateEvent?.Invoke();
                break;
            case StateType.IDLE:
                UpdateIdleStateEvent?.Invoke();
                break;
            case StateType.CONNECTING:
                UpdateConnectingStateEvent?.Invoke();
                break;
            case StateType.CONNECTED:
                UpdateConnectedStateEvent?.Invoke();
                break;
            case StateType.NOT_FOUND:
                UpdateNotFoundStateEvent?.Invoke();
                break;
            case StateType.NOT_PAIRED:
                UpdateNotPairedStateEvent?.Invoke();
                break;
            case StateType.DISCONNECTED:
                UpdateDisconnectedStateEvent?.Invoke();
                break;
        }
    }

    private void MainSignalChange(Signal signal)
    {
        switch ((SignalType)signal.type)
        {
            case SignalType.UNKNOWN:
                UpdateUnknownSignalEvent?.Invoke(signal.value);
                break;
            case SignalType.STATE_CHANGE:
                UpdateStateChangeSignalEvent?.Invoke(signal.value);
                break;
            case SignalType.POOR_SIGNAL:
                UpdatePoorSignalEvent?.Invoke(signal.value);
                break;
            case SignalType.ATTENTION:
                UpdateAttentionEvent?.Invoke(signal.value);
                break;
            case SignalType.MEDITATION:
                UpdateMeditationEvent?.Invoke(signal.value);
                break;
            case SignalType.BLINK:
                UpdateBlinkEvent?.Invoke(signal.value);
                break;
            case SignalType.SLEEP_STAGE:
                UpdateSleepStageEvent?.Invoke(signal.value);
                break;
            case SignalType.LOW_BATTERY:
                UpdateLowBatteryEvent?.Invoke(signal.value);
                break;
            case SignalType.RAW_COUNT:
                UpdateRawCountEvent?.Invoke(signal.value);
                break;
            case SignalType.RAW_DATA:
                UpdateRawDataEvent?.Invoke(signal.value);
                break;
            case SignalType.HEART_RATE:
                UpdateHeartRateEvent?.Invoke(signal.value);
                break;
            case SignalType.RAW_MULTI:
                UpdateRawMultiEventl?.Invoke(signal.value);
                break;
            case SignalType.EEG_POWER:
                UpdateEEGPowerEvent?.Invoke(signal.value);
                break;
        }
    }

    public void MainBrainWavesChange(List<Brainwave> brainwaves)
    {
        foreach (Brainwave brainwave in brainwaves)
        {
            switch ((BrainWaveType)brainwave.type)
            {
                case BrainWaveType.DELTA:
                    UpdateDeltaEvent?.Invoke(brainwave.value);
                    break;
                case BrainWaveType.THETA:
                    UpdateThetaEvent?.Invoke(brainwave.value);
                    break;
                case BrainWaveType.LOW_ALPHA:
                    UpdateLowAlphaEvent?.Invoke(brainwave.value);
                    break;
                case BrainWaveType.HIGH_ALPHA:
                    UpdateHighAlphaEvent?.Invoke(brainwave.value);
                    break;
                case BrainWaveType.LOW_BETA:
                    UpdateLowBetaEvent?.Invoke(brainwave.value);
                    break;
                case BrainWaveType.HIGH_BETA:
                    UpdateHighBetaEvent?.Invoke(brainwave.value);
                    break;
                case BrainWaveType.LOW_GAMMA:
                    UpdateLowGammaEvent?.Invoke(brainwave.value);
                    break;
                case BrainWaveType.MID_GAMMA:
                    UpdateMidGammaEvent?.Invoke(brainwave.value);
                    break;
            }
        }
    }

    public void StartMonitoring()
    {
        PluginObject.Call("startMonitoring");
    }

    public void StopMonitoring()
    {
        PluginObject.Call("stopMonitoring");
    }

    public void EnableRawSignal()
    {
        PluginObject.Call("enableRawSignal");
    }

    public void DisableRawSignal()
    {
        PluginObject.Call("disableRawSignal");
    }

    public bool IsRawSignalEnabled()
    {
        return PluginObject.CallStatic<bool>("isRawSignalEnabled");
    }


}
