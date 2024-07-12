// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;

// public class CameraFeed : MonoBehaviour
// {
//     public RawImage cameraImage;
//     public string streamURL = "http://your_camera_stream_url"; // 실제 스트림 URL

//     void Start()
//     {
//         StartCoroutine(StartStream());
//     }

//     IEnumerator StartStream()
//     {
//         while (true)
//         {
//             using (WWW www = new WWW(streamURL))
//             {
//                 yield return www;
//                 if (www.error == null)
//                 {
//                     Texture2D texture = new Texture2D(2, 2);
//                     www.LoadImageIntoTexture(texture);
//                     cameraImage.texture = texture;
//                 }
//                 else
//                 {
//                     Debug.LogError("Failed to load stream: " + www.error);
//                 }
//             }
//             yield return new WaitForSeconds(0.5f); // 일정 시간 간격으로 스트림 갱신
//         }
//     }
// }


using UnityEngine;
using UnityEngine.UI;

public class CameraFeed : MonoBehaviour
{
    public RawImage cameraImage;
    private WebCamTexture webcamTexture;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            webcamTexture = new WebCamTexture(devices[0].name);
            cameraImage.texture = webcamTexture;
            cameraImage.material.mainTexture = webcamTexture;
            webcamTexture.Play();
        }
        else
        {
            Debug.LogError("No camera found on the device.");
        }
    }

    void OnDisable()
    {
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
        }
    }
}
