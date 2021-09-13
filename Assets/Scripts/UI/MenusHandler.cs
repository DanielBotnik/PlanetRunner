using UnityEngine;
using UnityEngine.UI;

public class MenusHandler : MonoBehaviour
{

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField]
    private Dropdown sensorModeDropdown;
    [SerializeField]
    private Dropdown difficultyModeDropdown;

    [SerializeField]
    private Slider requiredSignalSlider;

    [SerializeField]
    private GameObject requiredSignalSecetion;

    private void Awake()
    {
        AstronautManager.ReturnToMenu += LoadMainMenu;
        string protocolType = PlayerPrefs.GetString("protocolType", "attention");
        string difficultyMode = PlayerPrefs.GetString("difficultyMode", "manual");
        sensorModeDropdown.value = sensorModeDropdown.options.FindIndex(option => option.text.ToLower() == protocolType);
        difficultyModeDropdown.value = difficultyModeDropdown.options.FindIndex(option => option.text.ToLower() == difficultyMode);
        requiredSignalSlider.value = PlayerPrefs.GetInt("requiredSignal",30);
        requiredSignalSecetion.SetActive(difficultyMode == "manual");
    }

    private void LoadMainMenu()
    {
        mainMenu.SetActive(true);
    }

    public void OnPlayButtonClicked()
    {
        mainMenu.SetActive(false);
        DifficultyManager.instance.ActivityStart();
        if(HeadsetManager.IsConnected)
            AstronautManager.playing = true;
        else
            HeadsetManager.HeadsetConnected += StartGameOnConnection;
    }

    private void StartGameOnConnection()
    {
        AstronautManager.playing = true;
        HeadsetManager.HeadsetConnected -= StartGameOnConnection;
    }

    public void OnSettingsButtonClicked()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnMainMenuButtonClicked()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void OnSensorModeChanged(Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            // Attention
            case 0:
                {
                    GeneralDataManager.ActiveProtocol = ProtocolType.ATTENTION;
                    PlayerPrefs.SetString("protocolType", "attention");
                    break;
                }
            // Meditation
            case 1:
                {
                    GeneralDataManager.ActiveProtocol = ProtocolType.MEDITATION;
                    PlayerPrefs.SetString("protocolType", "meditation");
                    break;
                }
            // PEAK
            case 2:
                {
                    GeneralDataManager.ActiveProtocol = ProtocolType.PEAK;
                    PlayerPrefs.SetString("protocolType", "peak");
                    break;
                }
        }
    }

    public void OnDifficultyModeChanged(Dropdown dropdown)
    {
        switch(dropdown.value)
        {
            //Dynamic
            case 0:
                {
                    GeneralDataManager.DifficultyMode = DifficultyMode.Dynamic;
                    PlayerPrefs.SetString("difficultyMode", "dynamic");
                    requiredSignalSecetion.SetActive(false);
                    break;
                }
            //Manual
            case 1:
                {
                    GeneralDataManager.DifficultyMode = DifficultyMode.Manual;
                    PlayerPrefs.SetString("difficultyMode", "manual");
                    requiredSignalSecetion.SetActive(true);
                    break;
                }
        }
    }

    public void OnSliderFinalValueChange()
    {
        PlayerPrefs.SetInt("requiredSignal", (int)requiredSignalSlider.value);
    }

}
