using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using NotTetrin.Ingame.UI;

namespace NotTetrin.Ingame {
    [DefaultExecutionOrder(1)]
    public class Settings : MonoBehaviour {
        [Header(@"Properties")]
        [SerializeField] private RectTransform container;
        [SerializeField] private AnimationCurve transitionCurve;
        [SerializeField] private float transitionTime;

        [Header(@"Prefabs")]
        [SerializeField] private SettingGroup groupPrefab;
        [SerializeField] private Toggle togglePrefab;
        [SerializeField] private Button buttonPrefab;

        [Header(@"References")]
        [SerializeField] private BGMManager bgmManager;

        private new RectTransform transform;
        private Coroutine coroutine;
        private bool active;

        private void Awake() {
            transform = GetComponent<RectTransform>();
            LoadAndCreateBgmSettings();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                Toggle();
            }
        }

        public void Toggle() {
            if (active) {
                Hide();
            } else {
                Show();
            }
        }

        public void Show() {
            active = true;

            var from_left_x = Camera.main.ViewportToWorldPoint(new Vector3(0, 0)).x;
            var to_left_x = transform.position.x;
            var diff = to_left_x - from_left_x;

            var from_x = Camera.main.transform.position.x;
            var to_x = from_x + diff;

            var camera_pos = Camera.main.transform.position;
            var from = new Vector3(from_x, camera_pos.y, camera_pos.z);
            var to = new Vector3(to_x, camera_pos.y, camera_pos.z);

            if (coroutine != null) {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(cameraMove(from, to));
        }

        public void Hide() {
            active = false;

            var from_x = Camera.main.transform.position.x;
            var to_x = 0.0f;

            var camera_pos = Camera.main.transform.position;
            var from = new Vector3(from_x, camera_pos.y, camera_pos.z);
            var to = new Vector3(to_x, camera_pos.y, camera_pos.z);

            if (coroutine != null) {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(cameraMove(from, to));
        }

        private IEnumerator cameraMove(Vector3 from, Vector3 to) { 
            var startTime = Time.time;

            while (true) {
                var duration = Time.time - startTime;
                var t1 = Mathf.Clamp01(duration / transitionTime);
                var t2 = transitionCurve.Evaluate(t1);
                var p = Vector3.Lerp(from, to, t2);
                Camera.main.transform.position = p;

                if (duration > transitionTime) { break; }
                yield return new WaitForEndOfFrame();
            }
        }
        
        private void LoadAndCreateBgmSettings() {
            var group = Instantiate(groupPrefab, container);
            group.SetHeader(@"BGM設定");

            foreach (var clip in bgmManager.Clips) {
                var key = $"bgm_{clip.name}";
                var enabled = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) == 1 : true;
                PlayerPrefs.SetInt(key, enabled ? 1 : 0);

                if (enabled) {
                    bgmManager.Add(clip);
                }

                var toggle = Instantiate(togglePrefab, group.Container);
                toggle.isOn = enabled;
                toggle.onValueChanged.AddListener(c => OnValueChanged(clip, c));
                var toggleLabel = toggle.GetComponentInChildren<Text>();
                toggleLabel.text = clip.name;
            }

            var button = Instantiate(buttonPrefab, group.Container);
            button.onClick.AddListener(() => bgmManager.RandomPlay());
            var buttonLabel = button.GetComponentInChildren<Text>();
            buttonLabel.text = @"ランダム再生";
        }

        private void OnValueChanged(AudioClip clip, bool enabled) {
            var key = $"bgm_{clip.name}";

            if (enabled) {
                bgmManager.Add(clip);
                PlayerPrefs.SetInt(key, 1);
            } else {
                bgmManager.Remove(clip);
                PlayerPrefs.SetInt(key, 0);
            }
        }
    }
}
