using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour//, IDataPersistence
{
    public static TimerController Instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (instance == null)
                // 씬에서 GameManager 오브젝트를 찾아 할당
                instance = FindObjectOfType<TimerController>();

            // 싱글톤 오브젝트를 반환
            return instance;
        }
    }
    public static TimerController instance;

    public Text timeCounter;

    private TimeSpan timePlaying;
    private bool timerGoing;

    public float elapsedTime = 0f;

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        // timeCounter.text = "Time: 00:00.00";
        // timerGoing = false;
        BeginTimer();
    }

    public void BeginTimer()
    {
        timerGoing = true;
        StartCoroutine(UpdateTimer());
        Time.timeScale = 1f;

    }

    public void EndTimer()
    {
        timerGoing = false;
        Time.timeScale = 0f;
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timeCounter.text = timePlayingStr;

            yield return null;
        }
    }

    // public void LoadData(GameData data)
    // {
    //     this.elapsedTime = data.playTime;
    // }
    // public void SaveData(ref GameData data)
    // {
    //     data.playTime = this.elapsedTime;
    // }
}
