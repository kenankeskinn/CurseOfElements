using PlayerManager;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    public class LevelMaker : EditorWindow
    {
        private string levelName = "NULL";
        private int levelCount = 1;
        private Element earnableElement = Element.None;

        private int zombieCount = 0;
        private int skeletonCount = 0;
        private int ghoulCount = 0;
        private int flyingDemonCount = 0;
        private int necromancerCount = 0;

        [MenuItem("Custom/Level Maker")]
        public static void ShowWindow()
        {
            var window = GetWindow<LevelMaker>("Level Maker");
            window.minSize = new Vector2(700, 420);
        }

        private void OnGUI()
        {
            try
            {
                GUILayout.Space(8);

                // Styles
                var titleStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 32,
                    fixedHeight = 40
                };

                var sectionStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 16,
                };

                // Title
                GUILayout.Label("LEVEL MAKER", titleStyle, GUILayout.ExpandWidth(true));

                GUILayout.Space(12);

                // General Section
                using (new GUILayout.VerticalScope())
                {
                    GUILayout.Label("General", sectionStyle);
                    GUILayout.Space(4);
                    levelName = EditorGUILayout.TextField("Level Name:", levelName);
                    levelCount = EditorGUILayout.IntSlider("Level Count:", levelCount, 1, 4);
                    earnableElement = (Element)EditorGUILayout.EnumPopup("Earnable Element:", earnableElement);
                }

                GUILayout.Space(15);

                // Enemy Settings Section
                using (new GUILayout.VerticalScope())
                {
                    GUILayout.Label("Enemy Settings", sectionStyle);
                    GUILayout.Space(6);
                    zombieCount = EditorGUILayout.IntSlider("Zombie Count:", zombieCount, 0, 50);
                    skeletonCount = EditorGUILayout.IntSlider("Skeleton Count:", skeletonCount, 0, 50);
                    ghoulCount = EditorGUILayout.IntSlider("Ghoul Count:", ghoulCount, 0, 50);
                    flyingDemonCount = EditorGUILayout.IntSlider("Flying Demon Count:", flyingDemonCount, 0, 50);
                    necromancerCount = EditorGUILayout.IntSlider("Necromancer Count:", necromancerCount, 0, 50);
                }

                GUILayout.FlexibleSpace();

                // Create New Level Button
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Create New Level", GUILayout.Width(250), GUILayout.Height(60)))
                    {
                        try
                        {
                            string newSceneName = levelCount + "_" + levelName;

                            bool error = false;
                            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                            {
                                string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

                                if (sceneName == newSceneName) { error = true; break; }
                            }

                            if (!error)
                            {
                                Scene newScene = EditorSceneManager.OpenScene("Assets/Scenes/TemplateScene.unity", OpenSceneMode.Single);

                                // Create Scene
                                EditorSceneManager.SaveScene(newScene, "Assets/Scenes/Levels/" + newSceneName + ".unity");

                                // Add Scene To Build Settings
                                var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                                scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/Levels/" + newSceneName + ".unity", true));
                                EditorBuildSettings.scenes = scenes.ToArray();

                                // Information Log
                                Debug.Log("Scene created and added to Build Settings.");
                            }
                            else { Debug.LogError("There is a scene same name with new scene!"); }
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogException(ex);
                        }
                    }

                    GUILayout.FlexibleSpace();
                }

                GUILayout.Space(10);
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}