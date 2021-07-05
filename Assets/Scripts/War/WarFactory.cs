using UnityEngine;

[CreateAssetMenu]
public class WarFactory : GameObjectFactory {

	[SerializeField]
	Explosion explosionPrefab = default;

	[SerializeField]
	Shell shellPrefab = default;

	[SerializeField]
	Arrow arrowPrefab = default;

	public Explosion Explosion => Get(explosionPrefab);

	public Shell Shell => Get(shellPrefab);

	public Arrow Arrow=> Get(arrowPrefab);

	T Get<T> (T prefab) where T : WarEntity {
		T instance = CreateGameObjectInstance(prefab);
		instance.OriginFactory = this;
		return instance;
	}

	public void Reclaim (WarEntity entity) {
		Debug.Assert(entity.OriginFactory == this, "Wrong factory reclaimed!");
		Destroy(entity.gameObject);
	}
}