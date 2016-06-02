using UnityEngine;
using System.Collections;

public class HowTo : MonoBehaviour
{
    private const int RETURN_MENU = 0;
    private int action;

    private GUIStyle normalFont;
    
    void Start()
    {
        action = -1;

        normalFont = new GUIStyle();
        normalFont.fontSize = 24;
        normalFont.alignment = TextAnchor.UpperCenter;
        normalFont.normal.textColor = new Color32(119, 136, 153, 255);
        normalFont.font = (Font)Resources.Load("Fonts/namco", typeof(Font));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            action = RETURN_MENU;
        }
    }

    void OnGUI()
    {
        switch (action)
        {
            case RETURN_MENU:

                break;
        }

        GUI.Label(new Rect(0, 0, 100, 100), "HOLA", normalFont);
    }
}
