using UnityEngine;
using UnityEngine.InputSystem;

public class TestSystem : GameSystem {
	private EntityManager _em;
	public TestSystem(Game game, EntityManager em) : base(game, true) {
		_em = em;
	}

	public override void Update() {
		
	}
}