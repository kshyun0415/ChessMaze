using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
public class SceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadStage1()
    {
        EditorSceneManager.LoadScene("Stage1");
    }
    public void MainMenu()
    {
        EditorSceneManager.LoadScene("MainMenu");
    }
}
