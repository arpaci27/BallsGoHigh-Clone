using UnityEngine; //using DG.Tweening;

public enum Stage
{
    Game,
    EndGame
}

public class BallsMoveController : MonoSingleton<BallsMoveController>
{
    private Rigidbody _body;

    private Vector3 _endVector = new Vector3(1, 0, 1);

    public Stage currentStage = Stage.Game;

    public float speed;
    public float maxSpeed = 100;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (currentStage == Stage.Game)
        {
            transform.position+=transform.forward * Time.deltaTime * speed;
        }
        else if (currentStage == Stage.EndGame)
        {
            transform.position += Vector3.forward * Time.deltaTime * 6;
        }
    }
    
    private void SetUSeGravity(bool useGravity)
    {
        _body.useGravity = useGravity;
    }
    
    public void SetSpeed(float multiplyConstant)
    {
        speed = Mathf.Clamp(speed * multiplyConstant, 0, maxSpeed);
    }

    public void SetEndVector(Vector3 vectorNormal)
    {
        _endVector = ReflectProjectile(vectorNormal);
    }
    
    private Vector3 ReflectProjectile(Vector3 reflectVector)
    {    
        var velocity = Vector3.Reflect(_endVector, reflectVector);

        return velocity;
    }

    public void EndStage()
    {
        currentStage = Stage.EndGame;

        transform.rotation = Quaternion.Euler(Vector3.zero);

        _body.detectCollisions = false;
        
        SetUSeGravity(false);
    }
    
    

    
}