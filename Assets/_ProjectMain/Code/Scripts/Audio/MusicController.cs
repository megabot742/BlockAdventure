using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioClip[] songs;
    AudioSource source;
    List<AudioClip> playlist;
    void Start()
    {
        int numOfMusicPlayers = FindObjectsByType<MusicController>(FindObjectsSortMode.None).Length;

        if (numOfMusicPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            source = GetComponent<AudioSource>();
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
            source.loop = false;
            ResetPlaylist();
            PlayNext();
        }
    }
    void Update()
    {
        if (!source.isPlaying)
        {
            PlayNext();
        }
    }
    private void PlayNext()
    {
        if (playlist.Count == 0)
        {
            ResetPlaylist();
        }
        // Random song
        int randomIndex = Random.Range(0, playlist.Count);
        AudioClip selectedClip = playlist[randomIndex];
        
        // Remove song
        playlist.RemoveAt(randomIndex);
        
        // Play SONG
        source.clip = selectedClip;
        source.Play();
    }
    private void ResetPlaylist()
    {
        //Rset Playlist
        playlist = new List<AudioClip>(songs);
    }
}
