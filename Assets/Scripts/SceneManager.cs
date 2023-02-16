using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
public class SceneManager : MonoBehaviour
{
    public bool isloaded;
    static GameObject container;
    static SceneManager instance;

    private void Start()
    {
        isloaded = false;
    }
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
        isloaded = true;
        EditorSceneManager.LoadScene("Stage1");

    }
    public void NewGame()
    {
        EditorSceneManager.LoadScene("Stage1");

    }
    public void MainMenu()
    {
        EditorSceneManager.LoadScene("MainMenu");
    }
}
