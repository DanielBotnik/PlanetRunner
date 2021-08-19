using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public int type { get; set; }
}

public class Signal
{
    public int type { get; set; }
    public int value { get; set; }
}

public class Brainwave
{
    public int type { get; set; }
    public int value { get; set; }
}

public enum StateType
{
    UNKNOWN = -1,
    IDLE = 0,
    CONNECTING = 1,
    CONNECTED = 2,
    NOT_FOUND = 4,
    NOT_PAIRED = 5,
    DISCONNECTED = 3
}

public enum SignalType
{
    UNKNOWN = -1,
    STATE_CHANGE = 1,
    POOR_SIGNAL = 2,
    ATTENTION = 4,
    MEDITATION = 5,
    BLINK = 22,
    SLEEP_STAGE = 178,
    LOW_BATTERY = 20,
    RAW_COUNT = 19,
    RAW_DATA = 128,
    HEART_RATE = 3,
    RAW_MULTI = 145,
    EEG_POWER = 131,
}

public enum BrainWaveType
{
    DELTA = 1,
    THETA = 2,
    LOW_ALPHA = 3,
    HIGH_ALPHA = 4,
    LOW_BETA = 5,
    HIGH_BETA = 6,
    LOW_GAMMA = 7,
    MID_GAMMA = 8,
}