using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    public GameObject tapToPlay;
    public GameObject scoreScreen;
    public Transform player;
    public Transform endLine;
    public Slider slider;
    private float distance;
    public GameObject nextLevelScreen;

    void Start()
    {
        Time.timeScale = 0f;
        distance = getDistance();

        

        
    }
    void Update()
    {

        scoreText.text = BallsManager.Instance.ballCount.ToString();
        if (BallsMoveController.Instance.currentStage == Stage.EndGame)
        {
            nextLevelScreen.gameObject.SetActive(true);
        }
        if (BallsMoveController.Instance.currentStage != Stage.EndGame)
        {
            nextLevelScreen.gameObject.SetActive(false);
        }

        if (player.position.z <= distance && player.position.z <= endLine.position.z)
        {
            float distance = 1 - (getDistance() / this.distance);
            setProgress(distance);
        }
    }
    public void TapToPlay()
             {
                 Time.timeScale = 1f;
                 tapToPlay.gameObject.SetActive(false);
                 // scoreScreen.gameObject.SetActive(true);
                 slider.gameObject.SetActive(true);
             }
             
             
             float getDistance()
             {
                 return Vector3.Distance(player.position, endLine.position);
             }
     
             void setProgress(float p)
             {
                 slider.value = p;
             }

             public void NextLevel()
             {
                 SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
             }
}
