using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class TournamentMaster_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Projects/Datas/Excels/TournamentMaster.xls";
	private static readonly string exportPath = "Assets/Projects/Datas/Excels/TournamentMaster.asset";
	private static readonly string[] sheetNames = { "TournamentMaster", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_TournamentMaster data = (Entity_TournamentMaster)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_TournamentMaster));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_TournamentMaster> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					Entity_TournamentMaster.Sheet s = new Entity_TournamentMaster.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_TournamentMaster.Param p = new Entity_TournamentMaster.Param ();
						
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.tournamentName = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.grade = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.month = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.week = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.monsterCount = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.money = (int)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
