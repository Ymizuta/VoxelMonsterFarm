using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel.SceneManagement;

namespace Voxel.Training
{
	public enum TrainingType
	{
		Running = 0, // ���荞��
		Domino, // �h�~�m�|��
		Shooting, // �˓I
		Studying, // �׋�
		AvoidRock, // ���Δ���
		LogShock, // �ۑ���
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
