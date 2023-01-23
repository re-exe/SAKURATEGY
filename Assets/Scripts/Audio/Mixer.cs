using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// AudioMixer
/// </summary>
public class Mixer : MonoBehaviour{
    
    [SerializeField]
    [Tooltip("AudioMixer")]
    private AudioMixer audioMixer = null;

    [SerializeField]
    [Tooltip("VolumeSliderClass")]
    private List<VolumeSlider> volumeSlider = new List<VolumeSlider>((int)AUDIO_ID.MAX);

    [System.Serializable]
    public class VolumeSlider{
        [Tooltip("name")]
        public string name = "";

        [Tooltip("Slider")]
        public Slider slider = null;
    }

    public enum AUDIO_ID{
        MASTER,
        BGM,
        SE,
        MAX
    }

    private void Start() {
        float m_nowVolume = 0f;

        audioMixer.GetFloat("SetMaster", out m_nowVolume);
        volumeSlider[(int)AUDIO_ID.MASTER].slider.value = Db2Pa(m_nowVolume);

        audioMixer.GetFloat("SetBGM", out m_nowVolume);
        volumeSlider[(int)AUDIO_ID.BGM].slider.value = Db2Pa(m_nowVolume);

        audioMixer.GetFloat("SetSE", out m_nowVolume);
        volumeSlider[(int)AUDIO_ID.SE].slider.value = Db2Pa(m_nowVolume);
    }

    /// <summary>
    /// SetMasterVolume
    /// </summary>
    /// <param name="volume">AudioVolume</param>
    public void SetMaster(float volume){
        float v = Pa2Db(volume);
        audioMixer.SetFloat("SetMaster", v);
    }

    /// <summary>
    /// SetBGMVolume
    /// </summary>
    /// <param name="volume">AudioVolume</param>
    public void SetBGM(float volume){
        float v = Pa2Db(volume);
        audioMixer.SetFloat("SetBGM", v);
    }

    /// <summary>
    /// SetSEVolume
    /// </summary>
    /// <param name="volume">AudioVolume</param>
    public void SetSE(float volume){
        float v = Pa2Db(volume);
        audioMixer.SetFloat("SetSE", v);
    }

    /// <summary>
    /// Decibel Conversion
    /// </summary>
    /// <param name="pa"></param>
    /// <returns></returns>
    private float Pa2Db(float pa){
        pa = Mathf.Clamp(pa, 0.0001f, 10f);
        return 20f * Mathf.Log10(pa);
    }

    /// <summary>
    /// Sound Pressure Conversion
    /// </summary>
    /// <param name="db"></param>
    /// <returns></returns>
    private float Db2Pa(float db){
        db = Mathf.Clamp(db, -80f, 20f);
        return Mathf.Pow(10f, db / 20f);
    }
}
