using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagers : MonoBehaviour, ISaveManager
{
    public static GameManagers instance;
    [SerializeField] private Checkpoint[] checkpoints;


    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    private void Start()
    {

    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.checkpointId == pair.Key && pair.Value == true)
                {
                    checkpoint.ActiveCheckpoint();

                }
            }
        }

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (_data.closestCheckpointId == checkpoint.checkpointId)
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
        }
    }
    public void SaveData(ref GameData _data)
    {
        Checkpoint cp = FindClosestCheckpoint();
        if (cp != null)
            _data.closestCheckpointId = cp.checkpointId;
        else
            _data.closestCheckpointId = "";

        _data.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.checkpointId, checkpoint.activationStatus);
        }
    }


    public Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.instance.player.transform.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }
        return closestCheckpoint;
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
