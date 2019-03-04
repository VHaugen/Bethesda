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
    public int swingCost = 10;
    public int manaCharge = 5;
    int numRays = 7;
    
    [HideInInspector] public Collider hitbox;
    [HideInInspector] public Collider overHeadHitBox;
    public AudioSource audioSource;
    public AudioClip impct;
    [HideInInspector] public AfterImages afterImages;
    Transform impact;
    Vector3 impactOffset;
    PlayerMovement playerMovement;
    Animator playerAnimator;
    public GameObject rayCastPoint;
    public DamageType damageType = DamageType.Hit;
    public Element currentElement = Element.None;
    public float swingDamage;
    public float knockbackStrength = 2.0f;
    


    void Awake()
    {
        hitbox = GetComponentInChildren<Collider>();

        //impact = transform.Find("Impact");
        //impactOffset = impact.localPosition;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerAnimator = playerMovement.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        overHeadHitBox = transform.Find("AOE").GetComponent<Collider>();
        afterImages = GetComponent<AfterImages>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Attack") && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlaceholderWeaponAttack"))
        {
            print("Attacku!");
            playerAnimator.SetTrigger("AttackTransition");
            afterImages.Show();
        }
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("B Button")) && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Heavy Attack") /*&& mana.manaSlider.value >= overHeadCost*/)
        {
            print("ROADO ROLLA DA");
            playerAnimator.SetTrigger("AttackTransitionHeavy");
            playerMovement.UseMana(overHeadCost);
        }

        if ((Input.GetKeyDown(KeyCode.Q)) /*&& mana.manaSlider.value >= swingCost*/)
        {
            print("SWINGAROOO");
            playerAnimator.SetTrigger("AttackTransitionSwing");
            playerMovement.UseMana(swingCost);
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

    /*public void SwingHit()
    {
        
        for (int i = 0; i < numRays; i++)
        {
            
            Vector3 fwd = rayCastPoint.transform.forward;
            float Afwd = Mathf.Atan2(fwd.z, fwd.x);
            float angle = Afwd + (-40 + i * 80 / numRays) * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

            direction.y = 0;

            Ray r = new Ray(rayCastPoint.transform.position, direction);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, 10f))
            {
                    print("hej1");

                IAttackable thingICanKill = hit.collider.GetComponent<IAttackable>();
                if (thingICanKill != null)
                {
                    print("hej2");
                    Vector3 knockback = Vector3.zero;
                    knockback = hit.transform.position - playerMovement.transform.position;
                    knockback.y = 1f;
                    knockback.Normalize();
                    knockback *= knockbackStrength;
                    if (!audioSource.isPlaying)
                        audioSource.PlayOneShot(impct);
                    thingICanKill.TakeDamage(new DamageParams(swingDamage, currentElement, damageType, knockback));
                }
            }
        }
    }*/



    public void PlaySound(Object audioClip)
    {
        audioSource.PlayOneShot((AudioClip)audioClip);
    }
}