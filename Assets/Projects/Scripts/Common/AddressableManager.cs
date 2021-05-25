using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace Voxel
{
	public class AddressableManager : SingletonMonoBehaviour<AddressableManager>
	{
		public async UniTask<GameObject> Load(string assetAddress)
		{
			var handle = Addressables
				.LoadAssetAsync<GameObject>(assetAddress);
			await handle.Task;
			return handle.Result;
		}
	}
}
