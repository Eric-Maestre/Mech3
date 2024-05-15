using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

[RequireComponent(typeof(TMP_Dropdown))]
[DisallowMultipleComponent]
public class SceneSwitcher : MonoBehaviour
{
    public Slider loadSlider;
    TMP_Dropdown sceneSelector;

    List<int> scenes = new List<int>();
    Scene current;

    private void OnValidate()
    {
        sceneSelector = GetComponent<TMP_Dropdown>();
        Scene active = SceneManager.GetActiveScene();
#if UNITY_EDITOR
        sceneSelector.options.Clear();
        scenes.Clear();
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            if (i != active.buildIndex)
            {
                scenes.Add(i);
                sceneSelector.options.Add(new TMP_Dropdown.OptionData(EditorBuildSettings.scenes[i].path));
            }
        }
#else
        //int scenecount = SceneManager.sceneCountInBuildSettings;
        //scenes = new Scene[scenecount];
        //for (int i = 0; i < scenecount; i++)
        //{
        //    if (i != active.buildIndex)
        //    {
        //        Scene temp = SceneManager.GetSceneByBuildIndex(i);
        //        Debug.Log(temp);
        //        scenes[i] = temp;
        //        sceneSelector.options.Add(new TMP_Dropdown.OptionData(temp.name));
        //    }
        //}
#endif
    }
    private void Start()
    {
        OnValidate();
        if (loadSlider)
        {
            loadSlider.maxValue = 2;
            loadSlider.gameObject.SetActive(false);
        }
        sceneSelector.onValueChanged.AddListener(LoadScene);
        LoadScene(0);
    }
    public void LoadScene(int index)
    {
        StartCoroutine(LoadScene_Async(index));
    }

    IEnumerator LoadScene_Async(int index)
    {
        if (loadSlider)
        {
            loadSlider.gameObject.SetActive(true);
        }
        if (current.IsValid())
        {
            AsyncOperation unload = SceneManager.UnloadSceneAsync(current);
            while (!unload.isDone)
            {
                if(loadSlider)
                {
                    loadSlider.value = unload.progress;
                }
                yield return null;
            }
        }
        if (scenes.Count > index)
        {
            AsyncOperation load = SceneManager.LoadSceneAsync(scenes[index], LoadSceneMode.Additive);
            while (!load.isDone)
            {
                if (loadSlider)
                {
                    loadSlider.value = 1 + load.progress;
                }
                yield return null;
            }
            current = SceneManager.GetSceneByBuildIndex(scenes[index]);
        }
        if (loadSlider)
        {
            loadSlider.gameObject.SetActive(false);
        }
    }
}
