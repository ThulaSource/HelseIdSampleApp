using StructureMap;

namespace HelseIdSampleApp.StructureMap
{
    public static class Bootstrapper
    {
        private static bool initialized;

        public static T GetInstance<T>()
        {
            if (!initialized)
            {
                Init();
            }

            return ObjectFactory.GetInstance<T>();
        }

        private static void Init()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<HelseIdSampleAppRegistry>();
            });
            initialized = true;
        }
    }
}