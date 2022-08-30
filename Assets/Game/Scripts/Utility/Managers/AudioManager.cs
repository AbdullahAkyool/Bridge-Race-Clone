using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    
    [SerializeField] private List<AudioClip> audios;
    private AudioListener audioListener;
    [SerializeField] private GameObject _audioSrc;
    private List<AudioSource> _audioSources = new List<AudioSource>();

    private void Start() {
        for(int i = 0; i<20;i++){
            GameObject audioObject = Instantiate(_audioSrc,Vector3.zero,Quaternion.identity,transform);
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();
            _audioSources.Add(audioSource);
        }
    }

    /// <summary> Plays the given audio clip one time </summary>
    public void Play(string name, float delay = 0f, float volume = 1f, float pitch = 1f)
    {
        if (!DataManager.GetAudio()) return;
        
        foreach (var audio in audios)
        {
            if(audio.name.Equals(name))
            {
                StartCoroutine(PlayCo(audio,delay,volume,pitch));
                return;
            }
        }
        Debug.Log("Audio Name:" + name + " is not in the list!");
    }

    
    private IEnumerator PlayCo(AudioClip audio,float delay = 0f, float volume = 1f,float pitch = 1f)
    {
        yield return new WaitForSeconds(delay);
        
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.pitch = pitch;
        audioSource.clip = audio;
        audioSource.PlayOneShot(audio,volume);

        PutAudioSourceBack(audioSource,audio.length);
    }

    private AudioSource GetAvailableAudioSource(){
        AudioSource source = _audioSources[0];
        _audioSources.RemoveAt(0);
        return source;
    }

    private void PutAudioSourceBack(AudioSource audioSource, float delay){

        StartCoroutine(PutAudioSourceBack_CO(audioSource,delay));
        IEnumerator PutAudioSourceBack_CO(AudioSource audioSource, float delay){
            yield return new WaitForSeconds(delay);
            _audioSources.Add(audioSource);
        }
    } 
}
