using System;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [SerializeField] private ParticleSystem breakParticle;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Path") || collision.gameObject.CompareTag("Ramp"))
        {
            var normal = collision.contacts[0].normal;
            var vector = collision.contacts[0].point - transform.position;
            
            vector.Normalize();

            var alpha = Mathf.Acos(Vector3.Dot(-vector, normal) / (vector.magnitude * normal.magnitude));
            
            /*var speedConstant = Mathf.Sin(alpha);

            if (speedConstant <= 1 && speedConstant >= 0.2f)
            {
                BallsMoveController.Instance.SetSpeed(speedConstant + .2f);
            }
            else if (speedConstant < 0.2f)
            {
                BallsMoveController.Instance.SetSpeed(speedConstant);
            }*/

            var angleX = CalculateAngleX(normal);

            transform.rotation = Quaternion.Euler(new Vector3(angleX, 0, 0));
        }
        else if (collision.gameObject.CompareTag("End") && BallsMoveController.Instance.currentStage == Stage.Game)
        {
            BallsMoveController.Instance.EndStage();
        }
        else if (collision.gameObject.CompareTag("EndEdge") && BallsMoveController.Instance.currentStage == Stage.EndGame)
        {
            BallsMoveController.Instance.SetEndVector(collision.contacts[0].normal.normalized);
        }
    }

    private float CalculateAngleX(Vector3 normal)
    {
        var alpha = Mathf.Atan(normal.z / normal.y) * Mathf.Rad2Deg;;

        if (normal.z<0)
        {
            
        }
        Debug.Log(alpha);

        return alpha;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GateController>())
        {
            var gate = other.GetComponent<GateController>();
            var mathType = gate.mathType;
            var operationNumber = gate.operationNumber;
            var ballCount = BallsManager.Instance.balls.Count;
            
            switch (mathType)
            {
                case MathType.Divide:
                    BallsManager.Instance.RemoveBalls((int) ((1 - 1 / operationNumber) * ballCount));
                    break;
                case MathType.Multiply:
                    BallsManager.Instance.AddBalls((int) (operationNumber - 1) * ballCount);
                    break;
                case MathType.Sum:
                    BallsManager.Instance.AddBalls((int)operationNumber);
                    break;
                case MathType.Subtraction:
                    BallsManager.Instance.RemoveBalls((int)operationNumber);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            BreakEffect(other.transform,gate.color);
        }

        if (other.GetComponent<LevelEndHoleController>())
        {
           BallsManager.Instance.RemoveBalls(2); 
        }
    }
    
    private void BreakEffect(Transform other,Color color)
    {
        other.GetComponent<Collider>().enabled = false;
        
        var main = breakParticle.main;

        main.startColor = color;
        
        var particle = Instantiate(breakParticle);

        particle.transform.position = other.position;

        other.gameObject.SetActive(false);
    }
}
