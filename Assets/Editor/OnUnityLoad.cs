using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class OnUnityLoad {

	static OnUnityLoad () {

		EditorApplication.playmodeStateChanged = () => {

			if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying) {
				EditorApplication.SaveScene();
				AssetDatabase.SaveAssets();
			}

		};

	}

}