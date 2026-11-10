// Copyright (c) Meta Platforms, Inc. and affiliates.

using Meta.XR.Samples;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Meta.XR.MRUtilityKitSamples.StartScene
{
    [MetaCodeSample("MRUKSample-StartScene")]
    public class MenuUIBuilder : MonoBehaviour
    {
        /// <summary>The spacing between UI elements in pixels.</summary>
        public float ElementSpacing = 16.0f;
        /// <summary>The horizontal margin for UI elements in pixels.</summary>
        public float MarginH = 32.0f;
        /// <summary>The vertical margin for UI elements in pixels.</summary>
        public float MarginV = 32.0f;
        /// <summary>The distance from the camera at which the UI should be spawned.</summary>
        public float SpawnDistanceFromCamera = 0.3f;
        /// <summary>The event system used for menu interactions.</summary>
        [Space(5)] public EventSystem MenuEventSystem;
        [SerializeField] private UICursor Cursor;
        [SerializeField] private RectTransform _buttonPrefab;
        [SerializeField] private RectTransform _labelPrefab;
        private Transform _targetContentPanel;
        private readonly List<RectTransform> _insertedElements = new();
        private OVRCameraRig _cameraRig;
        private Transform _parentTransform;
        private readonly float _followSpeed = 2.5f;

        private delegate void OnClick();

        private void Awake()
        {
            _cameraRig = FindAnyObjectByType<OVRCameraRig>();
            _targetContentPanel = GetComponent<RectTransform>();
            _parentTransform = transform.parent;
            if (EventSystem.current == null)
            {
                EventSystem.current = MenuEventSystem;
            }
            else if (EventSystem.current != MenuEventSystem)
            {
                // When navigating back to the StartScene, discard the left-over event system in favor
                // of the StarScene's.
                EventSystem.current.enabled = false;
                EventSystem.current = MenuEventSystem;
            }
        }

        private void Start()
        {
            BuildMenuUI();
            Show();
#if UNITY_ANDROID && !UNITY_EDITOR
            LoadSceneFromExtraParams();
#endif
        }

        private void Update()
        {
            Billboard();
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        /// <summary>
        /// This allows the app to be launched directly into a specific scene. For example:
        ///
        /// Unity 2022: `adb shell am start -n com.meta.MRUKSample/com.unity3d.player.UnityPlayerActivity -e scene VirtualHome`
        /// Unity 6: `adb shell am start -n com.meta.MRUKSample/com.unity3d.player.UnityPlayerGameActivity -e scene VirtualHome`
        /// </summary>
        private void LoadSceneFromExtraParams()
        {
            using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var intent = currentActivity.Call<AndroidJavaObject>("getIntent");
            string sceneName = intent.Call<string>("getStringExtra", "scene");
            if (!string.IsNullOrEmpty(sceneName))
            {
                Debug.Log($"Trying to find scene with name: {sceneName}");
                var generalScenes = GetScenesInBuildSettings();
                foreach (var generalScene in generalScenes)
                {
                    if (generalScene.Item2.Contains(sceneName))
                    {
                        Debug.Log($"Loading scene: {generalScene.Item2}");
                        LoadScene(generalScene.Item1);
                        return;
                    }
                }

                Debug.LogError($"Could not find scene with name: {sceneName}");
            }
        }
#endif

        /// <summary>
        /// Builds the Menu UI adding labels and buttons to enable navigations across different scenes.
        /// </summary>
        private void BuildMenuUI()
        {
            var generalScenes = GetScenesInBuildSettings();

            if (generalScenes.Count > 0)
            {
                foreach (var scene in generalScenes)
                {
                    AddButton(Path.GetFileNameWithoutExtension(scene.Item2), () => LoadScene(scene.Item1));
                }
            }

            AddLabel("Press the Menu Button at any time to return to this scene");
        }

        /// <summary>
        /// Returns the list of scenes included in the build settings.
        /// </summary>
        /// <returns>A tuple where each element is represented by the scene index (int) and the scene name (string)</returns>
        private static List<Tuple<int, string>> GetScenesInBuildSettings()
        {
            var generalScenes = new List<Tuple<int, string>>();

            var n = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            for (var sceneIndex = 0; sceneIndex < n; ++sceneIndex)
            {
                var path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(sceneIndex);
                if (!path.Contains(SceneManager.GetActiveScene().name))
                {
                    generalScenes.Add(new Tuple<int, string>(sceneIndex, path));
                }
            }

            return generalScenes;
        }

        private void LoadScene(int idx)
        {
            Hide();
            UnityEngine.SceneManagement.SceneManager.LoadScene(idx);
        }


        /// <summary>
        /// Adds a button to the procedural UI that will execute the OnClick delegate passed as a parameter.
        /// </summary>
        /// <param name="label">The text to be displayed on the button.</param>
        /// <param name="handler">The delegate to execute when clicking the button.</param>
        /// <returns>The RectTransform of the newly created button.</returns>
        private RectTransform AddButton(string label, OnClick handler = null)
        {
            var buttonRT = Instantiate(_buttonPrefab).GetComponent<RectTransform>();

            var button = buttonRT.GetComponentInChildren<Button>();
            if (handler != null)
            {
                button.onClick.AddListener(delegate { handler(); });
            }


            ((TextMeshProUGUI)buttonRT.GetComponentInChildren(typeof(TextMeshProUGUI), true)).text = label;


            AddRect(buttonRT);
            return buttonRT;
        }

        /// <summary>
        /// Adds a text label to the procedural UI.
        /// </summary>
        /// <param name="label">The text to be displayed.</param>
        /// <returns>The RectTransform of the newly created label</returns>
        private RectTransform AddLabel(string label)
        {
            var rt = Instantiate(_labelPrefab).GetComponent<RectTransform>();
            rt.GetComponent<TextMeshProUGUI>().text = label;
            AddRect(rt);
            return rt;
        }

        /// <summary>
        /// Adds a RectTransform to the procedural UI.
        /// </summary>
        /// <param name="r">The RectTransform to add to the UI.</param>
        private void AddRect(RectTransform r)
        {
            r.transform.SetParent(_targetContentPanel, false);
            _insertedElements.Add(r);
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Calculates and organizes the layout of the UI and the positioning of its elements.
        /// </summary>
        private void Relayout()
        {
            var canvasRect = _targetContentPanel.GetComponent<RectTransform>();
            var elemCount = _insertedElements.Count;
            var x = MarginH;
            var y = -MarginV;
            var maxWidth = 0.0f;
            for (var elemIdx = 0; elemIdx < elemCount; ++elemIdx)
            {
                var r = _insertedElements[elemIdx];
                r.anchoredPosition = new Vector2(x, y);


                y -= r.rect.height + ElementSpacing;


                maxWidth = Mathf.Max(r.rect.width + 2 * MarginH, maxWidth);
            }

            canvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
            canvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -y + MarginV);
        }

        /// <summary>
        /// Keeps the UI placed in front of the camera and facing it.
        /// </summary>
        private void Billboard()
        {
            if (!_cameraRig)
            {
                return;
            }

            var direction = _parentTransform.position - _cameraRig.centerEyeAnchor.transform.position;
            if (direction.sqrMagnitude > 0.01f)
            {
                var rotation = Quaternion.LookRotation(direction);
                _parentTransform.rotation = rotation;

                _parentTransform.position = Vector3.Lerp(_parentTransform.position,
                    _cameraRig.centerEyeAnchor.transform.position +
                    _cameraRig.centerEyeAnchor.transform.forward * SpawnDistanceFromCamera,
                    Time.deltaTime * _followSpeed);
            }
        }
    }
}
