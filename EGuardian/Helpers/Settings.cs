using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGuardian.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string access_token = "access_token";
        private const string idUsuario = "idUsuario";
        private const string idEmpresa = "idEmpresa";
        private const string nombreEmpresa = "nombreEmpresa";
        private const string username = "username";
        private const string expires_in = "expires_in";
        private const string authority = "authority";
        private static readonly string Predeterminado = string.Empty;
        private const string lastUpdate = "lastUpdate";
        private static readonly DateTime lastUpdateDefault = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Local);

        #endregion

        public static string session_access_token
        {
            get
            {
                return AppSettings.GetValueOrDefault(access_token, Predeterminado);
            }
            set
            {
                AppSettings.AddOrUpdateValue(access_token, value);
            }
        }

        public static string session_idUsuario
        {
            get
            {
                return AppSettings.GetValueOrDefault(idUsuario, Predeterminado);
            }
            set
            {
                AppSettings.AddOrUpdateValue(idUsuario, value);
            }
        }

        public static string session_idEmpresa
        {
            get
            {
                return AppSettings.GetValueOrDefault(idEmpresa, Predeterminado);
            }
            set
            {
                AppSettings.AddOrUpdateValue(idEmpresa, value);
            }
        }
        public static string session_nombreEmpresa
        {
            get
            {
                return AppSettings.GetValueOrDefault(nombreEmpresa, Predeterminado);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nombreEmpresa, value);
            }
        }

        public static string session_username
        {
            get
            {
                return AppSettings.GetValueOrDefault(username, Predeterminado);
            }
            set
            {
                AppSettings.AddOrUpdateValue(username, value);
            }
        }

        public static string session_expires_in
        {
            get
            {
                return AppSettings.GetValueOrDefault(expires_in, Predeterminado);
            }
            set
            {
                AppSettings.AddOrUpdateValue(expires_in, value);
            }
        }

        public static string session_authority
        {
            get
            {
                return AppSettings.GetValueOrDefault(authority, Predeterminado);
            }
            set
            {
                AppSettings.AddOrUpdateValue(authority, value);
            }
        }

        public static DateTime session_lastUpdate
        {
            get
            {
                return AppSettings.GetValueOrDefault(lastUpdate, lastUpdateDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(lastUpdate, value);
            }
        }
    }
}