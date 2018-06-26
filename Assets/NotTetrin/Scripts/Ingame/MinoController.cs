using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame {
    [RequireComponent(typeof(Rigidbody2D))]
    public class MinoController : MonoBehaviour {
        private IngameSfxManager sfxManager;
        private ParticleSystem dropEffect;
        private new Rigidbody2D rigidbody;

        private bool hit = false;

        private static float LimitAngularVelocity = 180.0f;
        private float prevHorizontal;

        private float fallSpeed = 1.5f;
        private float fallAccelaration = 0.0f;

        private static AnimationCurve SoftdropAccelarationCurve = new AnimationCurve(
            new Keyframe(0, 0, 1.617043f, 1.617043f, 0, 0.1401876f),
            new Keyframe(1, 1, 0.7356746f, 0.7356746f, 0.3873379f, 0)
        );
        public int SoftdropFrames { get; private set; } = 0;
        private static int SoftdropPeekFrame = 60;
        private static float SoftdropPeekAccelaration = 5.4f;

        public event EventHandler Hit;

        public void Awake() {
            dropEffect = GetComponentInChildren<ParticleSystem>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public MinoController Initialize(IngameSfxManager audioManager) {
            this.sfxManager = audioManager;
            return this;
        }

        public void Start() {
            dropEffect.Play();
        }

        public void Update() {
            var rigidbody = GetComponent<Rigidbody2D>();
            var velocity = new Vector2(rigidbody.velocity.x, -fallSpeed);
            var torque = 0.0f;

            var horizontal = Input.GetAxis(@"Horizontal");
            if (prevHorizontal <= 0 && horizontal > 0 || prevHorizontal >= 0 && horizontal < 0) {
                sfxManager.Play(IngameSfxType.MinoMove);
            }
            if (horizontal < 0) {
                velocity.x -= 0.1f;
            }
            if (horizontal > 0) {
                velocity.x += 0.1f;
            }
            prevHorizontal = horizontal;

            var vertical = Input.GetAxis(@"Vertical");
            if (vertical < 0) {
                SoftdropFrames++;
                var frames = Mathf.Clamp(SoftdropFrames, 0, SoftdropPeekFrame);
                fallAccelaration = SoftdropPeekAccelaration * SoftdropAccelarationCurve.Evaluate((float)frames / SoftdropPeekFrame);
            } else {
                SoftdropFrames = 0;
                fallAccelaration *= 0.86f;
            }

            if (Input.GetButtonDown(@"Rotate Left") || Input.GetButtonDown(@"Rotate Right")) {
                sfxManager.Play(IngameSfxType.MinoTurn);
            }
            if (Input.GetButton(@"Rotate Left")) {
                torque += 2.0f;
            }
            if (Input.GetButton(@"Rotate Right")) {
                torque -= 2.0f;
            }

            velocity.y -= fallAccelaration;

            rigidbody.AddTorque(torque);
            rigidbody.velocity = velocity;
            rigidbody.angularVelocity = Mathf.Clamp(rigidbody.angularVelocity, -LimitAngularVelocity, LimitAngularVelocity);

            dropEffect.transform.rotation = Quaternion.identity;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (hit) { return; }
            if (other.collider.CompareTag(@"Wall")) { return; }

            hit = true;
            sfxManager.Play(IngameSfxType.MinoHit);

            dropEffect.Stop();
            Destroy(dropEffect.gameObject, 1.0f);
            Destroy(this);

            Hit?.Invoke(this, EventArgs.Empty);
        }
    }
}
