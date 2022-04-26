using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 _vector;
    
    private float _angle;

    private float _speed = 6;

    private void Awake()
    {
        _angle = Random.Range(45, 135);
        _vector = new Vector3(Mathf.Cos(_angle), 0, 0);
    }

    private void Update()
    {
        switch (BallsMoveController.Instance.currentStage)
        {
            case Stage.EndGame when GetComponent<Rigidbody>() == null:
            {
                var body=gameObject.AddComponent<Rigidbody>();

                body.useGravity = false;
                body.constraints = RigidbodyConstraints.FreezeAll;
                break;
            }
            case Stage.EndGame:
                transform.localPosition += _vector * Time.deltaTime * _speed;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hole"))
        {
            BallsManager.Instance.RemoveBalls(2);
        }

       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EndEdge"))
        {
            
            
            SetEndVector(collision.contacts[0].normal.normalized);
        }
    }

    private void SetEndVector(Vector3 vectorNormal)
    {
        _vector = ReflectProjectile(vectorNormal);
    }
    
    private Vector3 ReflectProjectile(Vector3 reflectVector)
    {    
        var velocity = Vector3.Reflect(_vector, reflectVector);

        return velocity;
    }
}