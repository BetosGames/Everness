using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class DemoMusic : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] tracks;

    public List<int> availableTracks;
    private int selectedTrack;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MusicLoop());
    }

    public IEnumerator MusicLoop()
    {
        if(availableTracks == null || availableTracks.Count == 0) availableTracks = new List<int> { 0, 1, 2 };

        selectedTrack = availableTracks[Random.Range(0, availableTracks.Count)];
        availableTracks.Remove(selectedTrack);

        source.clip = tracks[selectedTrack];
        source.Play();

        yield return new WaitUntil(() => !source.isPlaying);
        yield return new WaitForSeconds(1f);

        StartCoroutine(MusicLoop());
    }
}
