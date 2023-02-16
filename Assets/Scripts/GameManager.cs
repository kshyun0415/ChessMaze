using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
// 점수와 게임 오버 여부, 게임 UI를 관리하는 게임 매니저
public class GameManager : MonoBehaviour
{
    private static GameManager instance; // 싱글톤이 할당될 static 변수
    public GameObject EscCanvas;
    public bool escPressed { get; private set; }
    public bool isPlayerHidden;
    public Transform safeView;
    public bool isEnemyNear;

    public Text featherCounter;

    public int featherCount;

    public bool detectedByQueen;
    public Transform playerTransform;

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
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면 자신을 파괴
        if (Instance != this) Destroy(gameObject);

    }
    private void Start()
    {
        escPressed = false;
        featherCount = 0;
        isPlayerHidden = false;
        audioSource = GetComponent<AudioSource>();
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
        featherCounter.text = "Feather: " + featherCount;
        // if (featherCount == 5)
        // {
        //     DataPersistenceManager.instance.SaveGame();
        // }
        // Debug.Log(featherCount);
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


    public void LoadData(GameData data)
    {
        this.featherCount = data.featherCount;
    }
    public void SaveData(ref GameData data)
    {
        data.featherCount = this.featherCount;
    }


}


