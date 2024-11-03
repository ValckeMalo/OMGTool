namespace MaloProduction.Tween.Plugin
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class PluginsManager
    {
        private static IPlugin floatPlugin;
        private static IPlugin vector2Plugin;

        private static Dictionary<Type, IPlugin> customPlugin;
        private const int maxCustomPlugin = 20;

        public static ABSPlugin<T1, T2> GetDefaultPlugin<T1, T2>()
        {
            Type type1 = typeof(T1);
            Type type2 = typeof(T2);
            IPlugin plugin = null;

            if (type1 == typeof(float))
            {
                if (floatPlugin == null)
                {
                    floatPlugin = new FloatPlugin();
                }
                plugin = floatPlugin;
            }
            else if (type1 == typeof(Vector2))
            {
                if (vector2Plugin == null)
                {
                    vector2Plugin = new Vector2Plugin();
                }
                plugin = vector2Plugin;
            }

            if (plugin != null)
            {
                return plugin as ABSPlugin<T1, T2>;
            }

            return null;
        }

        public static ABSPlugin<T1, T2> GetCustomPlugin<TPLugin, T1, T2>() where TPLugin : IPlugin, new()
        {
            Type pluginType = typeof(TPLugin);
            IPlugin plugin = null;

            if (customPlugin == null)
            {
                customPlugin = new Dictionary<Type, IPlugin>(maxCustomPlugin);
            }
            else if (customPlugin.TryGetValue(pluginType, out plugin))
            {
                return plugin as ABSPlugin<T1, T2>;
            }

            plugin = new TPLugin();
            customPlugin.Add(pluginType, plugin);
            return plugin as ABSPlugin<T1, T2>;
        }
    }
}