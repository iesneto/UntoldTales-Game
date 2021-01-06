using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class showFPS : MonoBehaviour {

    float deltaTime = 0.0f;
    float showTime = 0.0f;
    Text fpsText;

    void Start()
    {
        fpsText = GetComponent<Text>();
    }

    void Update()
    {
        
        showTime += Time.deltaTime;
        if (showTime >= 0.3f)
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            string text = string.Format(" {0:0.} fps", fps);
            fpsText.text = text;
            showTime = 0.0f;
        }
    }
   
	
	
}
