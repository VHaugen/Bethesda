using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    Fire,
    Ice,
    Lightning,
    Poison,
    None
}

[RequireComponent(typeof(AudioSource))]
public class HammerWeapon : MonoBehaviour
{
    public int overHeadCost = 20;
    //public int manaCharge = 5;
    Collider hitbox;
    AudioSource audioSource;
    Animator anim;
    Collider overHeadHitBox;
	AfterImages afterImages;
	Transform impact;
	Vector3 impactOffset;
    public GameObject playerObject;
    PlayerMovement mana;
    GameObject player;

    void Awake()
    {
        hitbox = transform.Find("Head").GetComponent<Collider>();

        //impact = transform.Find("Impact");
        //impactOffset = impact.localPosition;
        player = GameObject.FindGameObjectWithTag("Player");
        mana = player.GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        overHeadHitBox = transform.Find("AOE").GetComponent<Collider>();
		afterImages = GetComponentInChildren<AfterImages>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlaceholderWeaponAttack"))
        {
            print("Attacku!");
            anim.SetTrigger("AttackTransitionBasic");
			//mana.AddMana(manaCharge);
			afterImages.Show();
		}
        if (Input.GetKeyDown(KeyCode.E) && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlaceholderOverheadAttack") && mana.manaSlider.value >= 20 || Input.GetButtonDown("B Button") && !anim.GetCurrentAnimatorStateInfo(1).IsName("PlaceholderOverheadAttack") && mana.manaSlider.value >= 20)
        {
            print("ROADO ROLLA DA");
            anim.SetTrigger("AttackTransitionOverhead");
            mana.UseMana(overHeadCost);
        }

    }


	public void ShowAfterImages()
	{
		afterImages.Show();
	}

	public void ShowImpact(int show)
	{
		//if (show == 0)
		//{
		//	impact.gameObject.SetActive(false);
		//	impact.parent = transform;
		//	impact.localPosition = impactOffset;
		//	impact.localRotation = Quaternion.identity;
		//}
		//else
		//{
		//	impact.gameObject.SetActive(true);
		//	impact.parent = null;
		//	impact.rotation = Quaternion.Euler(90, 90, Random.Range(0f, 360f));;
		//}
	}

    public void EnableHitbox(int enable)
    {
        hitbox.enabled = enable == 0 ? false : true;
    }

    public void EnableAoeHitbox(int enable)
    {
        overHeadHitBox.enabled = enable == 0 ? false : true;
    }

    public void EnableSlowDown(int enableSlowness)
    {
        playerObject.GetComponent<PlayerMovement>().maxSpeed = enableSlowness == 1 ? 1.0f : 10.0f;
    }

    public void PlaySound(Object audioClip)
    {
        audioSource.PlayOneShot((AudioClip)audioClip);
    }

    public void ScreenShake()
    {
        CameraEffects.Get.Shake(0.25f, 0.4f);
    }
}