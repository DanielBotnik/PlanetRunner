using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetManager : MonoBehaviour
{
    #region Events
    public static event EmptyDelegate HeadsetConnected;
    public static event EmptyDelegate HeadsetDisconnected;
    public static event IntValueDelegate UpdatePoorSignalEvent;
    public static event IntValueDelegate UpdateAttentionEvent;
    public static event IntValueDelegate UpdateMeditationEvent;
    #endregion Events

    public static int Attention
    {
        get
        {
            return attentionValue;
        }
    }

    public static int Meditation
    { 
        get
        { 
            return meditationValue; 
        } 
    }

    public static int PoorSignal
    { 
        get
        { 
            return poorSignalValue; 
        } 
    }

    public static bool IsConnected
    { 
        get 
        { 
            return isConnected && PoorSignal < 20; 
        } 
    }

    public static int LiveProtocol
    {
        get
        {
            switch (GeneralDataManager.ActiveProtocol)
            {
                case ProtocolType.ATTENTION: { return Attention; }
                case ProtocolType.MEDITATION: { return Meditation; }
                case ProtocolType.PEAK: { return Mathf.FloorToInt(((float)Attention + Meditation) / 2); }
                default: return Attention;
            }
        }
    }

    public static HeadsetManager instance;
    private static bool isConnected;
    private static int attentionValue, meditationValue, poorSignalValue;
    private ThinkGear thinkGear;

    private const bool logPoorSignal = false;

    private void Awake()
    {
        thinkGear = GetComponent<ThinkGear>();
        if(instance != null)
        {
            Debug.LogError("Multiple Headset Managers. First on " + instance.name + ", second on " + name);
            throw new Exception();
        }
        if(thinkGear is null)
        {
            Debug.LogError("ThinkGear Object is not in the scence");
            throw new Exception();
        }
        instance = this;

        thinkGear.UpdateDisconnectedStateEvent += OnDisconnectedHeadset;
        thinkGear.UpdateConnectedStateEvent += OnConnectedHeadset;
        thinkGear.UpdatePoorSignalEvent += InvokePoorSignalEvent;
        thinkGear.UpdateAttentionEvent += InvokeAttentionEvent;
        thinkGear.UpdateMeditationEvent += InvokeMeditationEvent;
    }



    private void InvokePoorSignalEvent(int value)
    {
        if (poorSignalValue == value)
            return;
        poorSignalValue = value;
        if (logPoorSignal)
            Debug.Log("Poor Signal value: " + poorSignalValue);

        if (poorSignalValue != 0)
        {
            switch (poorSignalValue)
            {
                case 200:
                    {
                        //ChangeStatus(AdapterStatus.DEVICEDISCONNECTED);
                        if (logPoorSignal)
                            Debug.Log("Device is DISCONNECTED");
                        if (attentionValue != -1)
                        {
                            attentionValue = -1;
                        }

                        if (meditationValue != -1)
                        {
                            meditationValue = -1;
                        }
                        break;
                    }
                case 25:
                    {
                        if (logPoorSignal)
                            Debug.Log("There haven't been any att/med values for at least 3 seconds, poor signal value turned to 25");

                        //ChangeStatus(AdapterStatus.WEAKSIGNAL);
                        if (attentionValue != -2)
                        {
                            attentionValue = -2;
                        }

                        if (meditationValue != -2)
                        {
                            meditationValue = -2;
                        }
                        break;
                    }
                default:
                    {
                        if (logPoorSignal)
                            Debug.Log("WEAKSIGNAL");
                        if (attentionValue != 0)
                        {
                            attentionValue = 0;
                        }

                        if (meditationValue != 0)
                        {
                            meditationValue = 0;
                        }
                        break;
                    }
            }
        }
        else
        {
            if (logPoorSignal)
                Debug.Log("DEVICE CONNECTED");
        }
        Debug.Log("Signal Value: " + poorSignalValue);
        UpdatePoorSignalEvent?.Invoke(value);
    }

    private void InvokeAttentionEvent(int value)
    {
        if (!IsConnected) { attentionValue = -1; return; }
        attentionValue = value;
        UpdateAttentionEvent?.Invoke(value);
    }

    private void InvokeMeditationEvent(int value)
    {
        if (!IsConnected) { meditationValue = -1; return; }
        meditationValue = value;
        UpdateMeditationEvent?.Invoke(value);
    }

    private void OnConnectedHeadset()
    {
        isConnected = true;
        thinkGear.StartMonitoring();
        HeadsetConnected?.Invoke();
    }
    private void OnDisconnectedHeadset()
    {
        isConnected = false;
        InvokeAttentionEvent(-1);
        InvokeMeditationEvent(-1);
        thinkGear.StopMonitoring();
        HeadsetDisconnected?.Invoke();
    }

}
