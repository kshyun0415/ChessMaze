using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
public class SceneManager : MonoBehaviour
{

    static GameObject container;
    static SceneManager instance;


    public static SceneManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "SceneManager";
                instance = container.AddComponent(typeof(SceneManager)) as SceneManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }
    public void LoadGame()
    {
        InGameData.isloaded = true;
        EditorSceneManager.LoadScene("Stage1");


    }
    public void NewGame()
    {
        InGameData.isloaded = false;
        EditorSceneManager.LoadScene("Stage1");

    }
    public void MainMenu()
    {
        EditorSceneManager.LoadScene("MainMenu");
    }
}
