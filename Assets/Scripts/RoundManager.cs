using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    private UIManager uiManager;
    
    void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    
    void Update()
    {
        if(roundTime > 0)
        {
            roundTime -= Time.deltaTime;

            if(roundTime <= 0)
            {
                roundTime = 0;


            }
        }

        uiManager.timeText.text = roundTime.ToString("0.0") + "sec";
    }
}
