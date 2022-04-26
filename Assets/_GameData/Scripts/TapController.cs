using UnityEngine;

public class TapController : MonoSingleton<TapController>
{
    [SerializeField] private LayerMask layerMask;
    
    [SerializeField] private float minXAngle = -60, maxXAngle = 60;

    public bool isFly;

    public float diveAngleSpeed = 2, gravityForBalls = 1;
    

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && isFly && BallsMoveController.Instance.currentStage == Stage.Game)
        {
            Dive();
        }
        else if (isFly && BallsMoveController.Instance.currentStage == Stage.Game)
        {
            DecreaseAngleX(gravityForBalls);
        }
        /*else if (Input.GetMouseButton(0) && !isFly && BallsMoveController.Instance.currentStage == Stage.Game)
        {
            KeepTransformSurface();
        }*/

        if (BallsMoveController.Instance.currentStage == Stage.Game && !isFly && Input.GetMouseButton(0))
        {
            BallsMoveController.Instance.SetSpeed(1.05f);
        }
    }

    private void Dive()
    {
        DecreaseAngleX(diveAngleSpeed);
    }

    private void DecreaseAngleX(float constant)
    {
        var angleX = transform.eulerAngles.x;
        var newAngles = Vector3.zero;

        newAngles.x = angleX + Time.deltaTime * constant;
        newAngles.x = ClampAngle(newAngles.x, minXAngle, maxXAngle);

        //transform.eulerAngles = newAngles;
        transform.rotation = Quaternion.Euler(newAngles);
        //transform.Rotate(Vector3.right * Time.deltaTime * constant);

    }
    
    private static float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;

        return angle > 180f ? Mathf.Max(angle, 360 + from) : Mathf.Min(angle, to);
    }

    private void KeepTransformSurface()
    {
        var ray = new Ray(transform.position, Vector3.down);
        var pos = transform.position;
        
        RaycastHit hitInfo;

        if (Physics.Raycast(ray ,out hitInfo,2,layerMask))
        {
            pos.y = hitInfo.point.y;
        }

        transform.position = pos;
    }

    private void CheckGround()
    {
        var ray = new Ray(transform.position + Vector3.up * 2, Vector3.down);

        RaycastHit hitInfo;

        isFly = !Physics.Raycast(ray ,out hitInfo,4,layerMask);
    }
}
