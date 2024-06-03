using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Canvassc : MonoBehaviour
{
    public Scene Startscene;
    InputSkript input;
    [HideInInspector]
    public GameObject tsks;
    [HideInInspector]
    public Text t1, t2;
    public string s1 = "Задача1: ", s2 = "Задача2: ", s3 = "выполнено", s4 = "не выполнено";
    void Start()
    {
        t1 = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
        t2 = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>();
        t1.text = s1 + s4;
        t2.text = s2 + s4;
        input = GameObject.FindGameObjectWithTag("Input").GetComponent<InputSkript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(input.r){
            SceneManager.LoadScene("DemoDemo");
        }
        if(input.j){
            tsks.SetActive(true);
        }
        else{
            tsks.SetActive(false);
        }
    }
}
