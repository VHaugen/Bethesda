using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SquashEffect : MonoBehaviour
{
	[SerializeField]
	float squashDuration = 0.05f;

	[SerializeField]
	float remainSquashedDuration = 0.5f;

	[SerializeField]
	float recoverDuration = 0.2f;

	[SerializeField]
	[Range(0, 0.5f)]
	float squashedHeight = 0.01f;

	[SerializeField]
	float flatSize = 2f;

	[HideInInspector]
	public Vector3 baseScale;
	[HideInInspector]
	public Vector3 basePosition;


	public bool inSquash
	{
		get
		{
			return state != State.Still;
		}
	}


	public bool isFlat
	{
		get
		{
			return state == State.SquashedStill;
		}
	}

	public Vector3 squashScale { get; private set; }
	Vector3 offset;

	float timer;
	bool animatorWasEnabled;
	bool recover;

	Animator animator;
	MeshFilter meshFilter;
	AudioSource audio;

	enum State
	{
		Still,
		Squash,
		SquashedStill,
		Recover,
	}
	State state;

	// Use this for initialization
	void Awake()
	{
		baseScale = transform.localScale;
		basePosition = transform.position;
		squashScale = Vector3.one;
		animator = GetComponent<Animator>();
		meshFilter = GetComponent<MeshFilter>();
		audio = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		switch (state)
		{
			case State.Squash:
				{
					float heightChange = 1 - squashedHeight;
					float height = squashScale.y;
					height -= heightChange * Time.deltaTime / squashDuration;

					if (height <= squashedHeight)
					{
						height = squashedHeight;
						state = State.SquashedStill;
						timer = 0f;
					}

					SetScale(height);

					break;
				}
			case State.SquashedStill:
				if (timer > remainSquashedDuration && recover)
				{
					timer = 0f;
					state = State.Recover;
				}

				timer += Time.deltaTime;

				break;
			case State.Recover:
				{
					SetScale(Easings.EaseOutElastic(squashedHeight, 1, timer / recoverDuration));
					timer += Time.deltaTime;
					if (timer >= recoverDuration)
					{
						SetScale(1f);
						state = State.Still;
						timer = 0f;
						animator.enabled = animatorWasEnabled;
						transform.position = basePosition;
					}

					break;
				}

		}

		if (state != State.Still)
		{
			transform.localScale = Vector3.Scale(baseScale, squashScale);
			Vector3 pivotPosition = basePosition - new Vector3(0, meshFilter.mesh.bounds.extents.y, 0);
			Vector3 pivotOffset = basePosition - pivotPosition;
			pivotPosition = basePosition - new Vector3(0, meshFilter.mesh.bounds.extents.y * baseScale.y, 0);
			transform.position = Vector3.Scale(pivotOffset, transform.localScale) + pivotPosition;
		}
	}

	void SetScale(float height)
	{
		float otherSize = Mathf.Lerp(1, flatSize, 1 - height / (1 - squashedHeight));
		squashScale = new Vector3(otherSize, height, otherSize);
	}

	public void DoSquash(bool recover = true)
	{
		state = State.Squash;
		animatorWasEnabled = animator.enabled;
		animator.enabled = false;
		this.recover = recover;
		baseScale = transform.localScale;
		basePosition = transform.position;
		if (audio.clip != null)
		{
			audio.Play();
		}
	}
}
