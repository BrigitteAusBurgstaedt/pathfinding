using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    public Text textComponent;
    public string[] texts;      // Texte für aktuelle Szene
    public Text textComponent2;
    public string[] titles;      // Titel für aktuelle Szene
    public string targetScene;  // Ziel-Szene
    private int currentIndex = 0;

    void Start()
    {
        if (texts.Length > 0){
            textComponent.text = texts[currentIndex];
            textComponent2.text = titles[currentIndex];
        }else
            Debug.LogError("TextManager: Keine Texte definiert!");
    }

    void Update()
    {

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
            if (!string.IsNullOrEmpty(targetScene))
                SceneManager.LoadScene(targetScene);
            else
                Debug.LogError("TextManager: Keine Ziel-Szene definiert!");
        }
    }

}

