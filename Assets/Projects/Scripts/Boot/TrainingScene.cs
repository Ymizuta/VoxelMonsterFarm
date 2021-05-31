using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Voxel.SceneManagement;
using Voxel.UI;
using Voxel.Common;

namespace Voxel.Training
{
	public class TrainingScene : SceneBase
	{
		[SerializeField] private ParamResultUI resultUi;

		private TrainingManagerBase manager;

		public override void Initialize(SceneData data = null)
		{
			base.Initialize(data);
			var tData = data as TrainingSceneData;
			StartCoroutine(LoadAdditiveSceneAsync(tData.TrainingType, 
				() => 
				{
					FadeManager.Instance.PlayFadeIn(() => 
					{
						// �t�F�[�h�������Ă�����s
						StartCoroutine(Run(tData.TrainingType, TrainingResult.Success));
					});
				}));
		}

		/// <summary>
		/// �A�j���[�V�����V�[�������s
		/// </summary>
		/// <returns></returns>
		public IEnumerator Run(TrainingType type, TrainingResult result)
		{
			// ���K�V�[�����擾�E���s
			manager.Initialize(SaveDataManager.SaveData.CurrentMonster.MonsterModelId);
			yield return manager.Run(result);

			// ���䂪�߂����猋�ʔ��\
			resultUi.ShowResult();
			yield return new WaitForSeconds(1f);
			resultUi.HideResult();

			// �p�����[�^�X�V
			var monsterparam = SaveDataManager.SaveData.CurrentMonster;
			var addVal = GetAddMonsterParam(GetParamType(type), new MonsterCalculator().CalcCondition(monsterparam.Fatigue));
			resultUi.ShowParam(monsterparam, addVal);
			UpdateMonsterParam(monsterparam, addVal);
			Comment.Instance.Show(GetResultComment(addVal));

			// ���͂�҂��Ėq��ɖ߂�
			yield return PlayerInput();
			CalendarManager.Instance.NextWeek();
			FadeManager.Instance.PlayFadeOut(() => SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Farm));
		}

		/// <summary>
		/// �A�j���[�V�����p�̃V�[����񓯊����[�h
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		private IEnumerator LoadAdditiveSceneAsync(TrainingType type, Action action = null)
		{
			var asyncOperation = SceneManager.LoadSceneAsync(type.ToString(), LoadSceneMode.Additive);
			while (!asyncOperation.isDone)
			{
				yield return null;
			}
			manager = SceneManager.GetSceneAt(1).GetRootGameObjects()[0].GetComponent<TrainingManagerBase>();
			action?.Invoke();
		}

		/// <summary>
		/// ���ʃR�����g���擾
		/// </summary>
		/// <param name="addVal"></param>
		/// <returns></returns>
		private string GetResultComment(int[] addVal)
		{
			var calc = new StringCalculator();
			string comment = "";
			for (int i = 0; i < addVal.Length; i++)
			{
				if (addVal[i] != 0)
				{
					comment += $"{calc.GetParamName((ParamType)i)}��{addVal[i]}�オ��܂���\n";
				}
			}
			return comment;
		}

		/// <summary>
		/// �ύX�p�����[�^���擾
		/// </summary>
		/// <returns></returns>
		private int[] GetAddMonsterParam(ParamType type, MonsterCondition condition)
		{
			var addVal = new int[6];
			var val = UnityEngine.Random.Range(2, 5);
			if (condition == MonsterCondition.Tired || condition == MonsterCondition.VeryTired)
			{
				// ��J�����܂��Ă���Ə㏸�l������
				val = Mathf.Clamp((int)(val * 0.5f), 0, val);
			}
			addVal[(int)type] = val;
			return addVal;
		}

		/// <summary>
		/// �����X�^�[�̃p�������X�V
		/// </summary>
		private void UpdateMonsterParam(MonsterParam param, int[] addVal)
		{
			param.Hp += addVal[(int)ParamType.Hp];
			param.Power += addVal[(int)ParamType.Power];
			param.Guts += addVal[(int)ParamType.Guts];
			param.Hit += addVal[(int)ParamType.Hit];
			param.Speed += addVal[(int)ParamType.Speed];
			param.Deffence += addVal[(int)ParamType.Deffence];
			param.Fatigue += Common.GameSettingProvidor.Instance.TrainingFatigue;
		}

		private IEnumerator PlayerInput()
		{
			while (true)
			{
				yield return null;
				if (Input.GetKeyDown(KeyCode.Space)) break;
			}
		}

		private ParamType GetParamType(TrainingType type )
		{
			switch (type)
			{
				case TrainingType.AvoidRock:
					return ParamType.Speed;
				case TrainingType.Domino:
					return ParamType.Power;
				case TrainingType.LogShock:
					return ParamType.Deffence;
				case TrainingType.Running:
					return ParamType.Hp;
				case TrainingType.Shooting:
					return ParamType.Hit;
				case TrainingType.Studying:
					return ParamType.Guts;
				default:
					Debug.LogWarning($"�z�肵�Ă��Ȃ��^�C�v���I������܂��� = {type}");
					return ParamType.Hp;
			}
		}
	}
}
