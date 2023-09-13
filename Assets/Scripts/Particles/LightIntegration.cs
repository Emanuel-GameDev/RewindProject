using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(ParticleSystem))]
public class LightIntegration : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [Tooltip("Use this divider to decrease the objects instantiated in order to increase performances")]
    [SerializeField] private int objNumDivider = 1;
    [Tooltip("Time needed for the objects instantiated to fade out")]
    [SerializeField] private float fadeTime = 1f;

    private ParticleSystem _particleSystem;
    private List<GameObject> instances = new List<GameObject>();
    private ParticleSystem.Particle[] particles;
    private IEnumerator fadeCoroutine;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
    }

    private void LateUpdate()
    {
        // Ref to max particles
        int count = _particleSystem.GetParticles(particles) / objNumDivider; // Divide by 3 to decrease num of objects instantiated and increase performance

        while (instances.Count < count)
            instances.Add(Instantiate(prefab, _particleSystem.transform));

        bool worldSpace = (_particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < instances.Count; i++)
        {
            //instances[i].transform.localScale = new Vector3(particles[i].startSize, particles[i].startSize, particles[i].startSize);

            if (i < count)
            {
                if (worldSpace)
                    instances[i].transform.position = particles[i].position;
                else
                    instances[i].transform.localPosition = particles[i].position;

                instances[i].SetActive(true);
            }
            else
            {
                instances[i].SetActive(false);
            }
        }
    }

    private IEnumerator Fade(GameObject instance)
    {
        Light2D light = instance.transform.GetComponent<Light2D>();

        float timeElapsed = 0f;
        float initialIntensity = light.intensity;

        while (timeElapsed < fadeTime)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / fadeTime;

            light.intensity = Mathf.Lerp(initialIntensity, 0f, t);

            yield return null;
        }
    }
}
