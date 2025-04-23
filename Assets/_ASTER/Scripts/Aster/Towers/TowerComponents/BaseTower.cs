using System;
using Aster.Core;
using Aster.Core.Entity;
using Aster.Light;
using Aster.Utils;
using DependencyInjection;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Towers
{
    public abstract class BaseTower : BaseLightHittable
    {
        protected EntityHP HP;

        protected LightReceiver lightReceiver = new();
        public    LightReceiver LightReceiver => lightReceiver;

        public bool Duplicated = false;

        public override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            if (lightReceiver.TargetOnlyMode && lightHit.Ray is not TargetingRay)
            {
                return new LightHitContext(lightHit, blockLight: false);
            }

            lightReceiver.Register(lightHit);

            return CreateHitContext(lightHit);
        }

        protected virtual LightHitContext CreateHitContext(LightHit hit)
        {
            return new(hit, blockLight: true);
        }

        public override void OnLightRayExit(LightRayObject rayObject)
        {
            lightReceiver.Deregister(rayObject.Data);
        }
    }

    public abstract class TargetingTowerDuplicator<TTower> : AsterMono where TTower : BaseTower
    {
        protected int ColliderExcludeLayers;

        [SerializeField, ReadOnly] protected TTower      Original;
        [SerializeField, ReadOnly] protected IRotatatble Rotatable;
        [ShowNonSerializedField]   protected TTower      Duplicate;

        bool                rotationMode;
        private Angle       _activeAngle;
        private IRotatatble _duplicateRotatable;

        private void Awake()
        {
            Reset();

            ColliderExcludeLayers = LayerMask.GetMask("Player");

            transform.ScanForComponents(out TargetingTowerDuplicator<TTower>[] duplicators, parents: true,
                                        children: true);

            if (duplicators != null && duplicators.Length > 1) Destroy(this);
        }

        private void OnEnable()
        {
            GameEvents.OnRotationInteractionBegin += OnInteractionBegin;
            GameEvents.OnInteractionEnd           += OnInteractionEnd;
        }

        private void OnDisable()
        {
            GameEvents.OnRotationInteractionBegin -= OnInteractionBegin;
            GameEvents.OnInteractionEnd           -= OnInteractionEnd;
        }

        private void Start()
        {
            if (Duplicate == null && !Original.Duplicated)
            {
                Original.Duplicated = true;
                CreateDuplicate();
                Duplicate.Duplicated = true;
            }
        }

        private void OnInteractionEnd(InteractionContext obj)
        {
            if (Rotatable != obj.Interactable) return;
            if (Duplicate == null) return;

            if (TargetingRay.IgnoreHittable == Original) TargetingRay.IgnoreHittable = null;

            rotationMode = false;
        }

        private void OnInteractionBegin(RotationInteractionContext obj)
        {
            if (Rotatable != obj.Interactable) return;

            TargetingRay.IgnoreHittable = Original;
            rotationMode                = true;
        }

        private void CreateDuplicate()
        {
            GameObject duplicateGO = Instantiate(Original.gameObject, Original.transform);
            Destroy(duplicateGO.GetComponent<TTower>());

            Component duplicator = duplicateGO.transform.GetComponent<TargetingTowerDuplicator<TTower>>();
            Destroy(duplicator);

            duplicateGO.transform.position = Original.transform.position;

            _duplicateRotatable = ConfigureRotatable(duplicateGO);

            Duplicate            = duplicateGO.AddComponent<TTower>();
            Duplicate.Duplicated = true;

            Duplicate.LightReceiver.TargetOnlyMode = true;

            Debug.Log($"Original TargetingMode: {Original.LightReceiver.TargetOnlyMode}. Duplicated TargetingMode: {Duplicate.LightReceiver.TargetOnlyMode}",
                      this);

            // DisableMeshRenderers();
            ConfigureColliders();
        }

        protected abstract IRotatatble ConfigureRotatable(GameObject duplicate);

        private void FixedUpdate()
        {
            if (Duplicate == null) return;

            Duplicate.transform.position = Original.transform.position;

            if (rotationMode)
            {
                Angle angle = Rotatable.RotationHandler.ActiveTargetingAngle;

                _duplicateRotatable.RotationHandler.Rotate(angle);
            }
            else
            {
                Duplicate.transform.rotation = Original.transform.rotation;
            }
        }

        private void ConfigureColliders()
        {
            Collider[] colliders = Duplicate.GetComponentsInChildren<Collider>(true);

            foreach (Collider collider in colliders)
            {
                collider.excludeLayers = ColliderExcludeLayers;
            }
        }

        private void DisableMeshRenderers()
        {
            MeshRenderer[] renderers = Duplicate.GetComponentsInChildren<MeshRenderer>(true);

            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }


        private void Reset()
        {
            ValidateComponent(ref Original);
            ValidateRawComponent(ref Rotatable);
            if (Rotatable == null) Debug.LogError("Rotatable is null", this);
        }
    }
}