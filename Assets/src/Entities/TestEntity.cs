using UnityEngine;
using Reflex.Attributes;

public class TestEntity : Entity {
	[Inject] string TestString;

	public override void UpdateEntity() {
		Debug.Log(TestString);
	}
}