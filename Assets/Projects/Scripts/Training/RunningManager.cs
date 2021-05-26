using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using Voxel.UI;
using Voxel.SceneManagement;

namespace Voxel.Training
{
	public class RunningManager : MonoBehaviour
	{
		[SerializeField] private GameObject runner;
		[SerializeField] private Transform pathRoot;
		private Transform[] passPoints;
		private int passIdx;

		private float jumpDistance = 3f;
		[SerializeField] private float speed = 10f; // unit/�b

		private Tween tween;

		[SerializeField] private ParamResultUI resultUi;

		// Start is called before the first frame update
		void Start()
		{
			var paths = pathRoot.GetComponentsInChildren<PathPoint>();
			passPoints = new Transform[paths.Length];
			for (int i = 0; i < paths.Length; i++)
			{
				passPoints[i] = pathRoot.GetComponentsInChildren<PathPoint>()[i].gameObject.transform;
			}

			// �g���[�j���O�͂�����
			StartRunning();

			//runner.transform.position = passPoints.Last().position;
			//Run();
		}

		private void StartRunning()
		{
			Comment.Instance.Show("�E�b�V �ł�");
			Observable.Timer(TimeSpan.FromSeconds(2))
				.Subscribe(_ =>
				{
					tween.Pause();
					resultUi.ShowResult();
					Observable.Timer(TimeSpan.FromSeconds(1))
						.Subscribe(_ =>
						{
							resultUi.HideResult();
							// �p�����[�^�X�V
							var addVal = GetAddMonsterParam();
							var monsterparam = SaveDataManager.SaveData.CurrentMonster;
							resultUi.ShowParam(monsterparam, addVal);
							UpdateMonsterParam(monsterparam, addVal);
							// �R�����g
							Comment.Instance.SetComment(GetResultComment(addVal));

							Observable.Timer(TimeSpan.FromSeconds(4))
							.Subscribe(_ =>
							{
								CalendarManager.Instance.NextWeek();
								FadeManager.Instance.PlayFadeOut(() => SceneLoader.ChangeScene(SceneLoader.SceneName.Farm));
							}).AddTo(this);
						}).AddTo(this);
				}).AddTo(this);

			passIdx = 1;
			Run();
			FadeManager.Instance.PlayFadeIn();
		}

		private void Run()
		{
			var target = passPoints[passIdx].position;
			var distance = Vector3.Distance(target, runner.transform.position);
			int junpCount = (int)( distance/ jumpDistance);
			runner.transform.LookAt(target);
			tween = runner.transform.DOJump(passPoints[passIdx].position, 2f, junpCount, distance / speed)
				.SetEase(Ease.Linear)
				.OnComplete(() => 
				{
					passIdx++;
					if (passIdx >= passPoints.Length) return;
					Run();
				});
		}

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
		}

		private int[] GetAddMonsterParam()
		{
			return new int[] { UnityEngine.Random.Range(5,11), 0, UnityEngine.Random.Range(2, 6), 0, 0, 0};
		}
	}
}
