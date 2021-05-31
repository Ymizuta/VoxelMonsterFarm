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
						// フェードが明けてから実行
						StartCoroutine(Run(tData.TrainingType, TrainingResult.Success));
					});
				}));
		}

		/// <summary>
		/// アニメーションシーンを実行
		/// </summary>
		/// <returns></returns>
		public IEnumerator Run(TrainingType type, TrainingResult result)
		{
			// 練習シーンを取得・実行
			manager.Initialize(SaveDataManager.SaveData.CurrentMonster.MonsterModelId);
			yield return manager.Run(result);

			// 制御が戻ったら結果発表
			resultUi.ShowResult();
			yield return new WaitForSeconds(1f);
			resultUi.HideResult();

			// パラメータ更新
			var monsterparam = SaveDataManager.SaveData.CurrentMonster;
			var addVal = GetAddMonsterParam(GetParamType(type), new MonsterCalculator().CalcCondition(monsterparam.Fatigue));
			resultUi.ShowParam(monsterparam, addVal);
			UpdateMonsterParam(monsterparam, addVal);
			Comment.Instance.Show(GetResultComment(addVal));

			// 入力を待って牧場に戻る
			yield return PlayerInput();
			CalendarManager.Instance.NextWeek();
			FadeManager.Instance.PlayFadeOut(() => SceneLoader.Instance.ChangeScene(SceneLoader.SceneName.Farm));
		}

		/// <summary>
		/// アニメーション用のシーンを非同期ロード
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
		/// 結果コメントを取得
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
					comment += $"{calc.GetParamName((ParamType)i)}が{addVal[i]}上がりました\n";
				}
			}
			return comment;
		}

		/// <summary>
		/// 変更パラメータを取得
		/// </summary>
		/// <returns></returns>
		private int[] GetAddMonsterParam(ParamType type, MonsterCondition condition)
		{
			var addVal = new int[6];
			var val = UnityEngine.Random.Range(2, 5);
			if (condition == MonsterCondition.Tired || condition == MonsterCondition.VeryTired)
			{
				// 疲労が溜まっていると上昇値が減る
				val = Mathf.Clamp((int)(val * 0.5f), 0, val);
			}
			addVal[(int)type] = val;
			return addVal;
		}

		/// <summary>
		/// モンスターのパラムを更新
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
					Debug.LogWarning($"想定していないタイプが選択されました = {type}");
					return ParamType.Hp;
			}
		}
	}
}
