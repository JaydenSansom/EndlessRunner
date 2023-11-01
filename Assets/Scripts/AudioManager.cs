using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource fxSource;

    [Header("Audio Clips")]
    [SerializeField] AudioClip musicLoop;
    [SerializeField] AudioClip buttonFx;
    [SerializeField] AudioClip objectHitFx;
    [SerializeField] AudioClip objectMissFx;
    [SerializeField] AudioClip victoryFx;
    [SerializeField] AudioClip defeatFx;

    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<AudioManager>();
                    Debug.Log("Generating new game manager");
                }
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = musicLoop;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void MusicPause()
    {
        musicSource.Pause();
    }

    public void MusicUnpause()
    {
        musicSource.UnPause();
    }

    public void FXButtonClick()
    {
        fxSource.PlayOneShot(buttonFx);
    }

    public void FXObjectHit()
    {
        fxSource.PlayOneShot(objectHitFx);
    }

    public void FXObjectMiss()
    {
        fxSource.PlayOneShot(objectMissFx);
    }

    public void FXVictory()
    {
        fxSource.PlayOneShot(victoryFx);
    }

    public void FXDefeat()
    {
        fxSource.PlayOneShot(defeatFx);
        musicSource.Stop();
    }
}
