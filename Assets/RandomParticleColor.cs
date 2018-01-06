using UnityEngine;

public class RandomParticleColor : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem m_particleSystem;

	[SerializeField]
	private float colorBoost;

	[SerializeField]
	private int numberOfParticlesToEmitPerSecond;

	[SerializeField]
	private bool controlEmission;

	private Vector3 lastPosition;

	void Awake()
	{
		if (controlEmission)
		{
			var emission = m_particleSystem.emission;
			emission.enabled = false;
		}
		lastPosition = transform.position;
	}

	void Update()
	{
		var main = m_particleSystem.main;
		if (controlEmission)
		{
			var speed = main.startSpeed.Evaluate(0f);
			int numberOfParticlesToEmit = Mathf.CeilToInt(Time.deltaTime * numberOfParticlesToEmitPerSecond);
			for (int i = 0; i < numberOfParticlesToEmit; i++)
			{
				var emitParams = new ParticleSystem.EmitParams();
				emitParams.startColor = GetRandomColor();
				emitParams.velocity = transform.forward * speed;
				emitParams.position = GetInterpolatedPosition(i, numberOfParticlesToEmit);
				emitParams.startLifetime = 3;
				m_particleSystem.Emit(emitParams, 1);
			}
		}
		else
		{
			main.startColor = GetRandomColor();
		}
		lastPosition = transform.position;
	}

	private Vector3 GetInterpolatedPosition(int index, int numberOfParticlesToEmit)
	{
		var range = index / (float)(numberOfParticlesToEmit - 1);
		return Vector3.Lerp(lastPosition, transform.position, range);
	}

	private Color GetRandomColor()
	{
		var rgbSelector = Random.value;
		const float lowerRange = 0.3f;
		const float upperRange = 0.7f;
		float r, g, b;
		if (rgbSelector < 0.33f)
		{
			r = Random.Range(upperRange, 1f);
			g = Random.Range(0f, lowerRange);
			b = Random.Range(0f, lowerRange);
		}
		else if (rgbSelector < 0.66f)
		{
			r = Random.Range(0f, lowerRange);
			g = Random.Range(upperRange, 1f);
			b = Random.Range(0f, lowerRange);
		}
		else
		{
			r = Random.Range(0f, lowerRange);
			g = Random.Range(0f, lowerRange);
			b = Random.Range(upperRange, 1f);
		}

		var colorVector = new Vector3(r, g, b);
//		var colorVector = new Vector3(Random.value, Random.value, Random.value);
		colorVector.Normalize();
		colorVector *= colorBoost;
		return new Color(colorVector.x, colorVector.y, colorVector.z, 1f);
	}
}
