using System;
using Xamarin.Forms;
using SQLite;
using System.Collections;
using System.Collections.Generic;
using EGuardian.Models.Empleados;

namespace EGuardian.Models.Eventos
{
    public class eventos
    {
        System.Globalization.CultureInfo globalizacion = new System.Globalization.CultureInfo("es-GT");
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int idUsuario { get; set; }
        public int idEvento { get; set; }
        public int idUserCreator { get; set; }
        public empleados capacitador 
        { 
            get
            {
                return App.Database.GetEmpleadoById(idUserCreator);
            }
        }
        public string asunto { get; set; }
        public string ubicacion { get; set; }
        public int estado { get; set; }
        public string duracion { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public DateTime Fecha
        {
            get
            {
                var timestamp = Convert.ToDouble(fechaInicio.Trim());
                var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)
                .AddMilliseconds((long)timestamp)
                 .ToLocalTime();
                return time;
            }
        }
        public string FechaInicio
        {
            get
            {
                var timestamp = Convert.ToDouble(fechaInicio.Trim());
                string time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)
                .AddMilliseconds((long)timestamp) // put your value here
                 .ToLocalTime().ToString("g");
                return time;
            }
        }
        public string FechaFin
        {
            get
            {
                var timestamp = Convert.ToDouble(fechaFin.Trim());
                string time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)
                .AddMilliseconds((long)timestamp) // put your value here
                 .ToLocalTime().ToString("g");
                return time;
            }
        }
        public string fechaCita => getSelectedDate(Convert.ToDateTime(FechaInicio));
        public string horaInicioCita => Convert.ToDateTime(FechaInicio).ToString(@"hh:mm");
        public string horaFinCita => Convert.ToDateTime(FechaFin).ToString(@"hh:mm");
        public string horaTiempoCita => getHoraTiempoCita(Convert.ToDateTime(FechaInicio));
        public string horaTiempoCitaFinal => getHoraTiempoCita(Convert.ToDateTime(FechaFin));
        public string horarioCita
        {
            get
            {
                return horaInicioCita + " " + horaTiempoCita + " - " + horaFinCita + " " + horaTiempoCitaFinal;
            }
        }
        public Color estadoColor
        {
            get
            {
                switch (estado)
                {
                    case 1:
                        return Color.FromHex("33007F");
                    case 2:
                        return Color.FromHex("0067E7");
                    case 3:
                        return Color.FromHex("FFC000");
                    case 4:
                        return Color.FromHex("D0542F");
                    default:
                        return Color.FromHex("33007F");
                }
            }
        }
        public string estadoCita
        {
            get
            {
                switch (estado)
                {
                    case 1:
                        return "EN ESPERA";
                    case 2:
                        return "EN REUNIÓN";
                    case 3:
                        return "CANCELADO";
                    case 4:
                        return "FINALIZADO";
                    default:
                        return "EN ESPERA";
                }
            }
        }

        public IEnumerable Asistentes
        {
            get
            {
                List<asistentes> asistentesEvento = new List<asistentes>();

                asistentesEvento.Add(new asistentes
                {
                    nombres ="José Joel",
                    apellidos = "Hernández Gomez",
                    puesto ="Área de ventas",
                    genero = "0",
                    email = "hernandezjoel@gmail.com",
                    rol="Capacitador"
                });

                asistentesEvento.Add(new asistentes
                {
                    nombres = "María Pamela",
                    apellidos = "Chú Gaspar",
                    genero = "1",
                    email = "chupamela@gmail.com",
                    puesto = "Área de ventas",
                    rol = "Asistente"
                });

                asistentesEvento.Add(new asistentes
                {
                    nombres = "Ana Gabriela",
                    apellidos = "Marroquin Valdez",
                    genero="1",
                    email="marroqiungabriela@gmail.com",
                    puesto = "Área de ventas",
                    rol = "Asistente"
                });

                asistentesEvento.Add(new asistentes
                {
                    nombres = "Carlos Eduardo",
                    apellidos = "Veliz López",
                    genero = "0",
                    email = "velizeduardo@gmail.com",
                    puesto = "Área de ventas",
                    rol = "Asistente"
                });

                asistentesEvento.Add(new asistentes
                {
                    nombres = "Heck",
                    apellidos = "Ramirez C.",
                    genero = "0",
                    email = "ramirezcheck@gmail.com",
                    puesto = "Área de ventas",
                    rol = "Asistente"
                });

                return asistentesEvento;
            }
        }

        private string getHoraTiempoCita(DateTime Fecha)
        {
            var horatiempo = Fecha.ToString(@"tt", globalizacion);
            return horatiempo.ToLower();
        }
        private String getSelectedDate(DateTime fechaInicio)
        {
            var currentDate = char.ToUpper(globalizacion.DateTimeFormat.GetDayName(fechaInicio.DayOfWeek).ToString()[0]) + globalizacion.DateTimeFormat.GetDayName(fechaInicio.DayOfWeek).ToString().Substring(1) + " " + Convert.ToDateTime(fechaInicio).Day + " de " + globalizacion.DateTimeFormat.GetMonthName(fechaInicio.Month);
            return currentDate;
        }
    }
}