using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private BlackoutController blackoutController;

    [SerializeField] private GameObject wakeUpText;

    [SerializeField] private AudioSource bg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        StartGameIEnum();
    }

    private void StartGameIEnum()
    {
        button.SetActive(false);

        blackoutController.EngageBlackout(Load);
    }

    private IEnumerator LowerVolume()
    {
        while (bg.volume >= 0.01f)
        {
            bg.volume -= 0.001f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Load()
    {
        StartCoroutine(LowerVolume());
        SceneManager.LoadScene("MainScene");
        GameObject.Instantiate(wakeUpText, transform);
        // SceneManager.UnloadSceneAsync ("LoadingScene");
    }
}
