using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Transform swordTrans;
    public float swingSpeed = 720.0f;

    Vector3 rotateEnd;

    private void OnEnable()
    {
        rotateEnd = swordTrans.eulerAngles;
        rotateEnd += new Vector3(0, 0, 90.0f);
    }

    private void FixedUpdate()
    {
        swordTrans.Rotate(new Vector3(0, 0, swingSpeed) * Time.deltaTime);
        Vector3 check = rotateEnd - swordTrans.eulerAngles;
        if (Mathf.Abs(check.z) < 3.0f) gameObject.SetActive(false);
    }
}
