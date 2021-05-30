using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class MonsterParamMaster_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Projects/Datas/Excels/MonsterParamMaster.xls";
	private static readonly string exportPath = "Assets/Projects/Datas/Excels/MonsterParamMaster.asset";
	private static readonly string[] sheetNames = { "MonsterParamMaster", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_MonsterParamMaster data = (Entity_MonsterParamMaster)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_MonsterParamMaster));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_MonsterParamMaster> ();
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

					Entity_MonsterParamMaster.Sheet s = new Entity_MonsterParamMaster.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_MonsterParamMaster.Param p = new Entity_MonsterParamMaster.Param ();
						
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.monsterType = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.monsterName = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.modelId = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.grade = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.hp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.power = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.guts = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.hit = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.speed = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.deffence = (int)(cell == null ? 0 : cell.NumericCellValue);
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
