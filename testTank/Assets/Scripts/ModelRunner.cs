using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine.UI;                 // UI요소
using TMPro;                          // TextMeshPro


public class ModelRunner : MonoBehaviour
{
    public MjpegStreamReader AiTexture;

    [SerializeField]
    private TextMeshProUGUI AiText;
    string[] aitext = {"Human" , "Not Human"};
    [SerializeField] private Image uiImage1;

    [SerializeField]
    private NNModel modelAsset; // ONNX 모델 파일
    private Model runtimeModel;
    private IWorker worker;
    bool isrunning;

    private Dictionary<int, string> labels = new Dictionary<int, string>(); // 라벨 매핑

    void OnEnable()
    {
        isrunning = true;
        // 모델 로드
        runtimeModel = ModelLoader.Load(modelAsset);

        // 워커 생성 (GPU 사용 가능 시 ComputePrecompiled 또는 Auto)
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);

        // 라벨 로드
        LoadLabels();

        StartCoroutine(MachineGo());
    }

    void OnDisable()
    {
        isrunning = false;
        // 워커 해제
        worker.Dispose();
        StopCoroutine("MachineGo");
        labels.Clear();
    }

    IEnumerator MachineGo()
    {
        while(isrunning)
        {
            yield return new WaitForSeconds(0.2f); // 4fps으로 실행

            if(AiTexture.texture != null){
                Tensor inputTensor = ConverTextureToTensor(AiTexture.texture);
                RunModel(inputTensor);
            }
        }
    }

    // 라벨 파일 로드 메서드
    void LoadLabels()
    {
        // label.txt를 Resources 폴더에 넣었다고 가정
        TextAsset labelFile = Resources.Load<TextAsset>("label");
        string[] lines = labelFile.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                labels.Add(i, lines[i]);
            }
        }
    }

    // 모델 실행 메서드
    public void RunModel(Tensor inputTensor)
    {
        // 입력 텐서를 사용하여 모델 실행
        worker.Execute(inputTensor);

        // 모델의 출력 얻기 (출력 이름은 모델에 따라 다를 수 있음)
        Tensor outputTensor = worker.PeekOutput();

        // 결과 처리
        ProcessOutput(outputTensor);

        // 메모리 해제
        inputTensor.Dispose();
        outputTensor.Dispose();
    }

    Tensor ConverTextureToTensor(Texture2D texture)
    {
        //Debug.Log($"{texture.width} x {texture.height}");
        int targetWidth = texture.width;
        int targetHeight = texture.height;


        Color32[] pixels = texture.GetPixels32();

        // 텐서에 넣을 데이터를 담을 배열 (R, G, B 채널을 위한 공간)
        float[] imageData = new float[targetHeight * targetWidth * 3];

        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                int pixelIndex = y * targetWidth + x;
                Color32 pixel = pixels[pixelIndex];

                int index = pixelIndex * 3; // 1차원 배열 인덱스로 변환

                // R, G, B 채널 값 저장 (정규화: 0~1 범위로 변환)
                imageData[index] = pixel.r / 255.0f;   // R 채널
                imageData[index + 1] = pixel.g / 255.0f; // G 채널
                imageData[index + 2] = pixel.b / 255.0f; // B 채널
            }
        }

        // [1, height, width, 3] 차원으로 텐서를 생성 (배치 크기 1)
        Tensor inputTensor = new Tensor(1, targetHeight, targetWidth, 3, imageData);

        return inputTensor;
    }

    // 출력 처리 메서드
    void ProcessOutput(Tensor output)
    {
        // 예: 분류 모델의 경우
        int predictedClass = output.ArgMax()[0];
        //Debug.Log($"{predictedClass}");

        if (labels.ContainsKey(predictedClass))
        {
            //Debug.Log("예측된 클래스: " + labels[predictedClass]);
            AiText.text = aitext[predictedClass];
            if(predictedClass == 1){
                uiImage1.color = Color.red;
            }
            else{
                uiImage1.color = Color.white;
            }
        }
        else
        {
            //Debug.Log("예측된 클래스 인덱스: " + predictedClass);
        }
    }
}
