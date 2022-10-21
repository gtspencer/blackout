using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    private AudioClip clip;
    
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = this.AddComponent<AudioSource>();

        string[] split = this.name.Split('(');
        string[] split2 = split[split.Length - 1].Split(')');

        int pitchUp = int.Parse(split2[0]);
        pitchUp = pitchUp % 12;
        
        double pitchAmount = Math.Pow(1.05946, pitchUp);

        source.pitch = (float)pitchAmount;
        source.playOnAwake = false;
        source.clip = clip;
        
        // this.GetComponent<Renderer>().enabled = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        source.Play();
    }
}
