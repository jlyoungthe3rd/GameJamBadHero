using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;

// This attribute makes the class run when the editor is launched.
[InitializeOnLoad]
public class AutoSaveScene
{
    // Static constructor that gets called when the editor loads
    static AutoSaveScene()
    {
        // This hooks our custom 'Update' method into the editor's update loop.
        // So, our Update method gets called many times per second.
        EditorApplication.update += Update;
    }

    // This function will be called every time the editor updates.
    static void Update()
    {
        // Check if enough time has passed since our last save.
        // We are using a 'persistent' editor preference to store the last save time.
        string lastSaveTimeString = EditorPrefs.GetString("AutoSaveLastSaveTime", "");
        if (!string.IsNullOrEmpty(lastSaveTimeString))
        {
            long lastSaveTimeTicks = long.Parse(lastSaveTimeString);
            DateTime lastSaveTime = new DateTime(lastSaveTimeTicks);

            // If 5 minutes have not yet passed, do nothing.
            if (lastSaveTime.AddMinutes(5) > DateTime.Now)
            {
                return;
            }
        }
        
        // If we are in play mode, about to enter play mode, or compiling, don't save.
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling)
        {
            return;
        }

        // Save the currently open scene(s).
        Debug.Log("Auto-saving scene...");
        EditorSceneManager.SaveOpenScenes();
        
        // Record the time of this save.
        EditorPrefs.SetString("AutoSaveLastSaveTime", DateTime.Now.Ticks.ToString());
    }
}