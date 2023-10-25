using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;

namespace Artemis {

	public class ArtemisController : BaseSpaceShipController
	{

		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 1.0f;
			float targetOrient = spaceship.Orientation + 90.0f;
			bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
			return new InputData(thrust, targetOrient, needShoot, false, false);
		}

		public InputData GoToTarget(SpaceShipView spaceShip, GameData gameData, Transform targetPos)
		{
			float thrust = 1.0f;
			float targetOrient = spaceShip.Orientation + AimingHelpers.ComputeSteeringOrient(spaceShip, new Vector2(targetPos.position.x,targetPos.position.y));
			return new InputData(thrust, targetOrient, false, false, false);
		}
	}

}
