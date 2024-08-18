using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 관련 네임스페이스

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText; // 점수를 표시할 UI 요소
    public Button[] optionButtons; // 5개의 버튼을 위한 배열
    public TextMeshProUGUI[] optionTexts; // 버튼 텍스트를 위한 배열

    private int currentQuestionIndex = 0;
    private int score = 0; // 점수 변수

    // 질문과 선택지를 설정합니다.
    private string[] questions = {
        "What is the capital of France?",
        "What is 2 + 2?",
        "Which planet is known as the Red Planet?",
        "What is the largest ocean on Earth?",
        "Who wrote 'To Kill a Mockingbird'?"
    };

    private string[][] options = {
        new string[] { "Berlin", "Madrid", "Paris", "Rome", "London" },
        new string[] { "3", "4", "5", "6", "7" },
        new string[] { "Earth", "Mars", "Jupiter", "Saturn", "Venus" },
        new string[] { "Atlantic", "Indian", "Arctic", "Pacific", "Southern" },
        new string[] { "Harper Lee", "Mark Twain", "J.K. Rowling", "Ernest Hemingway", "George Orwell" }
    };

    private int[] correctAnswers = { 2, 1, 1, 3, 0 }; // 0-based index of correct answers

    void Start()
    {
        // 초기 점수 설정
        UpdateScoreText();

        // 버튼 클릭 이벤트 연결
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // 지역 변수로 복사
            optionButtons[i].onClick.AddListener(() => OnOptionButtonClick(index));
        }
        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            Debug.Log("Quiz Finished!");
            return;
        }

        // 질문과 옵션을 표시합니다.
        questionText.text = questions[currentQuestionIndex];

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionTexts[i].text = options[currentQuestionIndex][i];
            optionButtons[i].interactable = true; // 버튼을 활성화합니다.
        }
    }

    void OnOptionButtonClick(int index)
    {
        // 선택된 답안을 확인합니다.
        if (index == correctAnswers[currentQuestionIndex])
        {
            Debug.Log("Correct!");
            score++; // 정답을 맞췄을 때 점수 증가
            UpdateScoreText(); // 점수 텍스트 업데이트
        }
        else
        {
            Debug.Log("Wrong!");
        }

        // 버튼 클릭 후, 버튼을 비활성화합니다.
        foreach (Button button in optionButtons)
        {
            button.interactable = false;
        }

        // 다음 문제로 넘어갑니다.
        currentQuestionIndex++;
        Invoke("DisplayQuestion", 1f); // 1초 지연 후 다음 질문 표시
    }

    void UpdateScoreText()
    {
        // 점수를 표시하는 텍스트 업데이트
        scoreText.text = "Score: " + score.ToString();
    }
}