using UnityEngine;
using UnityEngine.UI;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    public Button button1;
    public Button button2;

    private void Start()
    {
        // Zu Beginn beide Panels deaktivieren
        panel1.SetActive(false);
        panel2.SetActive(false);

        // Den Buttons die Funktionen zum Umschalten zuweisen
        button1.onClick.AddListener(TogglePanel1);
        button2.onClick.AddListener(TogglePanel2);

        // Die Farbe der Buttons initialisieren
        UpdateButtonColor();
    }

    void TogglePanel1()
    {
        // Panel 1 umschalten
        panel1.SetActive(!panel1.activeSelf);

        // Panel 2 ausschalten
        panel2.SetActive(false);

        // Die Farbe der Buttons aktualisieren
        UpdateButtonColor();
    }

    void TogglePanel2()
    {
        // Panel 2 umschalten
        panel2.SetActive(!panel2.activeSelf);

        // Panel 1 ausschalten
        panel1.SetActive(false);

        // Die Farbe der Buttons aktualisieren
        UpdateButtonColor();
    }

    void UpdateButtonColor()
    {
        // Die Farbe des ersten Buttons basierend auf der Sichtbarkeit von Panel 1 aktualisieren
        button1.image.color = panel1.activeSelf ? Color.red : Color.green;

        // Die Farbe des zweiten Buttons basierend auf der Sichtbarkeit von Panel 2 aktualisieren
        button2.image.color = panel2.activeSelf ? Color.red : Color.green;
    }
}
