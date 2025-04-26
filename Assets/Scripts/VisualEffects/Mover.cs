using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 targetPosition, offset;
    [SerializeField] private float stoppingDistance = 0.1f;

    private Coroutine _moveCoroutine;

    public void Move() => MoveToPoint(targetPosition);

    public void MoveToOffset() => MoveToPoint((Vector2)transform.position - offset);

    public void MoveToPoint(Vector2 targetPoint)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
        _moveCoroutine = StartCoroutine(MoveToPointCoroutine(targetPoint));
    }

    private IEnumerator MoveToPointCoroutine(Vector2 targetPosition)
    {
        while (Vector2.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
    }

    public void StopMovement()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }
    }
}