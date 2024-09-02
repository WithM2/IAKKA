using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    // UI 요소들
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;
    public Button[] optionButtons; // 5개의 버튼을 위한 배열 (오지선다)
    public TextMeshProUGUI[] optionTexts; // 오지선다 버튼 텍스트를 위한 배열
    public GameObject multipleChoicePanel; // 오지선다 패널
    public GameObject oxPanel; // OX 패널
    public GameObject shortAnswerPanel; // 단답 패널
    public Button trueButton, falseButton; // OX 버튼들
    public TMP_InputField shortAnswerInput; // 단답 입력 필드
    public Button submitButton; // 단답 제출 버튼

    private int currentQuestionIndex = 0;
    private int score = 0;

    // 문제와 유형들
    private string[] questions = {
        "What is the capital of France?",
        "What is 2 + 2?",
        "Is the Earth flat?",
        "What is the largest ocean on Earth?",
        "Who wrote 'To Kill a Mockingbird'?"
    };

    private string[][] options = {
        new string[] { "Berlin", "Madrid", "Paris", "Rome", "London" },
        new string[] { "3", "4", "5", "6", "7" },
        null, // OX 문제는 선택지가 없음
        new string[] { "Atlantic", "Indian", "Arctic", "Pacific", "Southern" },
        null // 단답형 문제는 선택지가 없음
    };

    private int[] correctAnswers = { 2, 1, 1, 3, 0 };
    private string[] questionTypes = { "MultipleChoice", "MultipleChoice", "OX", "MultipleChoice", "ShortAnswer" };
    private string[] shortAnswers = { null, null, null, null, "Harper Lee" }; // 단답형 답안

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait; // 세로방향을 고정

        UpdateScoreText();

        // 오지선다 버튼 클릭 이벤트 연결
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnOptionButtonClick(index));
        }

        // OX 버튼 클릭 이벤트 연결
        trueButton.onClick.AddListener(() => OnOXButtonClick(true));
        falseButton.onClick.AddListener(() => OnOXButtonClick(false));

        // 단답 제출 버튼 클릭 이벤트 연결
        submitButton.onClick.AddListener(OnSubmitButtonClick);

        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            Debug.Log("Quiz Finished!");
            return;
        }

        // 패널 초기화 (모두 비활성화)
        multipleChoicePanel.SetActive(false);
        oxPanel.SetActive(false);
        shortAnswerPanel.SetActive(false);

        questionText.text = questions[currentQuestionIndex];

        // 질문 유형에 따른 패널 활성화
        string questionType = questionTypes[currentQuestionIndex];
        if (questionType == "MultipleChoice")
        {
            multipleChoicePanel.SetActive(true);
            for (int i = 0; i < optionButtons.Length; i++)
            {
                optionTexts[i].text = options[currentQuestionIndex][i];
                optionButtons[i].interactable = true;
            }
        }
        else if (questionType == "OX")
        {
            oxPanel.SetActive(true);
        }
        else if (questionType == "ShortAnswer")
        {
            shortAnswerPanel.SetActive(true);
            shortAnswerInput.text = ""; // 입력 필드 초기화
        }
    }

    void OnOptionButtonClick(int index)
    {
        if (index == correctAnswers[currentQuestionIndex])
        {
            Debug.Log("Correct!");
            score++;
            UpdateScoreText();
        }
        else
        {
            Debug.Log("Wrong!");
        }

        foreach (Button button in optionButtons)
        {
            button.interactable = false;
        }

        currentQuestionIndex++;
        Invoke("DisplayQuestion", 1f);
    }

    void OnOXButtonClick(bool isTrue)
    {
        bool correct = (isTrue && correctAnswers[currentQuestionIndex] == 1) || (!isTrue && correctAnswers[currentQuestionIndex] == 0);
        if (correct)
        {
            Debug.Log("Correct!");
            score++;
            UpdateScoreText();
        }
        else
        {
            Debug.Log("Wrong!");
        }

        currentQuestionIndex++;
        Invoke("DisplayQuestion", 1f);
    }

    void OnSubmitButtonClick()
    {
        string userAnswer = shortAnswerInput.text;
        if (userAnswer.Equals(shortAnswers[currentQuestionIndex], System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Correct!");
            score++;
            UpdateScoreText();
        }
        else
        {
            Debug.Log("Wrong!");
        }

        currentQuestionIndex++;
        Invoke("DisplayQuestion", 1f);
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}