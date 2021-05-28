using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.SceneManagement;

namespace Voxel.Training
{
	public enum TrainingType
	{
		Running = 0, // ‘–‚è‚İ
		Domino, // ƒhƒ~ƒm“|‚µ
		Shooting, // Ë“I
		Studying, // •×‹­
		AvoidRock, // ‹Î”ğ‚¯
		LogShock, // ŠÛ‘¾ó‚¯
	}

	public class TrainingSceneData : SceneData
	{
		public TrainingType TrainingType { get; private set; }

		public TrainingSceneData(TrainingType trainingType)
		{
			this.TrainingType = trainingType;
		}
	}
}
