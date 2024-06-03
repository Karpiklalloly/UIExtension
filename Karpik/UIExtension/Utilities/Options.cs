using System;

namespace Karpik.UIExtension
{
    public static class Options
    {

        private static EnvironmentType _environment = EnvironmentType.Runtime;

        public static EnvironmentType Environment
        {
            get => _environment;
            set
            {
                _environment = value;
                EnvironmentChanged?.Invoke(_environment);
            }
        }

        public static event Action<EnvironmentType> EnvironmentChanged;
    }
}