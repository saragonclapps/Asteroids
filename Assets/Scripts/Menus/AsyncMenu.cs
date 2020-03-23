using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncMenu : MonoBehaviour 
{
	public Image fillBar;
	public GameObject loadingPanel;
    public string toLoad;
    private Fading _fading;

    private void Awake()
    {
        _fading = FindObjectOfType<Fading>();
    }

    public void SelectToLoadGame(string toLoad)
    {
        this.toLoad = toLoad;
        LoadGame();
    }

    public void LoadGame(){
        StartCoroutine(FadingTransition());
        loadingPanel.SetActive (true);
		StartCoroutine (LoadScene());
	}

    public void SetToLoad(string value){
        toLoad = value;
    }


    protected IEnumerator FadingTransition()
    {
        _fading.BeginFade(1);
        yield return new WaitForSeconds(0.6f);
        _fading.BeginFade(-1);
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(toLoad);
        while (operation.progress < 0.95f){
            AudioListener.volume = 0.7f - operation.progress;
            if (fillBar)fillBar.fillAmount = operation.progress;
            yield return null;
		}
        _fading.BeginFade(1);
        loadingPanel.SetActive (false);
	}
}