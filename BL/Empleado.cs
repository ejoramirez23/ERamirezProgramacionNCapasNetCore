namespace BL
{
	public class Empleado
	{

		public static ML.Result GetAll()
		{

			ML.Result result = new ML.Result();

			try
			{
				using (DL.EramirezProgramacionNcapasNetCoreContext context = new DL.EramirezProgramacionNcapasNetCoreContext())
				{

					var EmpleadoLINQ = (from tableLINQ in context.Empleados
									select new
									{
										NumeroEmpleado = tableLINQ.NumeroEmpleado,
										RFC = tableLINQ.Rfc,
										Nombre = tableLINQ.Nombre,
										ApellidoPaterno = tableLINQ.ApellidoPaterno,
										ApellidoMaterno = tableLINQ.ApellidoMaterno,
										Email = tableLINQ.Emai,
										Telefono = tableLINQ.Telefono,
										FechaNacimiento = tableLINQ.FechaNacimiento,
										NSS = tableLINQ.Nss,
										FechaIngreso = tableLINQ.FechaIngreso,
										Foto = tableLINQ.Foto,
										IdEmpresa = tableLINQ.IdEmpresa
									}).ToList();

					result.Objects = new List<Object>();

					if (EmpleadoLINQ.Count > 0)
					{
                        foreach (var item in EmpleadoLINQ)
                        {
							ML.Empleado empleado = new ML.Empleado();

							empleado.NumeroEmpleado = item.NumeroEmpleado;
							empleado.Nombre = item.Nombre;
							empleado.ApellidoPaterno = item.ApellidoPaterno;	
							empleado.ApellidoMaterno = item.ApellidoMaterno;
							empleado.Email = item.Email;
							empleado.Telefono = item.Telefono;
							empleado.FechaNacimiento = item.FechaNacimiento;
							empleado.NSS = item.Telefono;
							empleado.FechaIngreso = item.FechaIngreso;

							byte[] empleadoFoto = item.Foto as byte[] ?? null;
							empleado.Foto = empleadoFoto;
							
							empleado.IdEmpresa = item.IdEmpresa;

							result.Objects.Add(empleado);
                        }
						
						result.Correct = true;
					}
					else
					{
						result.Correct = false;
						result.Message = "No se encontraron registros";
					}



				}

			}
			catch (Exception ex)
			{
				result.Correct = false;
				result.Ex = ex;
				result.Message = "Ocurrio un error: " + result.Ex.Message;
			}


			return result;

		}




		public static ML.Result Add(ML.Empleado empleado)
		{
			ML.Result result = new ML.Result();


			try
			{

				using (DL.EramirezProgramacionNcapasNetCoreContext context = new DL.EramirezProgramacionNcapasNetCoreContext())
				{

					DL.Empleado empleadoLINQ = new DL.Empleado();

					empleadoLINQ.NumeroEmpleado = empleado.NumeroEmpleado;
					empleadoLINQ.Nombre = empleado.Nombre;
					empleadoLINQ.ApellidoPaterno = empleado.ApellidoPaterno;
					empleadoLINQ.ApellidoMaterno = empleado.ApellidoMaterno;
					empleadoLINQ.Emai = empleado.Email;
					empleadoLINQ.Telefono = empleado.Telefono;
					empleadoLINQ.FechaNacimiento = empleado.FechaNacimiento;
                    empleadoLINQ.FechaIngreso = empleado.FechaIngreso;
					empleadoLINQ.Foto = empleado.Foto;
					empleadoLINQ.IdEmpresa = empleado.IdEmpresa;
					

					context.Empleados.Add(empleadoLINQ);

					int RowsAfected = context.SaveChanges();

					if (RowsAfected > 0)
					{
						result.Correct = true;
						result.Message = "Empleado agregado exitosamente";
					}
					else
					{
                        result.Correct = false;
                        result.Message = "No pudo ser agregado el empleado exitosamente";
                    }

				}




			}
			catch (Exception ex)
			{
				result.Correct = false;
				result.Ex = ex;
				result.Message = ex.Message;
			}

			return result;


		}






















	}
}