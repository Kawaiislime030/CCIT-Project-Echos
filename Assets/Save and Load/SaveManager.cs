using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHanler;


    [ContextMenu("Delete Save Data")]
    public void DeleteSaveData()
    {
        dataHanler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHanler.Delete();
    }



    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dataHanler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        saveManagers = FindAllSaveManagers();

        LoadGame();
    }
    public void NewGame()
    {
        gameData = new GameData();

        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
        {
            CharacterStats stats = PlayerManager.instance.player.GetComponent<CharacterStats>();
            if (stats != null)
            {
                stats.SaveData(ref gameData);
            }
        }
        else
        {
            Debug.LogWarning("PlayerManager or Player not set in scene!");
        }
    }


    public void LoadGame()
    {
        gameData = dataHanler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No game data found");
            NewGame();
        }

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        foreach (ISaveManager saveManager in saveManagers)
        {
            
            if (saveManager is MonoBehaviour mono)
            {
                
                if (mono != null && mono.CompareTag("Player"))
                {
                    saveManager.SaveData(ref gameData);
                }
            }
        }

        dataHanler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        
        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSavedData()
    {
        if (dataHanler.Load() != null)
        {
            return true;
        }

        return false;
    }
}

