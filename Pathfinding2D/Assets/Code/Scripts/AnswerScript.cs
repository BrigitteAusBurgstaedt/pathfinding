using UnityEngine;
using System.Collections;

public class AnswerScript : MonoBehaviour
{

	public bool isCorrect = false;
	public QuizManager quizmanager;

	void Answer()
    {

		if (isCorrect)
		{
			Debug.Log("Korrekt");
			quizmanager.correct();
		}
		else
		{
			Debug.Log("Falsch");
			quizmanager.correct();

		}

	}


}
