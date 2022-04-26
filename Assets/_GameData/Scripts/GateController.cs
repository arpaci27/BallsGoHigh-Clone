using UnityEngine;

public enum MathType
{
    Multiply,
    Divide,
    Sum,
    Subtraction
}
public class GateController : MonoBehaviour
{
    
    public Color color;
    
    public MathType mathType;

    public float operationNumber;

    private void Awake()
    {
        color = GetComponent<Renderer>().material.GetColor("_BaseColor");
    }

    
}