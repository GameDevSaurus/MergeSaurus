using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleExplosion : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _confettiRain;
    [SerializeField]
    ParticleSystem _confettiExplosion;
    [SerializeField]
    ParticleSystem _sparks;
    [SerializeField]
    ParticleSystem _implosion;
    [SerializeField]
    ParticleSystem _explosion;
    [SerializeField]
    ParticleSystem _capsuleIn;
    [SerializeField]
    ParticleSystem _godRays;
    [SerializeField]
    ParticleSystem _dustIn;
    [SerializeField]
    ParticleSystem _dustOut;
    [SerializeField]
    ParticleSystem _haloSquares;
    [SerializeField]
    ParticleSystem _haloExplosion;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(CrPlayParticle());
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Explode();
        }
    }
    public void Explode()
    {
        _confettiExplosion.Play();
        _sparks.Play();
        _haloSquares.Play();
        _dustOut.Play();
        _confettiRain.Play();
        _haloExplosion.Play();
    }

    IEnumerator CrPlayParticle()
    {
        _godRays.Play();
        _capsuleIn.Play();
        _dustIn.Play();
        yield return new WaitForSeconds(2f);
        _implosion.Play();
        yield return new WaitForSeconds(0.5f);
        _explosion.Play();
        yield return new WaitForSeconds(2f);
        _explosion.Stop();
        _explosion.Stop();
        _dustIn.Stop();
        _capsuleIn.Stop();
        _godRays.Stop();
        Explode();
    }
}
