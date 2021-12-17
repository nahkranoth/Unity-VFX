using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAudioResponder : MonoBehaviour
{
    public AudioSpectrumAnalyser audioSpectrum;

    public int bandID;

    public float heightScale;
    
    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(1, audioSpectrum.GetBandValue(bandID) * heightScale, 1);
    }
}
