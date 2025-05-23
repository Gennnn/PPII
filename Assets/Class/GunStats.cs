using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/Gun Stats")]
public class GunStats : ScriptableObject
{
    public GameObject model;
    [Range(1,10)]
    public int shootDamage;
    [ Range(5,1000)]
    public int shootDist;
    [Range(0.1f,3)]
    public float shootRate;
    public int ammoCur;
    [Range(5,50)]
    public int ammoMax;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    [Range(0,1)]
    public float shootSoundVol;
}
