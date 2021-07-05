using UnityEngine;

public class ArcherTower : Tower {

	[SerializeField, Range(0.5f, 5f)]
	float shotsPerSecond = 2.5f;

	[SerializeField, Range(1f, 100f)]
	float arrowDamage = 10f;

	[SerializeField]
	Transform archer = default;

	public override TowerType TowerType => TowerType.Archer;

	float launchSpeed;

	float launchProgress;

	void Awake() {
		OnValidate();
	}

	void OnValidate() {
		float x = targetingRange + 0.25001f;
		float y = -archer.position.y;
		launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
	}

	public override void GameUpdate() {
		launchProgress += shotsPerSecond * Time.deltaTime;
		while (launchProgress >= 1f) {
			if (AcquireTarget(out TargetPoint target)) {
				Launch(target);
				launchProgress -= 1f;
			}
			else {
				launchProgress = 0.999f;
			}
		}
	}

	public void Launch(TargetPoint target) {
		Vector3 launchPoint = archer.position;
		Vector3 targetPoint = target.Position;
		targetPoint.y = 0f;

		Vector2 dir;
		dir.x = targetPoint.x - launchPoint.x;
		dir.y = targetPoint.z - launchPoint.z;
		float x = dir.magnitude;
		float y = -launchPoint.y;
		dir /= x;

		float g = 9.81f;
		float s = launchSpeed;
		float s2 = s * s;

		float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
		Debug.Assert(r >= 0f, "Launch velocity insufficient for range!");
		float tanTheta = (s2 - Mathf.Sqrt(r)) / (g * x); //- for lower angle
		float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
		float sinTheta = cosTheta * tanTheta;

		archer.localRotation =
			Quaternion.LookRotation(new Vector3(dir.x, tanTheta, dir.y));

		Game.SpawnArrow().Initialize(
			launchPoint,
			new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y),
			arrowDamage, target
		);
	}
}