using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
// 점수와 게임 오버 여부, 게임 UI를 관리하는 게임 매니저
public class GameManager : MonoBehaviour
{
    public bool gameClear;
    private static GameManager instance; // 싱글톤이 할당될 static 변수
    public GameObject EscCanvas;
    public GameObject DefaultCanvas;

    public GameObject gameResultCanvas;
    public Text gameResultText;
    public Bishop b1;
    public Bishop b2;
    public bool escPressed { get; private set; }
    public bool isPlayerHidden;
    public Transform safeView;
    public bool isEnemyNear;

    public Text featherCounter;
    public Text playTimeText;

    public Text hpText;
    public int featherCount;

    public bool detectedByQueen;
    public bool detectedByBishop;
    public Transform playerTransform;
    public float playerHealth;
    AudioSource audioSource;
    public AudioClip heartBeatSound;
    // 외부에서 싱글톤 오브젝트를 가져올때 사용할 프로퍼티
    public static GameManager Instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (instance == null)
                // 씬에서 GameManager 오브젝트를 찾아 할당
                instance = FindObjectOfType<GameManager>();

            // 싱글톤 오브젝트를 반환
            return instance;
        }
    }

    private int score; // 현재 게임 점수
    public bool isGameover { get; private set; } // 게임 오버 상태

    private void Awake()
    {
        DefaultCanvas.SetActive(true);
        gameResultCanvas.SetActive(false);
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면 자신을 파괴
        if (Instance != this) Destroy(gameObject);

    }
    private void Start()
    {AudioListener.pause = false;
        playerHealth = 100f;
        escPressed = false;
        featherCount = 0;
        isPlayerHidden = false;
        audioSource = GetComponent<AudioSource>();
        if (InGameData.isloaded)
        {

            DataManager.Instance.LoadGameData();
            GameManager.instance.featherCount -= 1;
            Debug.Log("Loaded");
        }

    }



    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            score += newScore;
            // 점수 UI 텍스트 갱신
            // UIManager.Instance.UpdateScoreText(score);
        }
    }
    public void Update()
    {
        hpText.text = "HP: " + (int)(playerHealth);

        featherCounter.text = "Feather: " + featherCount;
        if (featherCount == 5)
        {
            gameClear = true;
            DefaultCanvas.SetActive(false);
            gameResultText.text = "Stage1: Clear";
            gameResultCanvas.SetActive(true);
            if (Time.timeScale == 0) return;
            TimerController.instance.EndTimer();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            TimerController.instance.timePlaying.ToString("mm':'ss'.'ff");
            playTimeText.text = "Play Time: " + TimerController.instance.timePlaying.ToString("mm':'ss'.'ff");
            AudioListener.pause = true;
            DataManager.Instance.SaveGameData();

        }
        if (playerHealth <= 0)
        {
            gameClear = false;
            DefaultCanvas.SetActive(false);
            TimerController.instance.EndTimer();
            gameResultText.text = "Stage1: Fail";
            playTimeText.text = "Play Time: " + TimerController.instance.timePlaying.ToString("mm':'ss'.'ff");
            gameResultCanvas.SetActive(true);
            AudioListener.pause = true;
            DataManager.Instance.SaveGameData();
        }
        detectedByBishop = b1.playerOnSight || b2.playerOnSight;
        Debug.Log(detectedByBishop);
        // Debug.Log(featherCount);
        if (isPlayerHidden)
        {
            b1.playerOnSight = false;
            b2.playerOnSight = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {


            if (escPressed == false)
            {
                OnEscPressed();
                escPressed = true;
            }
            else
            {
                EscCanvas.SetActive(false);
                escPressed = false;
                TimerController.instance.BeginTimer();
            }

        }
        if (isEnemyNear)
        {

            audioSource.clip = heartBeatSound;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }

    }

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        // UIManager.Instance.SetActiveGameoverUI(true);
    }
    public void OnEscPressed()
    {
        TimerController.instance.EndTimer();
        EscCanvas.SetActive(true);
        audioSource.Stop();

    }
    public void ResumeGame()
    {
        EscCanvas.SetActive(false);
        escPressed = false;
        TimerController.instance.BeginTimer();
    }




}


