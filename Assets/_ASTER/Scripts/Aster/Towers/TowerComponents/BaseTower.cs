using System;
using System.Linq;
using Aster.Core;
using Aster.Core.Entity;
using Aster.Light;
using Aster.Utils;
using DependencyInjection;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aster.Towers
{
    public abstract class BaseTower : BaseLightHittable
    {
        protected EntityHP HP;

        public abstract LightReceiver LightReceiver { get; }

        public bool Duplicated = false;

        protected override LightHitContext OnLightRayHit(LightHit lightHit)
        {
            LightReceiver.Register(lightHit);

            return CreateHitContext(lightHit);
        }

        protected virtual LightHitContext CreateHitContext(LightHit hit)
        {
            return new(hit, blockLight: true);
        }

        public override void OnLightRayExit(LightRayObject rayObject)
        {
            LightReceiver.Deregister(rayObject.Data);
        }

        protected override void Reset()
        {
            base.Reset();

            if (IsNotNull(Config)) AssignParametersFromConfig(Config);
        }

        protected virtual void AssignParametersFromConfig(AsterConfiguration config)
        {
        }
    }

    public abstract class TargetingTowerDuplicator<TTower> : AsterMono where TTower : BaseTower
    {
        private LayerMask LAYER_MASK_TARGETING;

        protected int ColliderExcludeLayers;

        [SerializeField, ReadOnly] protected TTower      Original;
        [SerializeField, ReadOnly] protected IRotatatble Rotatable;
        [ShowNonSerializedField]   protected TTower      Duplicate;

        bool                rotationMode;
        private Angle       _activeAngle;
        private IRotatatble _duplicateRotatable;

        private void Awake()
        {
            LAYER_MASK_TARGETING = LayerMask.NameToLayer("Targeting");
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
            if (!transform.IsChildOf(TowerParent.Instance.transform))
            {
                transform.parent = TowerParent.Instance.transform;
            }

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

            Duplicate.AddComponent<TargetingRayMarker>();

            Duplicate.LightReceiver.TargetOnlyMode = true;

            Debug.Log($"Original TargetingMode: {Original.LightReceiver.TargetOnlyMode}. Duplicated TargetingMode: {Duplicate.LightReceiver.TargetOnlyMode}",
                      this);

            duplicateGO.layer = LAYER_MASK_TARGETING;
            foreach (Transform t in duplicateGO.transform.GetComponentInChildren<Transform>())
            {
                t.gameObject.layer = LAYER_MASK_TARGETING;
            }

            DisableMeshRenderers();
            ConfigureColliders();

            duplicateGO.transform.GetComponentsInChildren<IDuplicatable>()
                       .ToList()
                       .ForEach(x => x.OnDuplicate());
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