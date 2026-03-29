using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public string checkpointId;
    public bool activationStatus;


    [ContextMenu("Generate Checkpoint ID")]
    private void GenerateId()
    {
        checkpointId = System.Guid.NewGuid().ToString();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActiveCheckpoint();
        }
    }

    public void ActiveCheckpoint()
    {
        activationStatus = true;
    }
}
