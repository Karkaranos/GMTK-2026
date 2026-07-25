using UnityEngine;

public class EventMethods : MonoBehaviour
{
    public void DestroySelf()
        => Destroy(gameObject);

    public void PlayParticle(string particleID)
        => ParticleMaster.INST.Play(particleID, transform);

    public void LerpGlobalLightingIntensity(float value)
        => GlobalLight2DController.INST.BeginLerpIntensity(value);
    public void PlayAudio(string audioID)
    {

    }
}
