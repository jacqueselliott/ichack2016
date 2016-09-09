using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public string dir;
    public bool start = true;
    public AudioClip music;

    void Awake()
    {
        if (start)
        {
            dir = "icHackPics";
            start = false;
           
        }
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}