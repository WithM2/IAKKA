using UnityEngine;
using UnityEngine.EventSystems;


// 360 Degree Joystick

// public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
// {
//     private RectTransform joystickBackground;
//     private RectTransform joystickHandle;
//     private Vector2 inputVector;

//     private void Start()
//     {
//         joystickBackground = GetComponent<RectTransform>();
//         joystickHandle = transform.GetChild(0).GetComponent<RectTransform>();
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         Vector2 position;
//         if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out position))
//         {
//             position.x = (position.x / joystickBackground.sizeDelta.x);
//             position.y = (position.y / joystickBackground.sizeDelta.y);

//             inputVector = new Vector2(position.x * 2, position.y * 2);
//             inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

//             // Handle position
//             joystickHandle.anchoredPosition = new Vector2(inputVector.x * (joystickBackground.sizeDelta.x / 2), inputVector.y * (joystickBackground.sizeDelta.y / 2));
//         }
//     }

//     public void OnPointerDown(PointerEventData eventData)
//     {
//         OnDrag(eventData);
//     }

//     public void OnPointerUp(PointerEventData eventData)
//     {
//         inputVector = Vector2.zero;
//         joystickHandle.anchoredPosition = Vector2.zero;
//     }

//     public float Horizontal()
//     {
//         return inputVector.x;
//     }

//     public float Vertical()
//     {
//         return inputVector.y;
//     }
// }


// 4 - way Joystick

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform joystickBackground;
    private RectTransform joystickHandle;
    private Vector2 inputVector;

    private void Start()
    {
        joystickBackground = GetComponent<RectTransform>();
        joystickHandle = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out position))
        {
            position.x = (position.x / joystickBackground.sizeDelta.x) * 2;
            position.y = (position.y / joystickBackground.sizeDelta.y) * 2;

            inputVector = new Vector2(position.x, position.y);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // 4방향 인식
            if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
            {
                inputVector.y = 0; // 수평 방향
            }
            else
            {
                inputVector.x = 0; // 수직 방향
            }

            // Handle position
            joystickHandle.anchoredPosition = new Vector2(inputVector.x * (joystickBackground.sizeDelta.x / 2), inputVector.y * (joystickBackground.sizeDelta.y / 2));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }

    public float Horizontal()
    {
        return inputVector.x;
    }

    public float Vertical()
    {
        return inputVector.y;
    }
}
