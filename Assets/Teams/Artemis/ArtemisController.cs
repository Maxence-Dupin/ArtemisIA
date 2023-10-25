using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using DoNotModify;

namespace Artemis {

	public class ArtemisController : BaseSpaceShipController
	{
		[SerializeField] private int _aiSpaceshipInt;
		[SerializeField] private int _enemySpaceshipInt;

		private SpaceShipView _enemySpaceship;
		private SpaceShipView _aiSpaceship;

		private WayPointView _closestWaypoint;
		private float _distanceWithSpaceship;

		private float _lastEnemyShotTime;
		private float _dangerScore;

		private bool _hasMoreEnergy;

		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
			_enemySpaceship = data.GetSpaceShipForOwner(_enemySpaceshipInt);
			_aiSpaceship = data.GetSpaceShipForOwner(_aiSpaceshipInt);

			_distanceWithSpaceship = DistanceWithEnemySpaceship();
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			_distanceWithSpaceship = DistanceWithEnemySpaceship();
			_closestWaypoint = GetClosestUnownedWaypoint();

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

		public bool AsteroideOnTheWay(SpaceShipView spaceShip, GameData gameData, Transform targetPos)
		{
			bool isClear = true;
			RaycastHit2D hit2D = Physics2D.Raycast(spaceShip.Position, targetPos.position);
			if (hit2D.collider.CompareTag("Asteroid"))
			{
				isClear = false;
			}
			return isClear;
		}
		
		public float DistanceWithEnemySpaceship() => Vector2.Distance(_enemySpaceship.Position, _aiSpaceship.Position);

		public WayPointView GetClosestUnownedWaypoint()
		{
			var gameData = GameManager.Instance.GetGameData();

			WayPointView waypoint = null;
			var smallestDistance = float.MaxValue;

			for (var i = 0; i < gameData.WayPoints.Count; i++)
			{
				var distance = Vector2.Distance(gameData.WayPoints[i].Position, _aiSpaceship.Position);

				if (!(distance < smallestDistance) || gameData.WayPoints[i].Owner == _aiSpaceshipInt) continue;

				waypoint = gameData.WayPoints[i];
				smallestDistance = distance;
			}

			return waypoint;
		}
	}
}
