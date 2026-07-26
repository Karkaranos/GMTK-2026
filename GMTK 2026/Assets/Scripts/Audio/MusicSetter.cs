using System;
using UnityEngine;

public class MusicSetter : MonoBehaviour
{
    [SerializeField] private MusicType musicType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.SetMusic(musicType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
