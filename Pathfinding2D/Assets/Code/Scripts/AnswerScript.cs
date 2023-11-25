using UnityEngine;
using System.Collections;

public class AnswerScript : MonoBehaviour
{

	public bool isCorrect = false;
	public QuizManager quizmanager;

	public void Answer()
    {

		if(isCorrect)
		{
			Debug.Log("Korrekt");
			quizmanager.Correct();
		}
		else
		{
			Debug.Log("Falsch");
			quizmanager.wrong();

		}

	}


}
