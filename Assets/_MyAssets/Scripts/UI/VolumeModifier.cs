using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeModifier : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private LimitlessGlitch8 healthGlitch;
    [SerializeField] private float effectStartThreshold;
    [SerializeField] private LimitlessGlitch8 gameOverGlitch;

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out healthGlitch);
        healthGlitch.Amount.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
