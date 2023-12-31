//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class PlayGame : MonoBehaviour
{
    public string filePath = "Assets/script.txt";
    public int lineCounter = 0;

    public string[] scriptArray;

    public int randomIndexNumber;

    public int roundNumber = 0;
    //public bool canContinue = false;

    public TMP_Text QuoteBox;
    public TMP_InputField Answer;
    public TMP_Text resultText;

    public GameObject submitButton;
    public GameObject nextQuestionButton;

    public GameObject yesButton;
    public GameObject noButton;

    public GameObject endGameButton;

    public float score = 0;
    public TMP_Text scoreText;
    public TMP_Text questionNumber;

    public bool isBonusQuestion = false;

    public string helpText;

    public GameObject inputField;

    // Start is called before the first frame update
    void Start()
    {

        filePath = Application.streamingAssetsPath + "/script.txt";

        inputField.SetActive(false);

        if (!File.Exists(filePath))
        {
            QuoteBox.text = "couldn't find file";
            Debug.Log("File doesn't exist");
            return;
        }

		try
		{
			using StreamReader reader = new StreamReader(filePath);

			while (!reader.EndOfStream)
			{
                lineCounter++;
				string line = reader.ReadLine();
			}


            using StreamReader readerAgain = new StreamReader(filePath);

            scriptArray = new string[lineCounter];

            lineCounter = 0;

            while (!readerAgain.EndOfStream)
            {
                string lineAgain = readerAgain.ReadLine();
                scriptArray[lineCounter] = lineAgain;
                lineCounter++;
            }

        }
		catch
		{
            Debug.Log("Error while reading from file");
		}

        //randomIndexNumber = generateRandomOddNumber(lineCounter, scriptArray);
        //QuoteBox.text = "\"" + scriptArray[randomIndexNumber] + "\"";

    }

    public void playerSubmitted()
    {
	if(Answer.text == null || Answer.text == ""){
        	resultText.text = "Please enter a valid answer";
            return;
        }
        else if(Answer.text.ToLower() == "help")
        {
            resultText.text = helpText;
        }
        else if (!isBonusQuestion)
        {
            if (Answer.text.ToLower() == scriptArray[randomIndexNumber - 1].ToLower())
            {
                resultText.text = "Good Job! Would you like you try a bonus question for more points?";

                score += 5;

                yesButton.SetActive(true);
                noButton.SetActive(true);
                submitButton.SetActive(false);
            }
            else
            {
                resultText.text = "Got it wrong. Right answer was: " + scriptArray[randomIndexNumber - 1];

                nextQuestionButton.SetActive(true);
                submitButton.SetActive(false);
            }
        }
        else if (isBonusQuestion)
        {
            if (Answer.text.ToLower() == scriptArray[randomIndexNumber + 1].ToLower())
            {
                resultText.text = "Terrific Job!";

                score += 10;

                yesButton.SetActive(false);
                noButton.SetActive(false);
                submitButton.SetActive(false);
                nextQuestionButton.SetActive(true);
            }
            else
            {
                resultText.text = "I'm sorry, the right answer was: " + scriptArray[randomIndexNumber + 1];

                score -= 2;

                nextQuestionButton.SetActive(true);
                submitButton.SetActive(false);
            }
        }
    }

    public void bonusQuestion()
    {
        yesButton.SetActive(false);
        noButton.SetActive(false);
        submitButton.SetActive(true);
        nextQuestionButton.SetActive(false);

        isBonusQuestion = true;

        Answer.text = "";

        resultText.text = "Who says the next spoken line after the one above?";
    }

    public void nextQuestion()
    {
        inputField.SetActive(true);

        isBonusQuestion = false;
        roundNumber++;

        randomIndexNumber = generateRandomOddNumber(lineCounter, scriptArray);
        QuoteBox.text = "\"" + scriptArray[randomIndexNumber] + "\"";

        resultText.text = "";
        Answer.text = "";

        yesButton.SetActive(false);
        noButton.SetActive(false);
        submitButton.SetActive(true);
        nextQuestionButton.SetActive(false);
    }

    public static int generateRandomOddNumber(int lengthOfArray, string[] scriptArray)
    {
        //Random random = new Random();
        int randomInt = UnityEngine.Random.Range(0, lengthOfArray);
        while (randomInt > lengthOfArray || randomInt % 2 != 1)
        {
            randomInt = UnityEngine.Random.Range(0, lengthOfArray);
        }
        //Debug.Log(scriptArray[randomInt]);
        return randomInt;
    }

    // Update is called once per frame
    void Update()
    {
        //if (roundNumber <= 5 && Input.GetKeyDown(KeyCode.Space) && canContinue)
        //{
        //    roundNumber += 1;
        //}

        

        if (roundNumber == 6)
        {
            QuoteBox.text = "Game Over. Final score was " + score;
            submitButton.SetActive(false);
            nextQuestionButton.SetActive(false);
            questionNumber.text = "";
            resultText.text = "";
            endGameButton.SetActive(true);
            inputField.SetActive(false);
            scoreText.text = "";
        }
        else
        {
            questionNumber.text = "Question #" + roundNumber;
            scoreText.text = "Score: " + score;
        }
    }
}
