//using System.IO;
//using UnityEngine;
//using UnityEditor;
//using UnityEditor.iOS.Xcode;
//using UnityEditor.Callbacks;
//using System.Collections;
//
//public class XCodeSettingsPostProcesser
//{
//	[PostProcessBuildAttribute (0)]
//	public static void OnPostprocessBuild (BuildTarget buildTarget, string pathToBuiltProject)
//	{
//		// iOS以外のプラットフォームは処理を行わない
//		if (buildTarget != BuildTarget.iOS) return; 
//
//		// PBXProjectの初期化
//		var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
//		PBXProject pbxProject = new PBXProject ();
//		pbxProject.ReadFromFile (projectPath);
////		string targetGuid = pbxProject.TargetGuidByName ("Unity-iPhone");
//
//		// ここに自動化の処理を記述する
//
//		// Plistの設定のための初期化
//		var plistPath = Path.Combine (pathToBuiltProject, "Info.plist");
//		var plist = new PlistDocument ();
//		plist.ReadFromFile (plistPath);
//
//		// 文字列の設定
//		plist.root.SetString ("Privacy - Microphone Usage Description", "マイクから音量を取得します");
//
//		// 設定を反映
//		plist.WriteToFile (plistPath);
//
//		// 設定を反映
//		File.WriteAllText (projectPath, pbxProject.WriteToString ());
//	}
//}