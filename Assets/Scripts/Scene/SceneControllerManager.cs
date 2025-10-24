using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControllerManager : SingletonMonobehaviour<SceneControllerManager>
{
   
    private bool isFading;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup faderCanvasGroup = null;
    [SerializeField] private Image faderImage = null;
    public SceneName startingSceneName;


    private IEnumerator Fade(float finalAlpha)
    {

        //set the fading to true so the fade and switch scenes coroutine won't be called again
        isFading = true; 

        //make sure the canvas group thingy is blocking raycasts
        faderCanvasGroup.blocksRaycasts = true;

        //calc how fast the canvas group should fade based on the current alpha, the final alpha, and how long it has to change between the two of them
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        //while the canvas group has not reached the final alpha yet 
        while(!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            //move the alpha towards its target alpha
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

            //wait for a frame then continue 
            yield return null;
        }
        
        //set the flag to false since the fade has finished 
        isFading = false;

        //stop the canvas group from blocking raycasts so input is no longer ignored
        faderCanvasGroup.blocksRaycasts = false;

    }


    private IEnumerator FadeAndSwitchScenes(string sceneName, Vector3 spawnPosition)
    {

        //call before scene unload fade out event
        StaticEventHandler.CallBeforeSceneUnloadFadeOutEvent();

        //start fading to black and wait for it to finish before continuing
        yield return StartCoroutine(Fade(1f));

        //store scene data
        SaveLoadManager.Instance.StoreCurrentSceneData();

        //set player position
        Player.Instance.gameObject.transform.position = spawnPosition;

        //call before scene unload event
        StaticEventHandler.CallBeforeSceneUnloadEvent();

        //unload the current active scene
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        //start loading the given scene and wait for it to finish
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        //call after scene load event 
        StaticEventHandler.CallAfterSceneLoadEvent();

        //restore new scene data
        SaveLoadManager.Instance.RestoreCurrentSceneData();

        //start fading back in and wait for it to finish before exiting the function
        yield return StartCoroutine(Fade(0f));

        //call after scene load fade in event 
        StaticEventHandler.CallAfterSceneLoadFadeInEvent();

    }


    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {

        //allow the given scene to load over several frames and add it to the already loaded scenes 
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        //find the scene that was most recently loaded 
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        //set the newly loaded scene as the active scene
        SceneManager.SetActiveScene(newlyLoadedScene);

    }


    private IEnumerator Start() 
    {

        //set the initial alpha to start off with a black screen 
        faderImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGroup.alpha = 1f;

        //start the first scene loading and wait for it to finish
        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName.ToString()));

        //if the ecent has any subscribers then call it
        StaticEventHandler.CallAfterSceneLoadEvent();

        SaveLoadManager.Instance.RestoreCurrentSceneData();

        //once the scene is finished loading then start fading in
        StartCoroutine(Fade(0f));

    }


    //this will be called when the player decides to switch scenes
    public void FadeAndLoadScene(string sceneName, Vector3 spawnPosition)
    {

        //if a fade is not happening then start fading and switching the scenes
        if(!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName, spawnPosition));
        }

    }


}
