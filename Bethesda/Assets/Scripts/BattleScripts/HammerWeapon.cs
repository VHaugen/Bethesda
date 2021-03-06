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
    [SerializeField] float normalAttackAfterImageDuration;
    [SerializeField] float smashAttackAfterImageDuration;

    [HideInInspector] public Collider hitbox;
    [HideInInspector] public Collider overHeadHitBox;
    public AudioSource audioSource;
    public AudioClip impct;
    [HideInInspector] public AfterImages afterImages;
    Transform impact;
    Vector3 impactOffset;
    PlayerMovement playerMovement;
    Animator playerAnimator;
	MeshRenderer meshRenderer;

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
		meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Attack") && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlaceholderWeaponAttack"))
        {
            print("Attacku!");
            playerAnimator.SetTrigger("AttackTransition");
            afterImages.Show();
            afterImages.duration = normalAttackAfterImageDuration;
        }
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("B Button")) && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Heavy Attack"))
        {
            if (playerMovement.currentMana >= overHeadCost)
            {
                print("ROADO ROLLA DA");
                playerAnimator.SetTrigger("AttackTransitionHeavy");
                playerMovement.UseMana(overHeadCost);
                afterImages.duration = smashAttackAfterImageDuration;
            }
        }

        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Y Button")) && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Swing Attack")  && playerMovement.currentMana >= swingCost)
        {
            print("SWINGAROOO");
            playerAnimator.SetTrigger("AttackTransitionSwing");
            playerMovement.UseMana(swingCost);
        }


    }

	public void SetMaterial(Material newMaterial)
	{
		StartCoroutine(ChangeMaterialCoroutine(newMaterial));
	}

	IEnumerator ChangeMaterialCoroutine(Material newMaterial)
	{
		Material oldMaterial = meshRenderer.material;
		for (int i = 0; i < 10; i++)
		{
			meshRenderer.material = newMaterial;
			yield return new WaitForSeconds(0.075f);
			meshRenderer.material = oldMaterial;
			yield return new WaitForSeconds(0.0375f);
		}
		meshRenderer.material = newMaterial;
	}

    public void ShowAfterImages()
    {
        afterImages.Show();
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