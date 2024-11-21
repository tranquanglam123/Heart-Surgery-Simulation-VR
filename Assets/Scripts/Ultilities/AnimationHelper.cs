using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VR_Surgery.Scripts.Utilities
{

    /// <summary>
    /// Use for multiple animations models, play all animations in 1 substep
    /// </summary>
    public class AnimationHelper : MonoBehaviour
    {
        private Animation animationComponent;
        private AnimationClip[] animationClips;
        public AnimationClip[] GetAnimationClipsFromImporter(Animation anim)
        {
            List<AnimationClip> clips = new List<AnimationClip>();

            foreach (AnimationState state in anim)
            {
                if (!state.clip.name.StartsWith("__preview__"))
                {
                    clips.Add(state.clip);
                }
            }

            return clips.ToArray();
        }
        public void PLayAllAnimationClips(Animation component)
        {
            animationComponent = component;
            if (animationClips != null)
            {
                Array.Clear(animationClips, 0, animationClips.Length);
            }
            animationClips = GetAnimationClipsFromImporter(component);
            //#if UNITY_EDITOR
            //            animationClips = AnimationUtility.GetAnimationClips(animationComponent.gameObject);
            //#endif
            Debug.Log($"Clip count: {animationClips.Length}");
            // Add all clips to the Animation component
            foreach (AnimationClip clip in animationClips)
            {
                animationComponent.AddClip(clip, clip.name);
                Debug.Log($"Adding the {clip.name}");
            }

            // Start playing animations sequentially
            if (animationClips.Length > 0)
            {
                StartCoroutine(PlayAnimationsSequentially());
            }
        }

        IEnumerator PlayAnimationsSequentially()
        {
            int currentClipIndex = 0;

            while (animationComponent)
            {
                animationComponent.clip = animationClips[currentClipIndex];
                animationComponent.Play();
                Debug.Log($"Still playing the {animationClips[currentClipIndex].name}");
                // Wait until the current animation clip finishes
                yield return new WaitForSeconds(animationClips[currentClipIndex].length + 0.5f);

                // Move to the next clip, looping back to the first clip if necessary
                currentClipIndex = (currentClipIndex + 1) % animationClips.Length;
            }
        }
    }
}
