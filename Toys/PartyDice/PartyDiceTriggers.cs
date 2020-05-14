
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace Childofthebeast.Toys
{
    public class PartyDiceTriggers : UdonSharpBehaviour
    {
        public ParticleSystem Side_1_PS;
        public AudioClip Side_1_Audio;
        public ParticleSystem Side_2_PS;
        public AudioClip Side_2_Audio;
        public ParticleSystem Side_3_PS;
        public AudioClip Side_3_Audio;
        public ParticleSystem Side_4_PS;
        public AudioClip Side_4_Audio;
        public ParticleSystem Side_5_PS;
        public AudioClip Side_5_Audio;
        public ParticleSystem Side_6_PS;
        public AudioClip Side_6_Audio;

        public InputField OutputUwU;
        public AudioSource SoundBoi;
        public override void Interact()
        {
            int v = int.Parse(OutputUwU.text);
            switch (v)
            {
                case 1:
                    if (Side_1_Audio != null)
                    {
                        SoundBoi.Stop();
                        SoundBoi.clip = Side_1_Audio;
                        SoundBoi.Play();
                    }
                    if (Side_1_PS != null)
                    {
                        Side_1_PS.Stop();
                        Side_1_PS.Play();
                    }
                    break;
                case 2:
                    if (Side_2_Audio != null)
                    {
                        SoundBoi.Stop();
                        SoundBoi.clip = Side_2_Audio;
                        SoundBoi.Play();
                    }
                    if (Side_2_PS != null)
                    {
                        Side_2_PS.Stop();
                        Side_2_PS.Play();
                    }
                    break;
                case 3:
                    if (Side_3_Audio != null)
                    {
                        SoundBoi.Stop();
                        SoundBoi.clip = Side_3_Audio;
                        SoundBoi.Play();
                    }
                    if (Side_3_PS != null)
                    {
                        Side_3_PS.Stop();
                        Side_3_PS.Play();
                    }
                    break;
                case 4:
                    if (Side_4_Audio != null)
                    {
                        SoundBoi.Stop();
                        SoundBoi.clip = Side_4_Audio;
                        SoundBoi.Play();
                    }
                    if (Side_4_PS != null)
                    {
                        Side_4_PS.Stop();
                        Side_4_PS.Play();
                    }
                    break;
                case 5:
                    if (Side_5_Audio != null)
                    {
                        SoundBoi.Stop();
                        SoundBoi.clip = Side_5_Audio;
                        SoundBoi.Play();
                    }
                    if (Side_5_PS != null)
                    {
                        Side_5_PS.Stop();
                        Side_5_PS.Play();
                    }
                    break;
                case 6:
                    if (Side_6_Audio != null)
                    {
                        SoundBoi.Stop();
                        SoundBoi.clip = Side_6_Audio;
                        SoundBoi.Play();
                    }
                    if (Side_6_PS != null)
                    {
                        Side_6_PS.Stop();
                        Side_6_PS.Play();
                    }
                    break;
                default:
                    break;
            }
            ClearBoi();
        }
        public Slider ASlide;
        public InputField ASlideText;
        public void AudioSlider()
        {
            ASlideText.text = ASlide.value.ToString();
            SoundBoi.volume = ASlide.value / 100;
        }

        public void ClearBoi()
        {
            Debug.Log(OutputUwU.text);
            OutputUwU.text = "0";
            Debug.Log(OutputUwU.text);
        }
    }
}
