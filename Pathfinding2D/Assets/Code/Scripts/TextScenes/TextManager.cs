using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    [SerializeField]
    private Text textComponent;

    [SerializeField]
    private string[] texts;      // Texte für aktuelle Szene

    [SerializeField]
    private Text textComponent2;

    [SerializeField]
    private string[] titles;      // Titel für aktuelle Szene

    public string targetScene;  // Ziel-Szene
    private int currentIndex = 0;

    void Start()
    {
        if (textComponent != null && textComponent2 != null && texts != null && titles != null)
        {
            if (texts.Length > 0)
            {
                textComponent.text = texts[currentIndex];
                textComponent2.text = titles[currentIndex];
            }
            else
            {
                Debug.LogError("TextManager: Keine Texte definiert!");
            }
        }
        else
        {
            Debug.LogError("TextManager: Stelle sicher, dass alle Variablen im Editor zugewiesen sind!");
        }
    }

    public void NextText()
    {
        currentIndex++;
        if (currentIndex < texts.Length)
        {
            textComponent.text = texts[currentIndex];
            textComponent2.text = titles[currentIndex];
        }
        // Wenn alle Texte durchgewechselt, Wechsel der Szene
        else
        {
            // Logik für Szenenwechsel
            if (!string.IsNullOrEmpty(targetScene) && SceneManager.GetSceneByName(targetScene) != null)
            {
                SceneManager.LoadScene(targetScene);
            }
            else
            {
                Debug.LogError("TextManager: Ungültige oder keine Ziel-Szene definiert!");
            }
        }
    }
}
