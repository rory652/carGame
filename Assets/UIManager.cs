using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private TextMeshProUGUI _speedText;
    private TextMeshProUGUI _revsText;
    
    // Start is called before the first frame update
    void Start()
    {
        _speedText = gameObject.transform.Find("Speed").gameObject.GetComponent<TextMeshProUGUI>();
        _revsText = gameObject.transform.Find("Revs").gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void writeSpeed(float speed)
    {
        _speedText.SetText("Speed: {0:00.000} MPH", speed);
    }

    public void writeRevs(float revs)
    {
        _revsText.SetText("{0:0000} RPM", revs);
    }
}
