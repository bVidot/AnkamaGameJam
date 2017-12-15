using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{

    private static UiManager _instance = null;

    public static UiManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UiManager>();

                if (_instance == null)

                {
                    GameObject go = new GameObject();
                    go.name = "UiManager";
                    _instance = go.AddComponent<UiManager>();
                }

            }

            return _instance;

        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void LoadTargetScene(int index, float delay=0f)
    {
        StartCoroutine(LoadWithDelay(index, delay));
    }

    IEnumerator LoadWithDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(index);
    }

}
