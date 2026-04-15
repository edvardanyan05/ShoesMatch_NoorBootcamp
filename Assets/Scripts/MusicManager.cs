using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogError("AudioSource not found!");
                return;
            }

            audioSource.loop = true;

            bool isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

            if (isOn)
                audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void musicOnOff()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            PlayerPrefs.SetInt("MusicOn", 0);
        }
        else
        {
            audioSource.Play();
            PlayerPrefs.SetInt("MusicOn", 1);
        }
    }
}