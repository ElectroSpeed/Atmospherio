using UnityEngine;
using UnityEngine.UI;

public class SetSlider : MonoBehaviour
{
    [SerializeField] private Slider _sliderSFX;
    [SerializeField] private Slider _sliderMusic;
    [SerializeField] private Slider _sliderMain;
    private void Start()
    {
        if (PlayerPrefs.HasKey("SFX"))
            _sliderSFX.value = PlayerPrefs.GetFloat("SFX");

        if (PlayerPrefs.HasKey("Music"))
            _sliderMusic.value = PlayerPrefs.GetFloat("Music");

        if (PlayerPrefs.HasKey("Main"))
            _sliderMain.value = PlayerPrefs.GetFloat("Main");
    }
    public void SetVolumeSFX(float volume)
    {
        PlayerPrefs.SetFloat("SFX", volume);
    }
    public void SetVolumeMusic(float volume)
    {
        PlayerPrefs.SetFloat("Music", volume);
    }
    public void SetVolumeMain(float volume)
    {
        PlayerPrefs.SetFloat("Main", volume);
    }
}
