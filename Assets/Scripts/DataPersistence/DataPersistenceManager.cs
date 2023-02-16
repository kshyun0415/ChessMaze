// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;
// public class DataPersistenceManager : MonoBehaviour
// {
//     [Header("File Storage Config")]
//     [SerializeField] private string fileName;
//     private GameData gameData;
//     private List<IDataPersistence> dataPersistenceObjects;

//     private FileDataHandler dataHandler;
//     public static DataPersistenceManager instance { get; private set; }
//     private void Awake()
//     {
//         if (instance != null)
//         {
//             Debug.LogError("Found more than one Data Persistence Manager in the Scene.");
//         }
//         instance = this;
//     }
//     void Start()
//     {
//         this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
//         this.dataPersistenceObjects = FindAllDataPersistenceObjects();
//         LoadGame();
//     }
//     public void NewGame()
//     {
//         this.gameData = new GameData();
//         Debug.Log("NewGame Stage1");
//     }

//     public void LoadGame()
//     {
//         this.gameData = dataHandler.Load();
//         if (this.gameData == null)
//         {
//             Debug.Log("No data Found");
//             NewGame();
//         }
//         Debug.Log("Scene Loaded");
//         foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
//         {
//             dataPersistenceObj.LoadData(gameData);
//         }
//         //
//     }
//     public void SaveGame()
//     {
//         //
//         foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
//         {
//             dataPersistenceObj.SaveData(ref gameData);
//         }
//         Debug.Log("Saved Stage1");

//         dataHandler.Save(gameData);
//     }
//     private List<IDataPersistence> FindAllDataPersistenceObjects()
//     {
//         IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
//         return new List<IDataPersistence>(dataPersistenceObjects);
//     }
//     private void OnApplicationQuit()
//     {
//         SaveGame();
//     }
// }
