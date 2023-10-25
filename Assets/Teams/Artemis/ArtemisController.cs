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
		
		[SerializeField] private SpaceShip _aiSpaceShip;
		[SerializeField] private SpaceShip _enemySpaceShip;

		private Vector2 _closestWaypointPosition;

		private bool _hasMoreEnergy;

		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
			_behaviorTree.SetVariableValue("DistanceE", DistanceWithEnemySpaceship());
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			_behaviorTree.SetVariableValue("DistanceE", DistanceWithEnemySpaceship());

			_closestWaypointPosition = GetClosestUnownedWaypointTransform();
			_behaviorTree.SetVariableValue("ClosestWP", _closestWaypointPosition);
			_behaviorTree.SetVariableValue("DistanceWP", DistanceWithClosestWaypoint());
			
			_behaviorTree.SetVariableValue("Mine", CanShotMine());
			
			_behaviorTree.SetVariableValue("EnergyJ", _aiSpaceShip.Energy);
			_behaviorTree.SetVariableValue("EnergyE", _enemySpaceShip.Energy);
			
			float thrust;
			float targetOrient = spaceship.Orientation;

			if (!_behaviorTree.FindTaskWithName("GoToPosition").Disabled)
			{
				thrust = 1f;
				targetOrient = AimingHelpers.ComputeSteeringOrient(_aiSpaceShip.view, _closestWaypointPosition);
			}
			else
			{
				thrust = 0f;
			}
			
			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
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
		
		public float DistanceWithEnemySpaceship() => Vector2.Distance(_enemySpaceShip.Position, _aiSpaceShip.Position);
		
		public float DistanceWithClosestWaypoint() => Vector2.Distance(_closestWaypointPosition, _aiSpaceShip.Position);

		public bool CanShotMine()
		{
			var gameData = GameManager.Instance.GetGameData();

			for (int i = 0; i < gameData.Mines.Count; i++)
			{
				if (AimingHelpers.CanHit(_aiSpaceShip.view, gameData.Mines[i].Position, 0.2f))
				{
					return true;
				}
			}

			return false;
		}

		public Vector2 GetClosestUnownedWaypointTransform()
		{
			var gameData = GameManager.Instance.GetGameData();

			WayPointView waypoint = null;
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
	}
}
