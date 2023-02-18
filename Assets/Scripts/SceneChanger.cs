using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{

    static GameObject container;
    static SceneManager instance;


    // public static SceneChanger Instance
    // {
    //     get
    //     {
    //         if (!instance)
    //         {
    //             container = new GameObject();
    //             container.name = "SceneChanger";
    //             instance = container.AddComponent(typeof(SceneChanger)) as SceneChanger;
    //             DontDestroyOnLoad(container);
    //         }
    //         return instance;
    //     }
    // }
    public void LoadGame()
    {
        InGameData.isloaded = true;
        SceneManager.LoadScene("Stage1");


    }
    public void NewGame()
    {
        InGameData.isloaded = false;
        SceneManager.LoadScene("Stage1");

    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
