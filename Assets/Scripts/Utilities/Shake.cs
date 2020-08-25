using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOShakeRotation(10f, new Vector3(3, 1, 3), 2, 90f, false).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
