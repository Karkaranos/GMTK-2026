using UnityEngine;

public class EventMethods : MonoBehaviour
{
    public void DestroySelf()
        => Destroy(gameObject);

    public void PlayParticle(string particleID)
        => ParticleMaster.INST.Play(particleID, transform);

    public void PlayAudio(string audioID)
    {

    }
}
