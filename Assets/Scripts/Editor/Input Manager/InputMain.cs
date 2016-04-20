using UnityEngine;
using System.Collections;
using UnityEditor;

public class InputMain : EditorWindow {
   // Add menu item named "My Window" to the Window menu
   [MenuItem("Window/Input Manager")]
   public static void ShowWindow()
   {
      //Show existing window instance. If one doesn't exist, make one.
      EditorWindow.GetWindow(typeof(InputMain));
   }

   void OnGUI()
   {
      GUILayout.Label("Base Settings", EditorStyles.boldLabel);
      GUILayout.TextField("test", 25);
   }

   // Use this for initialization
   void Start()
   {
   }

   // Update is called once per frame
   void Update()
   {
   }
}
