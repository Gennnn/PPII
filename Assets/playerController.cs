using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class playerController : MonoBehaviour, IDamage, IPickup
{
    public LayerMask ignoreLayer;
    public CharacterController characterController;

    [SerializeField] int HP;
    [SerializeField] int mana;
    [SerializeField] int manaRegenAmount;
    [SerializeField] float manaRegenSeconds;
    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;

    int jumpCount;
    public int HPOrig;
    public int manaOrig;

    float shootTimer;

    Vector3 moveDir;
    Vector3 playerVel;

    [System.NonSerialized]
    public bool isSprinting;

    [System.NonSerialized]
    public UnityEvent<bool> sprintChangeEvent;

    [System.NonSerialized]
    public UnityEvent<int> hpUpdatedEvent;
    [System.NonSerialized]
    public UnityEvent<int> manaUpdatedEvent;

    [System.NonSerialized]
    public UnityEvent<int> playerSwing;
    [System.NonSerialized]
    public UnityEvent<int> swapSlot;

    void Awake()
    {
        sprintChangeEvent = new UnityEvent<bool>();
        hpUpdatedEvent = new UnityEvent<int>();
        manaUpdatedEvent = new UnityEvent<int>();
        playerSwing = new UnityEvent<int>();
        swapSlot = new UnityEvent<int>();
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        hpUpdatedEvent.Invoke(HP);
        if (HP <= 0)
        {
            gameManager.instance.youLose();
        } 
    }

    void Start()
    {
        HPOrig = HP;
        manaOrig = mana;
        StartCoroutine(ManaRegen(manaRegenAmount, manaRegenSeconds));
    }


    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        sprint();
        movement();
        ItemSelection();
        if (Input.GetButtonDown("Fire2")) {
            playerSwing.Invoke(1);
        }
        if (Input.GetButton("Fire1"))
        {
            playerSwing.Invoke(0);
        }
    }

    void ItemSelection()
    {
        if (Input.GetButtonDown("Slot1"))
        {
            swapSlot.Invoke(0);
        } else if (Input.GetButtonDown("Slot2"))
        {
            swapSlot.Invoke(1);
        } else if (Input.GetButtonDown("Slot3"))
        {
            swapSlot.Invoke(2);
        }
    }

    void TestHeal()
    {
        mana -= 25;
        HP = 100;
        manaUpdatedEvent.Invoke(mana);
        hpUpdatedEvent.Invoke(HP);
    }

    void movement()
    {
        if (characterController.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = ((Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward));

        characterController.Move(moveDir * speed * Time.deltaTime);

        jump();

        playerVel.y -= gravity * Time.deltaTime;
        characterController.Move(playerVel * Time.deltaTime);

        shootTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            shoot();
        }
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }

    void sprint()
    {
        if (Input.GetButton("Sprint") && playerVel != Vector3.zero && !isSprinting)
        {
            speed *= sprintMod;
            isSprinting= true;
            sprintChangeEvent.Invoke(isSprinting);
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting= false;
            sprintChangeEvent.Invoke(isSprinting);
        }
        
    }

    void shoot()
    {
        shootTimer = 0;
        
    }

    IEnumerator ManaRegen(int regenAmount, float secondsToRegen)
    {
        mana += regenAmount;
        mana = Mathf.Clamp(mana, 0, manaOrig);
        manaUpdatedEvent.Invoke(mana);
        yield return new WaitForSeconds(secondsToRegen);
        StartCoroutine(ManaRegen(regenAmount, secondsToRegen));
    }

    public void GetGunStats(GunStats gun)
    {
        //gunList.Add(gun);
        //gunListPos = gunList.Count - 1;
        //ChangeGun(gun);
    }

    /*void SelectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPos < gunList.Count -1)
        {
            gunListPos++;
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
        {
            gunListPos--;
        }
    }*/

    /*void ChangeGun()
    {
        //shootDamage = gun.shootDamage;
        //shootDist = gun.shootDist;
        //shootRate = gun.shootRate;

        //gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        //gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;
    }*/

    /*void Reload()
    {
        if (Input.GetButtonDown("Reload") && gunList.Count > 0)
        {
            gunList[gunListPos].ammoCur = gunList[gunListPos].ammoMax;
            updatePlayerUI();
        }
    }*/
}
