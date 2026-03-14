using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundEffectsSlider : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI txt;

    private void OnEnable()
    {
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("SoundEffects", 80.0f);
        Display(slider.value);
    }

    public void Display(float n)
    {
        txt.text = n.ToString("0.00");
        MainManager.Instance.SetSoundEffects(n);
        PlayerPrefs.SetFloat("SoundEffects", n);
        PlayerPrefs.Save();
    }
}
