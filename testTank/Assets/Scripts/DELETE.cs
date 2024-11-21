using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETE : MonoBehaviour
{
    public void Onclicked()
    {
        GameScencesMove.Instance.MoveScene("LoginScene");
    }
}
