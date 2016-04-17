using System.Collections.Generic;

namespace MVVMSample.Common
{
    public static class ViewModelManager
    {
        private static readonly Dictionary<string, ViewModelBase> Dic = new Dictionary<string, ViewModelBase>();

        public static void Add(string key, ViewModelBase value)
        {
            if (Dic.ContainsKey(key)) return;

            Dic.Add(key, value);
        }

        public static ViewModelBase GetByKey(string key)
        {
            if (!Dic.ContainsKey(key)) return null;

            ViewModelBase value;
            Dic.TryGetValue(key, out value);

            return value;
        }

    }
}
