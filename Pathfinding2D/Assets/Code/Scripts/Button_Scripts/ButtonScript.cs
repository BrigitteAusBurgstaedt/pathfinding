using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{

	[SerializeField]
	private string targetSceneName;

	public void ExitButton()
	{
		// Diese Methode wird aufgerufen, wenn der Button geklickt wird.
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false; // Beendet die Play-Modus in der Unity-Editor-Umgebung.
		#else
			Application.Quit(); // Beendet die Anwendung im Build-Modus (zum Beispiel als eigenständige Anwendung).
		#endif
	}

	public void StartGame()
	{
		SceneManager.LoadScene(targetSceneName);
	}

	public void LoadGame()
	{
		SceneManager.LoadScene(targetSceneName);
	}

	public void LoadOptions()
	{
		SceneManager.LoadScene(targetSceneName);
	}
}
