using UnityEngine;
using UnityEngine.UI;

public class SliderResponsiveText : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        text.text = slider.value.ToString();
    }

    public void OnSliderChange()
    {
        text.text = slider.value.ToString();
    }
}
