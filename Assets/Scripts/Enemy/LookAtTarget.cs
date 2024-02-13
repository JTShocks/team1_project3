using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;
    public float lookAtSpeed = 1f;

    private Coroutine lookCoroutine;

    public void StartRotating()
    {
        if(lookCoroutine != null)
        {
            StopCoroutine(lookCoroutine);
        }

        lookCoroutine = StartCoroutine(LookAt());
    }

    private IEnumerator LookAt()
    {
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);

        float time = 0;
        while(time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * lookAtSpeed;

            yield return null;
        }
    }
}
