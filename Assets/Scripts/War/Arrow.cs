using UnityEngine;

public class Arrow : WarEntity {

	Vector3 launchPoint, launchVelocity;
	TargetPoint target;

	float age, damage;

	public void Initialize(
		Vector3 launchPoint, Vector3 launchVelocity,
		float damage, TargetPoint target
	) {
		this.launchPoint = launchPoint;
		this.target = target;
		this.launchVelocity = launchVelocity;
		this.damage = damage;
	}

	public override bool GameUpdate() {
		age += Time.deltaTime;
		Vector3 p = launchPoint + launchVelocity * age;
		p.y -= 0.5f * 9.81f * age * age;

		if (p.y <= 0f) {
			target.Enemy.ApplyDamage(damage);
			OriginFactory.Reclaim(this);
			return false;
		}

		transform.localPosition = p;
		Vector3 d = launchVelocity;
		d.y -= 9.81f * age;
		transform.localRotation = Quaternion.LookRotation(d);

		target.Enemy.ApplyDamage(damage);
		return true;
	}
}