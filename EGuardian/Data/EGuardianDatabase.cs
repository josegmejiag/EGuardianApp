using EGuardian.Helpers;
using EGuardian.Interfaces;
using EGuardian.Models.Acciones;
using EGuardian.Models.Aplicaciones;
using EGuardian.Models.Empleados;
using EGuardian.Models.Empresas;
using EGuardian.Models.Eventos;
using EGuardian.Models.Puestos;
using EGuardian.Models.SectorNegocio;
using EGuardian.Models.Usuarios;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace EGuardian.Data
{
    public class EGuardianDatabase
    {
        static object locker = new object();
        SQLiteConnection database;
        public EGuardianDatabase()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();

            //Modulo Acceso
            database.CreateTable<usuarios>();
            database.CreateTable<empresas>();
            database.CreateTable<puestos>();
            database.CreateTable<sectores>();

            //Modulo Eventos
            database.CreateTable<eventos>();

            //Modulo Perfil
            database.CreateTable<empleados>();
            
            //Modulo Configuración
            database.CreateTable<aplicaciones>();
            database.CreateTable<acciones>();
        }

        public usuarios GetUser(string email)
        {
            lock (locker)
            {
                return database.Table<usuarios>().FirstOrDefault(x => x.email == email);
            }
        }

        public usuarios GetUserById(int id)
        {
            lock (locker)
            {
                return database.Table<usuarios>().FirstOrDefault(x => x.id == id);
            }
        }

        public empleados GetEmpleadoById(int id)
        {
            lock (locker)
            {
                return database.Table<empleados>().FirstOrDefault(x => x.idUsuario == id);
            }
        }

        public IEnumerable<empleados> GetEmpleados(int id)
        {
            lock (locker)
            {
                return database.Table<empleados>().Where(x => x.idUsuario == id);
            }
        }

        public IEnumerable<eventos> GetEventos(int id)
        {
            lock (locker)
            {
                return database.Table<eventos>().Where(x => x.idUsuario == id);
            }
        }

        public empresas GetEmpresa(int idEmpresa)
        {
            lock (locker)
            {
                return database.Table<empresas>().FirstOrDefault(x => x.idEmpresa == idEmpresa);
            }
        }

        public IEnumerable<puestos> GetPuestos()
        {
            lock (locker)
            {
                return database.Table<puestos>().ToList().OrderBy((i) => i.id);
            }
        }

        public IEnumerable<puestos> GetPuesto()
        {
            lock (locker)
            {
                return database.Query<puestos>("SELECT * FROM puestos ORDER BY RANDOM() LIMIT 1;");
            }
        }

        public IEnumerable<sectores> GetSectores()
        {
            lock (locker)
            {
                return database.Table<sectores>().ToList().OrderBy((i) => i.id);
            }
        }

        public IEnumerable<aplicaciones> GetAplicaciones()
        {
            lock (locker)
            {
                return database.Table<aplicaciones>().ToList().OrderBy((i) => i.idAplicacion);
            }
        }

        public IEnumerable<acciones> GetAcciones()
        {
            lock (locker)
            {
                return database.Table<acciones>().ToList().OrderBy((i) => i.idAccion);
            }
        }


        //Insesrción de datos
        public int InsertUsuario(usuarios usuario)
        {
            lock (locker)
            {
                if (usuario.id != 0)
                {
                    database.Update(usuario);
                    return usuario.id;
                }
                else
                {
                    return database.Insert(usuario);
                }
            }
        }

        public int InsertEmpleado(empleados empleado)
        {
            lock (locker)
            {
                if (empleado.id != 0)
                {
                    database.Update(empleado);
                    return empleado.id;
                }
                else
                {
                    return database.Insert(empleado);
                }
            }
        }

        public int InsertEmpresa(empresas empresa)
        {
            lock (locker)
            {
                if (empresa.id != 0)
                {
                    database.Update(empresa);
                    return empresa.id;
                }
                else
                {
                    return database.Insert(empresa);
                }
            }
        }

        public int InsertPuesto(puestos puesto)
        {
            lock (locker)
            {
                return database.Insert(puesto);
            }
        }

        public int InsertSector(sectores sector)
        {
            lock (locker)
            {
                return database.Insert(sector);
            }
        }

        public int InsertAplicacion(aplicaciones aplicacion)
        {
            lock (locker)
            {
                return database.Insert(aplicacion);
            }
        }

        public int InsertAccion(acciones accion)
        {
            lock (locker)
            {
                return database.Insert(accion);
            }
        }

        public int InsertEvento(eventos evento)
        {
            lock (locker)
            {
                if (evento.id != 0)
                {
                    database.Update(evento);
                    return evento.id;
                }
                else
                {
                    return database.Insert(evento);

                }
            }
        }

        //Eliminación de datos
        public void DeleteAllPuestos()
        {
            lock (locker)
            {
                try
                {
                    database.Query<puestos>("DELETE FROM puestos;");
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        public void DeleteAllSectores()
        {
            lock (locker)
            {
                try
                {
                    database.Query<sectores>("DELETE FROM sectores;");
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        public void DeleteAllEmpleados()
        {
            lock (locker)
            {
                try
                {
                    database.Query<empleados>("DELETE FROM empleados WHERE idUsuario=?;",Convert.ToInt32(Settings.session_idUsuario));
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        public void DeleteAllEventos()
        {
            lock (locker)
            {
                try
                {
                    database.Query<eventos>("DELETE FROM eventos WHERE idUsuario=?;", Convert.ToInt32(Settings.session_idUsuario));
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
