﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
    /// <summary>
    /// Add this to an AudioSource to shake its stereo pan values remapped along a curve 
    /// </summary>
    [AddComponentMenu("More Mountains/Feedbacks/Shakers/Audio/MMAudioSourceStereoPanShaker")]
    [RequireComponent(typeof(AudioSource))]
    public class MMAudioSourceStereoPanShaker : MMShaker
    {
        [Header("Stereo Pan")]
        /// whether or not to add to the initial value
        public bool RelativeStereoPan = false;
        /// the curve used to animate the intensity value on
        public AnimationCurve ShakeStereoPan = new AnimationCurve(new Keyframe(0, 0f), new Keyframe(0.3f, 1f), new Keyframe(0.6f, -1f), new Keyframe(1, 0f));
        /// the value to remap the curve's 0 to
        [Range(-1f, 1f)]
        public float RemapStereoPanZero = 0f;
        /// the value to remap the curve's 1 to
        [Range(-1f, 1f)]
        public float RemapStereoPanOne = 1f;

        /// the audio source to pilot
        protected AudioSource _targetAudioSource;
        protected float _initialStereoPan;
        protected float _originalShakeDuration;
        protected bool _originalRelativeValues;
        protected AnimationCurve _originalShakeStereoPan;
        protected float _originalRemapStereoPanZero;
        protected float _originalRemapStereoPanOne;

        /// <summary>
        /// On init we initialize our values
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();
            _targetAudioSource = this.gameObject.GetComponent<AudioSource>();
        }
               
        /// <summary>
        /// When that shaker gets added, we initialize its shake duration
        /// </summary>
        protected virtual void Reset()
        {
            ShakeDuration = 2f;
        }

        /// <summary>
        /// Shakes values over time
        /// </summary>
        protected override void Shake()
        {
            float newStereoPan = ShakeFloat(ShakeStereoPan, RemapStereoPanZero, RemapStereoPanOne, RelativeStereoPan, _initialStereoPan);
            _targetAudioSource.panStereo = newStereoPan;
        }

        /// <summary>
        /// Collects initial values on the target
        /// </summary>
        protected override void GrabInitialValues()
        {
            _initialStereoPan = _targetAudioSource.panStereo;
        }

        /// <summary>
        /// When we get the appropriate event, we trigger a shake
        /// </summary>
        /// <param name="stereoPanCurve"></param>
        /// <param name="duration"></param>
        /// <param name="amplitude"></param>
        /// <param name="relativeStereoPan"></param>
        /// <param name="attenuation"></param>
        /// <param name="channel"></param>
        public virtual void OnMMAudioSourceStereoPanShakeEvent(AnimationCurve stereoPanCurve, float duration, float remapMin, float remapMax, bool relativeStereoPan = false,
            float attenuation = 1.0f, int channel = 0, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true)
        {
            if (!CheckEventAllowed(channel) || Shaking)
            {
                return;
            }
            
            _resetShakerValuesAfterShake = resetShakerValuesAfterShake;
            _resetTargetValuesAfterShake = resetTargetValuesAfterShake;

            if (resetShakerValuesAfterShake)
            {
                _originalShakeDuration = ShakeDuration;
                _originalShakeStereoPan = ShakeStereoPan;
                _originalRemapStereoPanZero = RemapStereoPanZero;
                _originalRemapStereoPanOne = RemapStereoPanOne;
                _originalRelativeValues = RelativeStereoPan;
            }

            ShakeDuration = duration;
            ShakeStereoPan = stereoPanCurve;
            RemapStereoPanZero = remapMin * attenuation;
            RemapStereoPanOne = remapMax * attenuation;
            RelativeStereoPan = relativeStereoPan;

            Play();
        }

        /// <summary>
        /// Resets the target's values
        /// </summary>
        protected override void ResetTargetValues()
        {
            base.ResetTargetValues();
            _targetAudioSource.panStereo = _initialStereoPan;
        }

        /// <summary>
        /// Resets the shaker's values
        /// </summary>
        protected override void ResetShakerValues()
        {
            base.ResetShakerValues();
            ShakeDuration = _originalShakeDuration;
            ShakeStereoPan = _originalShakeStereoPan;
            RemapStereoPanZero = _originalRemapStereoPanZero;
            RemapStereoPanOne = _originalRemapStereoPanOne;
            RelativeStereoPan = _originalRelativeValues;
        }

        /// <summary>
        /// Starts listening for events
        /// </summary>
        public override void StartListening()
        {
            base.StartListening();
            MMAudioSourceStereoPanShakeEvent.Register(OnMMAudioSourceStereoPanShakeEvent);
        }

        /// <summary>
        /// Stops listening for events
        /// </summary>
        public override void StopListening()
        {
            base.StopListening();
            MMAudioSourceStereoPanShakeEvent.Unregister(OnMMAudioSourceStereoPanShakeEvent);
        }
    }

    /// <summary>
    /// An event used to trigger vignette shakes
    /// </summary>
    public struct MMAudioSourceStereoPanShakeEvent
    {
        public delegate void Delegate(AnimationCurve stereoPanCurve, float duration, float remapMin, float remapMax, bool relativeStereoPan = false,
            float attenuation = 1.0f, int channel = 0, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true);
        static private event Delegate OnEvent;

        static public void Register(Delegate callback)
        {
            OnEvent += callback;
        }

        static public void Unregister(Delegate callback)
        {
            OnEvent -= callback;
        }

        static public void Trigger(AnimationCurve stereoPanCurve, float duration, float remapMin, float remapMax, bool relativeStereoPan = false,
            float attenuation = 1.0f, int channel = 0, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true)
        {
            OnEvent?.Invoke(stereoPanCurve, duration, remapMin, remapMax, relativeStereoPan,
                attenuation, channel, resetShakerValuesAfterShake, resetTargetValuesAfterShake);
        }
    }
}
