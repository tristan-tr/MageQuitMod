using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace MageQuitMod
{
    public static class Helper
    {
		public static Vector2 GetWorldToScreenPoint(Vector3 position)
		{
			var cam = Camera.main; //Get camera
			var w2s = cam.WorldToScreenPoint(position); //Translate position to screen
			if (w2s.z < 0) //Behind screen?
			{
				Debug.Log("MOD GetWorldToScreen position is behind screen");
				return Vector2.zero; //Skip
			}

			return new Vector2(w2s.x, Screen.height - w2s.y);
		}


		public static WizardController GetLocalWizardController()
        {
			return GetFieldValue<Transform>(Globals.wizard_cursor, "wizard").GetComponent<WizardController>();
		}

		/// <summary>
		/// This is way too fucking hard, why, why god why
		/// </summary>
		/// <returns></returns>
		public static Player GetLocalPlayer()
        {
            WizardController wizardController = GetLocalWizardController();

			// Get player that corresponds to the wizard
			return PlayerManager.players.Values.Where(player => player.wizard.GetComponent<WizardController>() == wizardController).First();
		}

		// im sorry, not my code
		public static int GetNumberOfAlivePlayers()
        {
			List<global::Player> source = PlayerManager.players.Values.Where(delegate (global::Player x)
			{
				if (x.wizard != null)
				{
					WizardStatus component = x.wizard.GetComponent<WizardStatus>();
					return component != null && !component.isDead;
				}
				return false;
			}).ToList<global::Player>();
			int num = (from x in source
					   where x.teamColor == TeamColor.None
					   select x).ToList<global::Player>().Count<global::Player>();
			int num2 = source.Any((global::Player x) => x.teamColor == TeamColor.Red) ? 1 : 0;
			int num3 = source.Any((global::Player x) => x.teamColor == TeamColor.Blue) ? 1 : 0;
			int num4 = source.Any((global::Player x) => x.teamColor == TeamColor.Yellow) ? 1 : 0;

			return num + num2 + num3 + num4;
		}

		private readonly static BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		private static FieldInfo GetField(object obj, string name)
        {
            // Set the flags so that private and public fields from instances will be found
			return obj.GetType().GetField(name, bindingFlags);
		}

		/// <summary>
		/// Gets the field value of a private field using reflection
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="name">Name of the private field</param>
		/// <returns></returns>
		public static T GetFieldValue<T>(this object obj, string name)
		{
			return (T)GetField(obj, name)?.GetValue(obj);
		}
		public static void SetFieldValue<T>(this object obj, string name, T value)
        {
			GetField(obj, name)?.SetValue(obj, value);
		}

		/// <summary>
		/// Gets the closest player to the aiming player's aim. This is used to predict a player's target
		/// </summary>
		/// <returns>Closest player to aim</returns>
		public static Player GetPlayerAimedAt(Player aimingPlayer, Quaternion aimingPlayerRotation)
		{
			float smallestAngle = float.MaxValue;
			Player playerAimedAt = null;

			foreach (Player currentPlayer in PlayerManager.players.Values) 
			{ 

				// Make sure we don't select our own aiming player (which of course is the closest)
				if (currentPlayer == aimingPlayer)
					continue;


				// Get our angle to this player
				Vector3? nullableWizardVector = currentPlayer?.wizard?.transform.position - aimingPlayer?.wizard?.transform.position;

				if (!nullableWizardVector.HasValue)
					continue;

				Vector3 wizardVector = nullableWizardVector.Value;

				wizardVector.y = 0;


				Quaternion wizardRotation = Quaternion.LookRotation(wizardVector.normalized, Vector3.up);
				float angleToPlayer = Quaternion.Angle(aimingPlayerRotation, wizardRotation);

				// Store smallest angle and player
				if (angleToPlayer < smallestAngle)
				{
					smallestAngle = angleToPlayer;
					playerAimedAt = currentPlayer;
				}
			}

			return playerAimedAt;
		}

		public static Quaternion GetAIAim(Spell spell, Vector3 position, Transform target, float curve)
        {
			// Calculate aim (no curve) to predicted target position
			Vector3? predictedTargetPosition = Helper.GetPredictedPosition(spell, true, target, position);

			Vector3 targetVector;
			if (predictedTargetPosition.HasValue)
			{
				targetVector = predictedTargetPosition.Value - position;
			}
			else
			{
				Debug.LogError("MOD Couldn't predict target position!");
				targetVector = target.position - position;
			}

			targetVector.y = 0f;

			return Quaternion.LookRotation(targetVector.normalized, Vector3.up);
		}


		public static Vector3? GetPredictedPosition(Spell spell, bool includeWindUp, Transform targetTransform, Vector3 casterPos)
		{
			WizardController component = targetTransform.GetComponent<WizardController>();
			if (!(component != null) || component.isClone)
			{
				return null;
			}
			PositionTracker component2 = targetTransform.GetComponent<PositionTracker>();
			if (component2 == null)
			{
				return null;
			}

			float magnitude = (targetTransform.position - casterPos).magnitude;
			float num = (spell.initialVelocity == 0f) ? 0f : (magnitude / spell.initialVelocity);
			if (includeWindUp)
			{
				num += spell.windUp;
			}

			return new Vector3?(targetTransform.position + component2.PredictedMovementVector() * num);
		}
	}
}
