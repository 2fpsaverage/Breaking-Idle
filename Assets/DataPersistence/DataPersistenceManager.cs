using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private SaveData saveData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance {get; private set;}

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    public void NewGame()
    {
        this.saveData = new SaveData();
        SaveGame();
        Debug.Log("Data Reset");
    }

    public void LoadGame()
    {
        //load data from file using data handler
        this.saveData = dataHandler.Load();

        //if no data, new game
        if(this.saveData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        //push loaded data to other scripts
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(saveData);
        }

        Debug.Log("Data Loaded");
    }

    public void SaveGame()
    {
        //pass data to other scripts so they can update it
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(saveData);
        }

        //save that data to a file using data handler
        dataHandler.Save(saveData);
        Debug.Log("Data Saved");
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
