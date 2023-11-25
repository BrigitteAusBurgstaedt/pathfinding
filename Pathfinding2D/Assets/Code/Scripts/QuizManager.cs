using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
	public List<QuestionAndAnswer> QnA;
	public GameObject[] options;
	public int currentQuestion;

	public GameObject Quizpanel;
	public GameObject GoPanel;

	public Text QuestionTxt;
	public Text ScoreText;

	int totalQuestions = 0;
	public int score;

	private void Start()
	{
		totalQuestions = QnA.Count;
		GoPanel.SetActive(false);
		generateQuestion();
	}

	public void retry()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

	public void GameOver()
	{
		Quizpanel.SetActive(false);
		GoPanel.SetActive(true);
		ScoreText.text = score + "/" + totalQuestions;
	}

	public void Correct()
	{
		//eine Antwort ist richtig
		score += 1;
		QnA.RemoveAt(currentQuestion);
		generateQuestion();
	}

	public void wrong()
    {
		//eine Antwort ist falsch
		QnA.RemoveAt(currentQuestion);
		generateQuestion();

	}

	void SetAnswers()
	{
        for (int i = 0; i < options.Length; i++)
		{
			options[i].GetComponent<AnswerScript>().isCorrect = false;

			options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];

			if(QnA[currentQuestion].CorrectAnswer == i++)
			{
				options[i].GetComponent<AnswerScript>().isCorrect = true;
			}
		}
	}

	void generateQuestion()
	{
		if (QnA.Count > 0)
		{
			currentQuestion = Random.Range(0, QnA.Count);

			QuestionTxt.text = QnA[currentQuestion].Questions;
			SetAnswers();
		}
		else
		{
			Debug.Log("keine weiteren Fragen");
			GameOver();
		}
	}
}