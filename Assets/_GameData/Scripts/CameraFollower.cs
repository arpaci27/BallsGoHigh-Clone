using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector3 _offSet;

    private void Awake()
    {
        _offSet = transform.position - target.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + _offSet, Time.deltaTime * 10);
    }
}