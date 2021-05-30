using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Voxel.SceneManagement;
using Voxel.UI;

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
						StartCoroutine(Run(TrainingResult.Success));
					});
				}));
		}

		/// <summary>
		/// �A�j���[�V�����V�[�������s
		/// </summary>
		/// <returns></returns>
		public IEnumerator Run(TrainingResult result)
		{
			// ���K�V�[�����擾�E���s
			manager.Initialize((int)MonsterModel.Dog);
			yield return manager.Run(result);

			// ���䂪�߂����猋�ʔ��\
			resultUi.ShowResult();
			yield return new WaitForSeconds(1f);
			resultUi.HideResult();

			// �p�����[�^�X�V
			var addVal = GetAddMonsterParam();
			var monsterparam = SaveDataManager.SaveData.CurrentMonster;
			resultUi.ShowParam(monsterparam, addVal);
			UpdateMonsterParam(monsterparam, addVal);

			// ���͂�҂��Ėq��ɖ߂�
			yield return new WaitForSeconds(1f);
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
		private int[] GetAddMonsterParam()
		{
			return new int[] { UnityEngine.Random.Range(5, 11), 0, UnityEngine.Random.Range(2, 6), 0, 0, 0 };
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
	}
}
