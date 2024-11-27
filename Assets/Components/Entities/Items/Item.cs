using System.Collections;
using System.Collections.Generic;
using UnityEngine;
abstract public class Item : Entity
{
    protected bool isInUse = false;
    public AudioSource audioSource;
    public List<AudioClip> audioClips = new List<AudioClip>();

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public bool CheckIfInUse()
    {
        return isInUse;
    }

    public override void Init()
    {
        base.Init();
        isInUse = false;
    }
    public Dictionary<ItemStats, int> Stats;
    abstract public IEnumerator Use(Mob player);
    abstract public string GetStats();
}
