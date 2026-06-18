using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _spawnEffect;

    public void SetPosition(Vector3 position) => _spawnEffect.transform.position = position;

    public void Launch() => _spawnEffect.Play();

    public void Complete()
    {
        _spawnEffect.Stop();
        _spawnEffect.Clear();
    }
}