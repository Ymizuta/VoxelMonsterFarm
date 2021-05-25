using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
	public class CanvasManager : SingletonMonoBehaviour<CanvasManager>
	{
		[SerializeField] private Canvas backCanvas;
		[SerializeField] private Canvas frontCanvas;
		public Canvas BackCanvas => backCanvas;
		public Canvas FrontCanvas => frontCanvas;
	}
}
