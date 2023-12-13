using Microsoft.AspNetCore.Mvc;

namespace PLIdentity.Controllers
{
	public class EmpleadoController : Controller
	{
		[HttpGet]	
		public IActionResult GetAll()
		{
			ML.Result result = BL.Empleado.GetAll();

			ML.Empleado empleado = new ML.Empleado();
			empleado.Empleados = result.Objects;

			return View(empleado);
		}

		[HttpGet]
		public IActionResult Form(string numeroEmpleado)
		{

            ML.Empleado empleado = new ML.Empleado();

			if (numeroEmpleado == null)
			{

				@ViewBag.Accion = "Agregar";

			}
			else
			{
				@ViewBag.Accion = "Actualizar";
			}

                return View(empleado);
        }


        [HttpPost]
        public IActionResult Form(ML.Empleado empleado, IFormFile fuFoto)
        {

            if (fuFoto != null)
            {

                empleado.Foto = convertFileToByteArray(fuFoto);

            }


            if (empleado.NumeroEmpleado == null)
            {

                @ViewBag.Accion = "Agregar";

                DateTime dateAndTiem = new DateTime();

                dateAndTiem.ToString("ddMMyyHHmmss"); 

                string firstLetterName = empleado.Nombre.Trim().ToUpper().Substring(0,1);
                string firstLetterApellidoPaterno = empleado.ApellidoPaterno.Trim().ToUpper().Substring(0,1);
                string firstLetterApellidoMaterno = empleado.ApellidoMaterno.Trim().ToUpper().Substring(0,1);
                string birthdate = empleado.FechaNacimiento.ToString();

                birthdate = birthdate.Replace("/", "");




                string numeroEmpleado = firstLetterName + firstLetterApellidoPaterno + firstLetterApellidoMaterno + birthdate;

                empleado.NumeroEmpleado = numeroEmpleado;

                empleado.FechaNacimiento = DateTime.ParseExact(empleado.FechaNacimiento.ToString(),"dd-MM-yyyy",System.Globalization.CultureInfo.InvariantCulture);

                ML.Result result = BL.Empleado.Add(empleado);


                return View("Modal");

            }
            else
            {
                @ViewBag.Accion = "Actualizar";
            }

            return View(empleado);
        }



        public byte[] convertFileToByteArray(IFormFile Foto)
        {

            MemoryStream target = new MemoryStream();
            Foto.CopyTo(target);
            byte[] data = target.ToArray();

            return data;
        }


    }
}
