using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AnswerScript : MonoBehaviour
{

	public bool isCorrect = false;
	public QuizManager quizmanager;

	public Color startColor;

   private void Start()
    {
		startColor = GetComponent<Image>().color;
    }

    public void Answer()
    {

		if(isCorrect)
		{
			GetComponent<Image>().color = Color.green;
			Debug.Log("Korrekt");
			quizmanager.Correct();
		}
		else
		{
			GetComponent<Image>().color = Color.red;
			Debug.Log("Falsch");
			quizmanager.wrong();

		}

	}


}
