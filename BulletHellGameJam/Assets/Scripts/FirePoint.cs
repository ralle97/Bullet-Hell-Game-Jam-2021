using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    public void ChangePosition(Vector2 newPos)
    {
        transform.localPosition = newPos;
    }
}
