using UnityEngine;
using System;

[RequireComponent(typeof(ParticleSystem))]
public class CardVFX : MonoBehaviour
{
    public event Action Finished;

    private ParticleSystem particles;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();

        var main = particles.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    private void OnParticleSystemStopped()
    {
        Finished?.Invoke();
    }
}
