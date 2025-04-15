using UnityEngine;

public class QuackClick : MonoBehaviour
{
    public AudioClip quackClip;
    private AudioSource audioSource; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayQuackSound();
        }
    }

    void PlayQuackSound()
    {
        if (quackClip != null)
        {
            audioSource.PlayOneShot(quackClip);
        }
    }
}