using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{

    public ParticleSystem[] AllParticles;
    // Start is called before the first frame update
    void Start()
    {
        AllParticles = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayFX()
    {
        foreach (ParticleSystem item in AllParticles)
        {
            item.Stop();
            item.Play();
        }
    }
}
