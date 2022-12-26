using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TrackManager : MonoBehaviour
{
    // temp
    public TextMeshProUGUI timer;
    public TextMeshProUGUI last;

    private Dictionary<string, bool> checkpoints;
    private float time = 0;

    public void Start()
    {
        timer.SetText("Time: 0:00.000");
        last.SetText("");

        checkpoints = new Dictionary<string, bool>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Checkpoint"))
            {
                checkpoints.Add(child.gameObject.name, false);
            }
        }

        if (!checkpoints.ContainsKey("finish"))
        {
            Debug.LogError("No finish checkpoint");
        }
    }

    public void Update()
    { 
        if (checkpoints["finish"])
        {
            time += Time.deltaTime;

            timer.SetText("Time: {00.000}", time);
        }
    }

    public void CheckpointHit(string id)
    {
        Debug.Log("Checkpoint Hit: " + id);

        if (id == "finish")
        {
            if (!checkpoints[id])
            {
                time = 0;
            }
            else
            {
                bool valid = true;
                foreach (bool value in checkpoints.Values)
                {
                    if (!value)
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                {
                    last.SetText("Last Time: Invalid");
                } 
                else
                {
                    last.SetText("Last Time: {00.000}", time);
                }
                
                time = 0;
            }
        }

        checkpoints[id] = !checkpoints[id];
    }
}
