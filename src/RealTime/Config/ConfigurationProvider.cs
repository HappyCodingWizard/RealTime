﻿// <copyright file="ConfigurationProvider.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace RealTime.Config
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using RealTime.Tools;

    internal static class ConfigurationProvider
    {
        private static readonly string SettingsFileName = typeof(ConfigurationProvider).Assembly.GetName().Name + ".xml";

        public static Configuration LoadConfiguration()
        {
            try
            {
                if (!File.Exists(SettingsFileName))
                {
                    return new Configuration();
                }

                return Deserialize();
            }
            catch
            {
                return new Configuration();
            }
        }

        public static void SaveConfiguration(Configuration config)
        {
            try
            {
                Serialize(config);
            }
            catch (Exception ex)
            {
                Log.Error("The 'Real Time' mod cannot save its configuration, error message: " + ex.Message);
            }
        }

        private static Configuration Deserialize()
        {
            var serializer = new XmlSerializer(typeof(Configuration));
            using (var sr = new StreamReader(SettingsFileName))
            {
                return (Configuration)serializer.Deserialize(sr);
            }
        }

        private static void Serialize(Configuration config)
        {
            var serializer = new XmlSerializer(typeof(Configuration));
            using (var sw = new StreamWriter(SettingsFileName))
            {
                serializer.Serialize(sw, config);
            }
        }
    }
}
