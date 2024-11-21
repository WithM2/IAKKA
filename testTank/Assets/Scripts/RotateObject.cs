using System.Collections;
using UnityEngine;

public class RotateUI : MonoBehaviour
{
    public RectTransform targetUIElement; // 회전할 UI 요소
    public float rotationSpeed = 90f; // 초당 회전 속도 (각도)
    public int numberOfRotations = 3; // 회전할 횟수

    void Start()
    {
        StartRotation();
    }

    public void StartRotation()
    {
        if (targetUIElement != null) // 대상 UI 요소가 설정되어 있는지 확인
        {
            StartCoroutine(RotateUIElement());
        }
        else
        {
            Debug.LogError("Target UI element is not assigned!");
        }
    }

    private IEnumerator RotateUIElement()
    {
        yield return new WaitForSeconds(0.3f); // 다음 프레임까지 대기

        float totalRotation = 0f;
        float targetRotation = 360f * numberOfRotations;

        while (totalRotation < targetRotation)
        {
            float rotationStep = rotationSpeed * Time.deltaTime; // 프레임 단위 회전 각도 계산
            targetUIElement.Rotate(0, rotationStep, 0); // Z축을 기준으로 회전 (UI는 Z축 회전)
            totalRotation += rotationStep;

            yield return null; // 다음 프레임까지 대기
        }

        // 정확히 목표 회전량으로 맞추기 위해 남은 각도 보정
        float remainingRotation = targetRotation - totalRotation;
        targetUIElement.Rotate(0, 0, remainingRotation);
    }
}