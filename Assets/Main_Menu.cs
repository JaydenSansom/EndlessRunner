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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene("NyelaScene");
    }

    public void Highscore()
    {
        panel.SetActive(true);
        backgroundCanvas.SetActive(false);
    }

    public void Back()
    {
        panel.SetActive(false);
        backgroundCanvas.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
