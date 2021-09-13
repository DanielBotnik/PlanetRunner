using System;
using UnityEngine;

public class GeneralDataManager : MonoBehaviour
{
    public static GeneralDataManager instance;
    public static ProtocolType ActiveProtocol { get; set; }
    public static DifficultyMode DifficultyMode { get; set; }
    void Awake()
    {

        if (instance != null)
        {
            Debug.Log("Another instance of GeneralDataManager found! First instance: " + instance.name + ", second instance: " + name);
            throw new Exception();
        }
        else
            instance = this;

        switch(PlayerPrefs.GetString("protocolType", "attention"))
        {
            case "attention":
                {
                    ActiveProtocol = ProtocolType.ATTENTION;
                    break;
                }
            case "meditation":
                {
                    ActiveProtocol = ProtocolType.MEDITATION;
                    break;
                }
            case "peak":
                {
                    ActiveProtocol = ProtocolType.PEAK;
                    break;
                }
        }

        switch(PlayerPrefs.GetString("difficultyMode","dynamic"))
        {
            case "dynamic":
                {
                    DifficultyMode = DifficultyMode.Dynamic;
                    break;
                }
            case "manual":
                {
                    DifficultyMode = DifficultyMode.Manual;
                    DifficultyManager.SetDifficulty(PlayerPrefs.GetInt("requiredSignal", 30));
                    break;
                }
        }
        
    }

    public void ChangeProtocol(int protocolID)
    {
        ActiveProtocol = (ProtocolType)(protocolID);
    }

}
