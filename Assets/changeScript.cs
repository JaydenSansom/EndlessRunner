using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class changeScript : MonoBehaviour
{

    [SerializeField]
    public TMP_Text score;
    // Start is called before the first frame update
    void Start()
    {
        score.text = "Hello";
    }

    // Update is called once per frame
    void Update()
    {
        //get game manager and update text based of current game time
    }
}
