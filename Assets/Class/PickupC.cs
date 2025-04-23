using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupC : MonoBehaviour
{
    [SerializeField] GunStats gun;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pickup = other.GetComponent<IPickup>();

        if (pickup != null )
        {
            pickup.GetGunStats(gun);
            gun.ammoCur = gun.ammoMax;
            //gameManager.instance.playerSCript.updatePlayerUI();
            Destroy(gameObject);
        }
    }
}
