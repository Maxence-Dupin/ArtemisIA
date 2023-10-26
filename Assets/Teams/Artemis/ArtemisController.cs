using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using DoNotModify;

namespace Artemis {

	public class ArtemisController : BaseSpaceShipController
	{
		[SerializeField] private BehaviorTree _behaviorTree;
		
		private SpaceShip _aiSpaceShip;
		private SpaceShipView _enemySpaceShip;

		private Vector2 _closestWaypointPosition;

		private bool _hasMoreEnergy;

		public bool GoingToWaypoint;
		public bool UseShockWave;

		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
			_aiSpaceShip = GameManager.Instance.GetSpaceShipForController(this);
			_enemySpaceShip = data.GetSpaceShipForOwner(1 - spaceship.Owner);
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			_behaviorTree.SetVariableValue("DistanceE", DistanceWithEnemySpaceship());

			_closestWaypointPosition = GetClosestUnownedWaypointPosition();
			_behaviorTree.SetVariableValue("ClosestWP", _closestWaypointPosition);
			_behaviorTree.SetVariableValue("DistanceWP", DistanceWithClosestWaypoint());
			
			_behaviorTree.SetVariableValue("Mine", CanShotMine());
			
			_behaviorTree.SetVariableValue("EnergieJ", _aiSpaceShip.Energy);
			_behaviorTree.SetVariableValue("EnergieE", _enemySpaceShip.Energy);
			
			_behaviorTree.SetVariableValue("DistanceTir", DistanceWithClosestEnemyShot());

			if (GoingToWaypoint)
			{
				return GoToTarget(_aiSpaceShip.view, _closestWaypointPosition);
			}
			
			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float targetOrient = spaceship.Orientation;
			float thrust = 1f;
			bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);

			var inputData = new InputData(thrust, targetOrient, needShoot, false, UseShockWave);
			
			UseShockWave = false;
			
			return inputData;
		}

		public InputData GoToTarget(SpaceShipView spaceShip, Vector2 targetPos)
		{
			float thrust = 1.0f;
			float targetOrient = AimingHelpers.ComputeSteeringOrient(spaceShip, targetPos);
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
		
		public float DistanceWithEnemySpaceship() => Vector2.Distance(_enemySpaceShip.Position, _aiSpaceShip.Position);
		
		public float DistanceWithClosestWaypoint() => Vector2.Distance(_closestWaypointPosition, _aiSpaceShip.Position);

		public bool CanShotMine()
		{
			var gameData = GameManager.Instance.GetGameData();

			for (int i = 0; i < gameData.Mines.Count; i++)
			{
				if (AimingHelpers.CanHit(_aiSpaceShip.view, gameData.Mines[i].Position, 0.15f))
				{
					return true;
				}
			}

			return false;
		}

		public Vector2 GetClosestUnownedWaypointPosition()
		{
			var gameData = GameManager.Instance.GetGameData();

			WayPointView waypoint = gameData.WayPoints[0];
			var smallestDistance = float.MaxValue;

			for (var i = 0; i < gameData.WayPoints.Count; i++)
			{
				var distance = Vector2.Distance(gameData.WayPoints[i].Position, _aiSpaceShip.Position);

				if (!(distance < smallestDistance) || gameData.WayPoints[i].Owner == _aiSpaceShip.Owner) continue;

				waypoint = gameData.WayPoints[i];
				smallestDistance = distance;
			}

			return waypoint.Position;
		}
		
		public float DistanceWithClosestEnemyShot()
		{
			var gameData = GameManager.Instance.GetGameData();
			
			var smallestDistance = float.MaxValue;

			for (var i = 0; i < gameData.Bullets.Count; i++)
			{
				var distance = Vector2.Distance(gameData.Bullets[i].Position, _aiSpaceShip.Position);

				if (distance > smallestDistance) continue;
				
				smallestDistance = distance;
			}

			return smallestDistance;
		}
	}
}
