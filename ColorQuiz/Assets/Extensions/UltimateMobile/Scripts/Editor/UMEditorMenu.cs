////////////////////////////////////////////////////////////////////////////////
//  
// @module V2D
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

public class UMEditorMenu : EditorWindow {
	


	//--------------------------------------
	//  GENERAL
	//--------------------------------------

	[MenuItem("Window/UltimateMobile/Edit Settings")]
	public static void Edit() {
		Selection.activeObject = UltimateMobileSettings.Instance;
	}



}
