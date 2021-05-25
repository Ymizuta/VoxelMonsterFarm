using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorSceneChanger : MonoBehaviour
{
	[MenuItem("ChangeScene/Boot")]
	public static void ChangeBootScene()
	{
		ChangeScene("Boot");
	}

	[MenuItem("ChangeScene/Farm")]
	public static void ChangeFarmScene()
	{
		ChangeScene("Farm");
	}

	[MenuItem("ChangeScene/Tournament")]
	public static void ChangeTournamentScene()
	{
		ChangeScene("Tournament");
	}

	private static void ChangeScene(string scene)
	{
		EditorApplication.SaveCurrentSceneIfUserWantsTo();
		EditorSceneManager.OpenScene($"Assets/Projects/Scenes/{scene}.unity");
	}
}
