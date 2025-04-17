using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bow : Item
{
    public int damage = 20;
    public float maxRange = 99999.0f;
    public float reloadSpeed = 0.1f;
    public float shotSpacing = 0.05f;
    int chargeLvl = 0;
    float swingCooldown = 0.0f;
    public string chargeAnimationName = "Bow_Charging";
    public string chargedAnimationName = "Bow_Charged";
    public string idleAnimationName = "Bow_Idle";
    Coroutine idleCoroutine;
    bool isCharging = false;
    public ParticleSystem chargeParticles;
    [SerializeField] Color[] particleColors = new Color[3];
    public LineRenderer bolt;
    void Start()
    {
        currentAnimationState = idleAnimationName;
    }

    void Update()
    {
        swingCooldown += Time.deltaTime;
    }
    public override void Primary()
    {
        if (!isCharging && swingCooldown >= reloadSpeed)
        {
            isCharging = true;
            ChangeAnimationState(chargeAnimationName);
            
        }
    }

    public override void Secondary()
    {
        if (chargeLvl > 0)
        {
            isCharging = false;
            swingCooldown = 0.0f;
            ChangeAnimationState(idleAnimationName);
            for (int i = 0; i < chargeLvl; i++) {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxRange, ~gameManager.instance.playerScript.ignoreLayer))
                {
                    Debug.Log(hit.collider.name);

                    LineRenderer line = Instantiate(bolt);
                    Color startColor = particleColors[i];
                    Color endColor = new Color(1, 1, 1, 0.0f);
                    line.SetPosition(0, Camera.main.transform.position);
                    line.SetPosition(1, hit.point);

                    line.DOColor(new Color2(startColor, startColor), new Color2(endColor,endColor), 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        Destroy(line);
                    });
                    chargeLvl--;
                    IDamage dmg = hit.collider.GetComponent<IDamage>();

                    if (dmg != null)
                    {
                        dmg.takeDamage(damage);
                    }
                }
            }
        }
    }


    public void ChargeLevel(int level)
    {
        //if (chargeParticles == null) Debug.Log("Need particles");
        ParticleSystem inst = Instantiate(chargeParticles, Camera.main.transform.position, Quaternion.identity);
        //Debug.Log("Spawned at " + transform.position);
        var p = inst.main;
        p.startColor = particleColors[level];
        inst.gameObject.hideFlags = HideFlags.None;

        chargeLvl++;
        if (chargeLvl == 3)
        {
            ChangeAnimationState(chargedAnimationName);
        }
    }
}
