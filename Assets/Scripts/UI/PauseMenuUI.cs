using System.Collections;
using TMPro;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{

    #region Tooltip
    [Tooltip("Populate with the music volume level")]
    #endregion Tooltip 
    [SerializeField] private TextMeshProUGUI musicLevelText;

    #region Tooltip 
    [Tooltip("Populate with the sounds volume level")]
    #endregion Tooltip 
    [SerializeField] private TextMeshProUGUI soundsLevelText;

  
    private void Start() 
    {

        //initially hide the pause menu
        gameObject.SetActive(false);

    }


    //intitialize the ui text
    private IEnumerator InitializeUI() 
    {

        //wait a frame to ensure the previous music and sound levels have been set
        yield return null; 

        //initialise the ui text
        soundsLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());

    }


    private void OnEnable() 
    {

        Time.timeScale = 0f;

        //initialise the ui text
        StartCoroutine(InitializeUI());

    }

    private void OnDisable() 
    {

        Time.timeScale = 1f;
        

    }


    //increase music volume - linked to from the music volume increase button in UI 
    public void IncreaseMusicVolume() 
    {

        MusicManager.Instance.IncreaseMusicVolume();
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());

    }


    //decrease music volume - linked to from music volume decrease button in UI
    public void DecreaseMusicVolume() 
    {

        MusicManager.Instance.DecreaseMusicVolume();
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());

    }


    //increase the sounds volume - linked from sounds volume to increase button in UI
    public void IncreaseSoundsVolume() 
    {

        SoundEffectManager.Instance.IncreaseSoundsVolume();
        soundsLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());

    }


    //decrease sounds volume - linked to from sounds volume decrease button in the ui
    public void DecreaseSoundsVolume() 
    {

        SoundEffectManager.Instance.DecreaseSoundsVolume();
        soundsLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());

    }


    #region Validation
#if UNITY_EDITOR 

    private void OnValidate() 
    {

        HelperUtilities.ValidateCheckNullValue(this, nameof(musicLevelText), musicLevelText);
        HelperUtilities.ValidateCheckNullValue(this, nameof(soundsLevelText), soundsLevelText);

    }

#endif 
    #endregion Validation

}
