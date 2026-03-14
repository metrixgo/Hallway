using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensitivitySlider : MonoBehaviour
{
    private Slider slider;
    public PlayerController pc;
    public TextMeshProUGUI txt;

    private void OnEnable()
    {
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("Sensitivity", 10.0f);
        Display(slider.value);
    }

    public void Display(float n)
    {
        txt.text = n.ToString("0.00");
        pc.SetSensitivity(n);
        PlayerPrefs.SetFloat("Sensitivity", n);
        PlayerPrefs.Save();
    }
}
