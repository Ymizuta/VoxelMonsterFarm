using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Voxel.UI;

namespace Voxel.Farm
{
	public class FarmTopModel : ModelBase
	{
		/// <summary>
		/// ‘€ìó‘Ô
		/// </summary>
		public enum CommandType
		{
			None,
			FarmTopMenu,
			TrainingMenu,
			MonsterParam,
		}

		public enum FarmTopMenu
		{
			Training = 0, // ˆç¬
			TakeRest, // ‹x—{
			Tournament, // ‘å‰ï
			Params,
		}

		public enum TrainingMenu
		{
			Running = 0, // ‘–‚è‚İ
			ObstacleCourse, // áŠQ•¨ƒR[ƒX
			Swimming, // …‰j
			Meditation, // áÒ‘z
			DestroyObstacle, // áŠQ•¨”j‰ó
		}

		public ReactiveProperty<CommandType> Command = new ReactiveProperty<CommandType>();

		public readonly string[] FarmTopMenuStrs = new string[] { "ˆç¬", "‹x—{", "‘å‰ï", "”\—Í’l"};
		public readonly string[] TrainingMenuStrs = new string[] { "‘–‚è‚İ", "áŠQ•¨", "…‰j", "áÒ‘z", "‘ÅŒ‚ŒP—û"};

		public MonsterParam MonsterParam { get; private set; }

		public override void Initialize()
		{
			base.Initialize();
			MonsterParam = SaveDataManager.SaveData.CurrentMonster;
		}

		public string GetInitComment()
		{
			var comments = new string[] 
			{
				"ƒCƒbƒk ‚ÍŒ³‹C‚¾‚æI",
				"ƒCƒbƒk ‚Í‚·‚Á‚²‚­Œ³‹C‚¾‚æI",
				"ƒCƒbƒk ‚Í­‚µ”æ‚ê‚Ä‚é‚İ‚½‚¢c",
			};
			return comments[Random.Range(0,3)];
		}

		protected override void OnBack()
		{
			base.OnBack();
		}
	}
}
