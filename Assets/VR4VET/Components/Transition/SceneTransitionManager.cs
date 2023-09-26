using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneTransitionManager : MonoBehaviour
{
    //Setting up needed variables and displays them for debugging
    private bool _isFaded;
    //The build index of the scene to be loaded (Only sed for debugging with 'space')
    private int _nextScene = 0;
    //The overlay image that will be used for the fade
    [SerializeField] private RawImage _rawImage;

    // Just a checking if 'space' is pressed and runs the function if it is (to test the script without needing to put on the VR headset)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            StartCoroutine(TransitionMethod(_nextScene));
        }
    }
    //Call this method to start the transition
    public void TransitionScene(int nextSceneIndex)
    {
     StartCoroutine(TransitionMethod(nextSceneIndex));
    }


     IEnumerator TransitionMethod(int nextSceneIndex)
    {
        //Makes it so the transitionManager doesn't unload along with the rest of the scene
        DontDestroyOnLoad(gameObject);
        //Starts the actual screen fade
        StartCoroutine(TransitionFade());
        //Loads the new scene without unloading the current scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex);
        //prevents the new scene from fully loading
        asyncLoad.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads and the screen is fully faded before proceeding with the code
        while (!asyncLoad.isDone && !_isFaded)
        {
        yield return null;
        }
        //Allows the new scene to be loaded
        asyncLoad.allowSceneActivation = true;
        //fade the screen back in
        StartCoroutine(TransitionFade());
    }


        IEnumerator TransitionFade(){
        Color c = _rawImage.color;
       //Fades the screen in or out across 1 second
       if (_isFaded){
        for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
    {
        c.a = alpha;
        _rawImage.color = c;
        yield return new WaitForSeconds(.01f);

    }
    c.a -= 0.1f;
    _isFaded = false;
    //Destroys the manager after loading the screen back in
    Destroy(gameObject);
    
    }else{ for (float alpha = 0f; alpha <= 1; alpha += 0.01f)
    {
        c.a = alpha;
        _rawImage.color = c;
        yield return new WaitForSeconds(.01f);

    }
    c.a += 0.1f;
    _isFaded = true;
    }
            yield return null;
        }
}
