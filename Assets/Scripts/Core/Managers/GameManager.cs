using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private Coroutine _startRunCoroutine;
    private RunManager _runManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void RegisterRunManager(RunManager runManager)
    {
        if (runManager == null)
        {
            _runManager = FindAnyObjectByType<RunManager>();
            if (_runManager == null)
            {

                Debug.LogError("RunManager is null in GameManager.");
                return;
            }
        }
        else
        {
            _runManager = runManager;
        }
    }

    private void ExitToMain()
    {
        if (_startRunCoroutine != null)
        {
            StopCoroutine(_startRunCoroutine);
        }
        SceneManager.LoadScene("Main");
    }

    private void StartNewRun()
    {
        if (_startRunCoroutine != null)
        {
            StopCoroutine(_startRunCoroutine);
        }
        StartRun();
    }

    private void StartRun()
    {
        _startRunCoroutine = StartCoroutine(StartRunCoroutine());
    }

    private void OnApplicationQuit()
    {
        Debug.Log(" prima o poi salverò qualcosa");
    }


    private void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    IEnumerator StartRunCoroutine()
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync("Run");
        while (!loadOp.isDone)
        {
            yield return null;
        }
    }
}
