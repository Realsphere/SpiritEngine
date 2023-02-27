using System;
using System.Collections.Generic;
using BulletSharp.SoftBody;
using libsndfile.NET;
using Realsphere.Spirit.Mathematics;
using static OpenAL.AL10;
using static OpenAL.ALC10;

namespace Realsphere.Spirit
{
    /// <summary>
    /// Spirit's way of interacting with the OpenAL implementation.
    /// </summary>
    public class AudioSource : IDisposable
    {
        internal audiosource src;

        /// <summary>
        /// Volume of the Audio Source.
        /// </summary>
        public float Volume
        {
            get
            {
                return src.vol;
            }
            set
            {
                src.vol = value;
            }
        }

        /// <summary>
        /// Pitch of the Audio Source.
        /// </summary>
        public float Pitch
        {
            get
            {
                return src.pitch;
            }
            set
            {
                src.pitch = value;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromSeconds(src.secs);
            }
        }

        /// <summary>
        /// Position of the Audio Source.
        /// </summary>
        public SVector3 Position
        {
            get
            {
                return src.pos;
            }
            set
            {
                src.pos = value;
            }
        }

        /// <summary>
        /// Checks if the Audio Source is playing.
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return src.playing;
            }
        }

        /// <summary>
        /// Doppler Velocity of the Audio Source.
        /// </summary>
        public SVector3 Velocity
        {
            get
            {
                return src.vel;
            }
            set
            {
                src.vel = value;
            }
        }

        public AudioSource(string filename)
        {
            src = new(filename);
        }

        /// <summary>
        /// Play / Resume the Audio Source.
        /// </summary>
        public void Play()
        {
            src.play();
        }

        /// <summary>
        /// Play / Resume the Audio Source, but its looping.
        /// </summary>
        public void PlayLooping()
        {
            src.playloop();
        }

        /// <summary>
        /// Stop the Audio Source.
        /// </summary>
        public void Stop() { src.stop(); }

        /// <summary>
        /// Pause the Audio Source.
        /// </summary>
        public void Pause() { src.pause(); }

        /// <summary>
        /// Clears the OpenAL Audio source and buffer.
        /// </summary>
        public void Dispose()
        {
            src.stop();
            src.dispose();
        }
    }

    internal class audiosource
    {
        uint sourceID;
        internal string filename;
        internal float secs = 0;

        public float vol
        {
            get
            {
                float val = 0f;
                alGetSourcef(sourceID, AL_GAIN, out val);
                return val;
            }
            set
            {
                alSourcef(sourceID, AL_GAIN, value);
            }
        }

        public bool playing
        {
            get
            {
                alGetSourcei(sourceID, AL_SOURCE_STATE, out int state);
                return state == AL_PLAYING;
            }
        }

        public float pitch
        {
            get
            {
                float val = 0f;
                alGetSourcef(sourceID, AL_PITCH, out val);
                return val;
            }
            set
            {
                alSourcef(sourceID, AL_PITCH, value);
            }
        }

        public SVector3 pos
        {
            get
            {
                alGetSource3f(sourceID, AL_POSITION, out float x, out float y, out float z);
                return new(x, y, z);
            }
            set
            {
                alSource3f(sourceID, AL_POSITION, value.X, value.Y, value.Z);
            }
        }

        public SVector3 vel
        {
            get
            {
                alGetSource3f(sourceID, AL_VELOCITY, out float x, out float y, out float z);
                return new(x, y, z);
            }
            set
            {
                alSource3f(sourceID, AL_VELOCITY, value.X, value.Y, value.Z);
            }
        }
        uint buffer;

        public audiosource(string file)
        {
            filename = file;
            alGenSources(1, out sourceID);
            alSourcef(sourceID, AL_GAIN, 1f);
            alSourcef(sourceID, AL_PITCH, 1f);
            alSource3f(sourceID, AL_POSITION, 0f, 0f, 0f);
            var res = AudioMaster.loadSound(filename);
            buffer = res.Item1;
            secs = res.Item2;
        }

        public void play()
        {
            alSourcei(sourceID, AL_BUFFER, (int)buffer);
            alSourcei(sourceID, AL_LOOPING, 0);
            alSourcePlay(sourceID);
        }

        public void playloop()
        {
            alSourcei(sourceID, AL_BUFFER, (int)buffer);
            alSourcei(sourceID, AL_LOOPING, 1);
            alSourcePlay(sourceID);
        }

        internal void stop()
        {
            alSourceStop(sourceID);
        }

        internal void pause()
        {
            alSourcePause(sourceID);
        }

        public void dispose()
        {
            alDeleteBuffers(1, ref sourceID);
        }
    }

    internal class AudioMaster
    {
        static List<uint> buffers = new();
        static IntPtr device;
        static IntPtr context;

        public static void init()
        {
            Logger.Log("Initializing OpenAL...", LogLevel.Information);
            device = alcOpenDevice(null);
            context = alcCreateContext(device, null);
            alcMakeContextCurrent(context);
            Logger.Log("Finished Initializing OpenAL!", LogLevel.Information);
        }

        public static Tuple<uint, float> loadSound(string file)
        {
            uint buffer = 0;
            alGenBuffers(1, out buffer);
            var wav = SndFile.OpenRead(file);
            short[] waveData = new short[wav.Frames];
            wav.ReadFrames(waveData, wav.Frames);
            float ms = (float)wav.Frames / (float)wav.Format.SampleRate;

            var format = AL_NONE;
            if (wav.Format.Channels == 1)
                format = AL_FORMAT_MONO16;
            else if (wav.Format.Channels == 2)
                format = AL_FORMAT_STEREO16;

            alBufferData(buffer, format, waveData, (int)wav.Frames * wav.Format.Channels * sizeof(short), wav.Format.SampleRate);

            buffers.Add(buffer);

            wav.Dispose();
            return new(buffer, ms);
        }

        public static void setListenerData()
        {
            alListener3f(AL_POSITION, Game.Player.PlayerPosition.X, Game.Player.PlayerPosition.Y, Game.Player.PlayerPosition.Z);
            alListener3f(AL_VELOCITY, 0, 0, 0);

            // Orientation
            // Layout:
            //  Cam Forward
            //  Cam upward
            List<float> buffer = new List<float>();
            buffer.Add(Game.Player.CameraForward.X);
            buffer.Add(Game.Player.CameraForward.Y);
            buffer.Add(Game.Player.CameraForward.Z);
            buffer.Add(Game.Player.CameraUp.X);
            buffer.Add(Game.Player.CameraUp.Y);
            buffer.Add(Game.Player.CameraUp.Z);
            alListenerfv(AL_ORIENTATION, buffer.ToArray());
        }

        public static void cleanup()
        {
            Logger.Log("Disposing OpenAL...", LogLevel.Information);
            foreach (uint buffer in buffers)
                alDeleteBuffers(1, new uint[] { buffer });
            alcDestroyContext(context);
            alcCloseDevice(device);
            Logger.Log("Finished Disposing OpenAL!", LogLevel.Information);
        }
    }
}
