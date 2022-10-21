using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackoutController : MonoBehaviour
{
    public GameObject pixel;

    [SerializeField] private bool startOnStart = true;
    [SerializeField] private bool doInUpdate;

    private System.Random r;

    private int numWidth;

    private int numHeight;

    private int total;

    private List<GameObject> pixels;
    // Start is called before the first frame update
    void Start()
    {
        var width = (int)pixel.GetComponent<RectTransform>().rect.width;
        var height = (int)pixel.GetComponent<RectTransform>().rect.height;
        r = new System.Random();
        pixels = new List<GameObject>();
        
        numWidth = Screen.width / width;
        numHeight = Screen.height / height;

        numHeight += 2;
        numWidth += 2;
        
        for (int i = 0; i <= numWidth; i++)
        {
            for (int j = 0; j <= numHeight; j++)
            {
                var pix = GameObject.Instantiate(pixel, transform);
                pix.transform.position = new Vector3((i * width), (j * height), pix.transform.position.z);
                pix.SetActive(false);
                
                pixels.Add(pix);
            }
        }

        total = pixels.Count;
        
        if (startOnStart)
            InvokeRepeating(nameof(BlackoutTimer), 1f, 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        if (doInUpdate)
        {
            BlackRandom();
            
            if (total <= 0)
                doneCallback.Invoke();
        }
    }

    public int GetTotal()
    {
        return total;
    }

    public void EngageBlackout(Action doneCallback)
    {
        if (total == 0)
            return;
        this.doneCallback = doneCallback;
        /*CancelInvoke(nameof(BlackoutTimer));
        InvokeRepeating(nameof(BlackoutTimer), 0f, 0.0001f);*/
        doInUpdate = true;
    }

    private Action doneCallback;

    private void BlackRandom()
    {
        if (total <= 0)
        {
            CancelInvoke(nameof(BlackoutTimer));
            return;
        }
        
        var index = r.Next(0, total);

        pixels[index].SetActive(true);
        pixels.RemoveAt(index);
        
        total--;
    }

    private void BlackoutTimer()
    {
        BlackRandom();
    }
}
