using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASIO_LoudSpeaker.Helpers
{
    public abstract class UniqueNameSampleProvider : ISampleProvider
    {
        private static readonly HashSet<string> _uniqueNames = new HashSet<string>();
        private static readonly object _lockObj = new object();
        private static int _globalIndex = 0;
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(nameof(value));

                lock (_lockObj)  // 加锁保证线程安全
                {
                    if (_uniqueNames.Contains(value))
                        return;

                    // 移除旧名称(如果存在)
                    if (_name != null)
                    {
                        _uniqueNames.Remove(_name);
                    }

                    _name = value;
                    _uniqueNames.Add(value);
                }
            }
        }

        protected UniqueNameSampleProvider()
        {
            lock (_lockObj)
            {
                string defaultName;
                do
                {
                    _globalIndex++;
                    defaultName = $"default{_globalIndex}";
                }
                while (_uniqueNames.Contains(defaultName)); // 确保生成的名称唯一

                _name = defaultName;
                _uniqueNames.Add(defaultName);
            }
        }

        ~UniqueNameSampleProvider()
        {
            lock (_lockObj)
            {
                if (_name != null)
                {
                    _uniqueNames.Remove(_name);
                }
            }
        }

        public abstract WaveFormat WaveFormat { get; }
        public abstract int Read(float[] buffer, int offset, int count);
    }
}
