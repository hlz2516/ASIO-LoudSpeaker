using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASIO_LoudSpeaker.Helpers
{
    public class RepeatCachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound _cachedSound;
        public WaveFormat WaveFormat => throw new NotImplementedException();

        public RepeatCachedSoundSampleProvider(CachedSound sound)
        {
            _cachedSound = sound;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
