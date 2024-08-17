using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _progresText;
    [SerializeField] private Image _progresBar;
    [SerializeField] private GameObject _canvas;

    private Animator _animator;

    private AsyncOperation _asyncOperation;

    private static SceneLoader instance;

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_asyncOperation != null) 
        {
            _progresText.text = Mathf.RoundToInt(_asyncOperation.progress * 100) + "%";
            _progresBar.fillAmount = Mathf.Lerp(_progresBar.fillAmount, _asyncOperation.progress, Time.deltaTime * 5);
        }
    }

    public void LoadScene(int sceneID)
    {
        _canvas.SetActive(true);
        _animator.SetTrigger("IsOpening");
        _asyncOperation = SceneManager.LoadSceneAsync(sceneID);
        _asyncOperation.allowSceneActivation = false;
        
        while (!_asyncOperation.isDone)
        {
            if (_asyncOperation.progress >= 1f)
            {
                _animator.SetTrigger("IsClosing");
                _canvas.SetActive(false);
                _asyncOperation.allowSceneActivation = true;
                _asyncOperation = null;
            }
        }

        CanvasSwitcher canvasSwitcher = FindObjectOfType<CanvasSwitcher>();
        if (canvasSwitcher != null)
        {
            canvasSwitcher.Initialize();
        }
    }

    private void ClearData()
    {
        GameObject.Find("PlayfabManager(Clone)").GetComponent<CardManager>().ClearList();
    }
}
