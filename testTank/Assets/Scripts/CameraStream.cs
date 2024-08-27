using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraStream : MonoBehaviour
{
    public RawImage rawImage;  // 유니티 에디터에서 설정해야 할 RawImage 컴포넌트
    public string esp32CamURL = "http://192.168.35.95/capture"; // ESP32-CAM의 스트림 URL 
                                                                  //연결된 와이파이에 따라 변경 될 수 있음
    void Start()
    {
        StartCoroutine(LoadCameraStream());
    }

    IEnumerator LoadCameraStream()
    {
        while (true)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(esp32CamURL))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + www.error);
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    rawImage.texture = texture;
                    rawImage.SetNativeSize();
                }
            }

            yield return new WaitForSeconds(0.05f); // 스트림 갱신 간격
        }
    }
}
