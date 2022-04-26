using UnityEngine;

public class EndController : MonoBehaviour
{
    public GameObject secondCamera;
    public GameObject firstCamera;

    private void Update()
    {
        if (BallsMoveController.Instance.currentStage == Stage.EndGame)
        {
            firstCamera.gameObject.SetActive(false);
            secondCamera.gameObject.SetActive(true);
        }
    }
}
