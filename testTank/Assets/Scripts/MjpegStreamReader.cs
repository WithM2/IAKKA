using System; // 기본 시스템 기능을 위한 네임스페이스
using System.IO; // 데이터 입출력 처리
using System.Net; // HTTP 요청 처리를 위한 네임스페이스
using System.Threading; // 스레드 처리를 위한 네임스페이스
using UnityEngine; // Unity에서 기본 사용되는 기능
using UnityEngine.UI; // Unity UI, 특히 RawImage 사용을 위한 네임스페이스

public class MjpegStreamReader : MonoBehaviour
{
    [SerializeField]
    private UnityMainThreadDispatcher unityMainThreadDispatcher;

    public GameObject RawImage; // RawImage 오브젝트

    [SerializeField]
    private string mjpegUrl = "http://192.168.35.95:81/stream"; // MJPEG 스트림 URL 설정
    public RawImage rawImageDisplay; // UI 요소로 MJPEG 스트림을 표시할 RawImage
    private Texture2D texture; // 스트림 이미지를 표시할 텍스처
    private Thread streamThread; // 스트림을 처리할 스레드
    private bool isRunning = true; // 스트림 읽기 상태 제어 플래그

    void OnEnable()
    {
        // 텍스처 초기화 - 기본 크기로 시작 (2x2)
        texture = new Texture2D(2, 2, TextureFormat.RGB24, false);

        // MJPEG 스트림을 읽기 시작하는 새로운 스레드를 생성하고 시작
        streamThread = new Thread(new ThreadStart(StreamMJPEG));
        streamThread.Start();
    }

    void OnDisable()
    {
        // 스크립트 비활성화 시 스레드를 종료
        isRunning = false;
        if (streamThread != null && streamThread.IsAlive)
        {
            streamThread.Join(); // 스레드가 종료될 때까지 대기
        }
    }

    void StreamMJPEG()
    {
        try
        {
            // HTTP 요청을 사용하여 MJPEG 스트림에 연결
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mjpegUrl);
            request.Timeout = int.MaxValue; // 시간 초과를 무한으로 설정하여 스트림 지속 가능

            // 응답을 받고 스트림을 열어 데이터 수신
            using (WebResponse response = request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                byte[] buffer = new byte[1280 * 720]; // 최대 1MB 크기의 데이터를 수용할 수 있는 버퍼
                MemoryStream ms = new MemoryStream(); // 데이터를 수집할 메모리 스트림

                int bytesRead;
                while (isRunning && (bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0) // 스트림에서 데이터를 읽음
                {
                    ms.Write(buffer, 0, bytesRead); // 수신한 데이터를 메모리 스트림에 기록
                    byte[] imageBytes = ms.ToArray(); // 기록된 데이터를 바이트 배열로 변환

                    // JPEG 이미지의 시작(0xFFD8) 및 끝(0xFFD9)을 찾아 이미지 분리
                    int startIdx = FindJpegHeader(imageBytes, imageBytes.Length);
                    int endIdx = -1;
                    if(startIdx != -1){
                        endIdx = FindJpegFooter(imageBytes, startIdx, imageBytes.Length);
                    }

                    // JPEG 데이터가 완전한 경우에만 처리
                    if (startIdx < imageBytes.Length && endIdx < imageBytes.Length && 
                        startIdx != -1 && endIdx != -1 && endIdx > startIdx)
                    {
                        int length = endIdx - startIdx + 1; // JPEG 데이터 길이 계산
                        byte[] jpg = new byte[length];
                        Array.Copy(imageBytes, startIdx, jpg, 0, length); // 특정 길이의 이미지만 복사

                        // 다음 이미지 처리를 위해 스트림과 버퍼 초기화
                        ms.SetLength(0);

                        // 다음 이미지의 시작 부분이 이미 있는 경우 남은 부분 유지
                        if (endIdx < imageBytes.Length)
                        {
                            int remainingLength = imageBytes.Length - (endIdx + 1);
                            ms.Write(imageBytes, endIdx + 1, remainingLength);
                        }
                        // UI 업데이트 - 메인 스레드에서 수행
                        UpdateTexture(jpg);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error in MJPEG stream: " + e.Message); // 스트림 연결/처리 오류 처리
        }
    }

    private int FindJpegHeader(byte[] buffer, int length)
    {
        for (int i = 0; i < length - 1; i++)
        {
            if (buffer[i] == 0xFF && buffer[i + 1] == 0xD8){
                return i;
            }
        }
        return -1;
    }

    private int FindJpegFooter(byte[] buffer, int startIdx, int length)
    {
        for (int i = startIdx; i < length - 1; i++)
        {
            if (buffer[i] == 0xFF && buffer[i + 1] == 0xD9){
                return i + 1;
            }
        }
        return -1;
    }

    void UpdateTexture(byte[] jpegData)
    {
        if (jpegData.Length > 0)
        {
            unityMainThreadDispatcher.Enqueue(() =>
            {
                texture.LoadImage(jpegData);
                rawImageDisplay.texture = texture;
            });
        }
    }
    public void camera_control(bool isture){
        RawImage.SetActive(isture);
    }
}
