using Assets.Scripts.Models.HeadsetModels;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

#region Enums
public enum UserType
{
    USER = 0x00,
    TRAINER = 0x01,
    TRAINEE = 0x02,
    CLIENT = 0x03,
    STUDENT = 0x04,
    GAMER = 0x05,
    PRACTITIONER = 0x06,
    ADMIN = 0x07,
    PARENT = 0x08,
    CHILD = 0x09,
    UNDEFINED = 0x10,
};
public enum GameMode
{
    TrainingProgram = 1,
    SingleActivity = 2,
    Video = 3,
    Sample = 4
}
public enum DifficultyMode
{
    Static = 1,
    Manual = 2,
    Dynamic = 3
}
public enum ProtocolType
{
    ATTENTION = 1,
    MEDITATION = 2,
    PEAK = 12
};
public enum ServerResponse
{
    //General Messages 0-15
    Success = 0,
    Command_Not_Recognized = 1,
    Server_Exception_Caught = 2,
    MySQLi_Error = 3,
    Could_Not_Connected = 4,
    // User Specific Messages 16-31
    User_Doesnt_Exist = 16,
    Username_Password_Mismatch = 17,
    User_Not_Confirmed = 18,
    License_Expired = 19,
    App_Not_Purchased = 20

    // Other Custom Errors 32-47
};
// Not Used
//public enum SignalType
//{
//    DELTA = 0, //0
//    THETA = 1, //1
//    LOWALPHA = 1 << 1, //2
//    HIGHALPHA = 1 << 2, //4
//    LOWBETA = 1 << 3, //8
//    HIGHBETA = 1 << 4, //16
//    LOWGAMMA = 1 << 5, //32
//    HIGHGAMMA = 1 << 6 //64
//}
#endregion

#region Delegates
public delegate void EmptyDelegate();
public delegate void IntValueDelegate(int value);
public delegate void FloatValueDelegate(float value);
public delegate void BoolValueDelegate(bool value);
public delegate void tringMessageDelegate(string message);
public delegate void ArrayListValueDelegate(ArrayList value);
public delegate void WaveValueDelegate(FullDataWave wave);
public delegate void ProtocolChanged(ProtocolType protocol);
#endregion

public static class ExtensionMethods
{
    //Makes AsyncOperations awaitable to reduce reliance on Coroutines and event callbacks
    public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
    {
        var tcs = new TaskCompletionSource<object>();
        asyncOp.completed += obj => { tcs.SetResult(null); };
        return ((Task)tcs.Task).GetAwaiter();
    }


}