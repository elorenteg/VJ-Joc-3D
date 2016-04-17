using UnityEngine;
using System.Collections;

public class MenuKeyboard : MonoBehaviour {

    private string[] mainMenuLabels = {"Jugar", "Instruccions", "Opcions", "Crèdits", "Sortir"};
    private bool[] mainMenuButtons;
    private int mainMenuSelected;

    private GUIStyle normalFont;
    private GUIStyle selectFont;

    public Texture2D separatorTexture;
    public Texture2D selectTexture;
    public Texture2D backgroundTexture;
    public Texture2D logoTexture;

    public AudioClip moveSound;

    void Start() {
        mainMenuSelected = 0;
        mainMenuButtons = new bool[mainMenuLabels.Length];
        mainMenuButtons[mainMenuSelected] = true;

        normalFont = new GUIStyle(); normalFont.fontSize = 28;
        selectFont = new GUIStyle(); selectFont.fontSize = 32;

        normalFont.alignment = TextAnchor.UpperCenter;
        selectFont.alignment = TextAnchor.UpperCenter;

        normalFont.normal.textColor = Color.yellow;
        selectFont.normal.textColor = new Color32(153,209,26,255);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.DownArrow) == true) {
            AudioSource.PlayClipAtPoint(moveSound, transform.position);
            mainMenuButtons[mainMenuSelected] = false;

            mainMenuSelected++;
            mainMenuSelected = Mathf.Min(mainMenuSelected, mainMenuLabels.Length-1);
            
            mainMenuButtons[mainMenuSelected] = true;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) == true) {
            AudioSource.PlayClipAtPoint(moveSound, transform.position);
            mainMenuButtons[mainMenuSelected] = false;

            mainMenuSelected--;
            mainMenuSelected = Mathf.Max(mainMenuSelected, 0);
            
            mainMenuButtons[mainMenuSelected] = true;
        }
    }

    void OnGUI()
    {
        float w_backg = 1920;
        float h_backg = 1200;
        Graphics.DrawTexture(new Rect(0,0,w_backg,h_backg), backgroundTexture);

        float w_logo = 500;
        float h_logo = 150;
        Graphics.DrawTexture(new Rect(Screen.width / 2 - w_logo / 2, 10, w_logo, h_logo), logoTexture);

        float w_text = 200;
        float h_text = 40;
        float xo_text = Screen.width / 2 - w_text / 2;
        float yo_text = 200;

        float w_sep = 500;
        float h_sep = 40;

        float w_sel = 250;
        float h_sel = 60;

        Graphics.DrawTexture(new Rect(Screen.width / 2 - w_sep / 2, yo_text-h_sep, w_sep, h_sep), separatorTexture);
        Graphics.DrawTexture(new Rect(Screen.width / 2 - w_sep / 2, yo_text+mainMenuLabels.Length*50 -20, w_sep, h_sep), separatorTexture);
        for (int i = 0; i < mainMenuLabels.Length; i++) {
            if (mainMenuButtons[i]) {
                GUI.Button(new Rect(xo_text, yo_text + i * 50, w_text, h_text), mainMenuLabels[i], selectFont);
                Graphics.DrawTexture(new Rect(Screen.width / 2 - w_sel / 2, yo_text - 10 + i * 50, w_sel, h_sel), selectTexture);
            }
            else GUI.Button(new Rect(xo_text, yo_text + i * 50, w_text, h_text), mainMenuLabels[i], normalFont);
        }

        if (mainMenuButtons[0]) { }

        if (mainMenuButtons[1]) { }

        if (mainMenuButtons[2]) { }

        if (mainMenuButtons[3]) { }

        if (mainMenuButtons[4]) {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.Return)) {
            mainMenuButtons[mainMenuSelected] = true;
        }
    }
}