

using UnityEngine;

public class BGM : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        // Resources/BGM.wav Çì«Ç›çûÇﬁ
        AudioClip clip = Resources.Load<AudioClip>("BGM");
        audioSource.clip = clip;

        audioSource.loop = true;
        audioSource.Play();
    }
}