using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
  public static SoundManager Instance { set; get; }

    public AudioClip[] allSounds;
    public AudioSource source;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Menu");

    }
    public void PlaySound(int soundIndex)
    {
        AudioSource.PlayClipAtPoint(allSounds[soundIndex], transform.position);
    }
}
