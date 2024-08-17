using System.Collections;
using UnityEngine;

public class FighterMove : MonoBehaviour
{
    private float _rotateSpeed = 360f;
    private float _moveSpeed = 50f;

    public Coroutine StartMove(Transform target, float stoppingDistance = 0)
    {
        return StartCoroutine(MoveTo(target, stoppingDistance));
    }

    private IEnumerator MoveTo(Transform target, float stoppingDistance = 0)
    {
        while (Vector3.Distance(transform.position, target.position) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, _moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public Coroutine StartLookAtRotation(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Quaternion rotation = lookRotation * Quaternion.Euler(0, -270f, 0);

        return StartCoroutine(RotateTo(rotation));
    }


    public Coroutine StartRotation(Quaternion targetRotation)
    {
        return StartCoroutine(RotateTo(targetRotation));
    }

    public IEnumerator RotateTo(Quaternion targetRotation)
    {
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
