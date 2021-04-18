using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Clip { Select, Swap, Clear };
public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    private AudioSource[] sfx;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponents<AudioSource>();
    }

    public void PlaySFX(Clip audioClip)
    {
        sfx[(int)audioClip].Play();
    }
}
