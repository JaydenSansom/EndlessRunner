using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    [SerializeField]
    GameObject panel;
    [SerializeField]
    GameObject backgroundCanvas;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        AudioManager.Instance.MusicUnpause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Highscore()
    {
        panel.SetActive(true);
    }

    public void Back()
    {
        panel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
