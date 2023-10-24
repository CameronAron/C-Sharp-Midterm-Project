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

    public int roundNumber = 1;
    //public bool canContinue = false;

    public TMP_Text QuoteBox;
    public TMP_InputField Answer;
    public TMP_Text resultText;

    public GameObject submitButton;
    public GameObject nextQuestionButton;

    public GameObject yesButton;
    public GameObject noButton;

    public float score = 0;
    public TMP_Text scoreText;
    public TMP_Text questionNumber;

    public bool isBonusQuestion = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!File.Exists(filePath))
        {
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

        randomIndexNumber = generateRandomOddNumber(lineCounter, scriptArray);
        QuoteBox.text = "\"" + scriptArray[randomIndexNumber] + "\"";

    }

    public void playerSubmitted()
    {
        if (!isBonusQuestion)
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

        resultText.text = "Who says the next spoken line after the one above?";
    }

    public void nextQuestion()
    {
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

        scoreText.text = "Score: " + score;
        questionNumber.text = "Question #" + roundNumber;

        if (roundNumber == 6)
        {
            Debug.Log("Game Over. Final score was " + score);
        }
    }
}
