using LeonardoSuarezCamiones.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace LeonardoSuarezCamiones.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CamionController : ControllerBase
    {

        private const int ESPACIO_SEGURIDAD = 30;

        [HttpGet]
        public IActionResult Get(PaquetesCamion paquetesCamion)
        {
            int espacioDisponible = paquetesCamion.TamanioCamion - ESPACIO_SEGURIDAD;
            List<Tuple<int, int, int>> combinaciones = new List<Tuple<int, int, int>>();


            for (int i = 0; i < paquetesCamion.LstPaquetes.Length - 1; i++)
            {
                for (int j = i + 1; j < paquetesCamion.LstPaquetes.Length; j++)
                {
                    if (paquetesCamion.LstPaquetes[i] > paquetesCamion.LstPaquetes[j])
                    {
                        combinaciones.Add(new Tuple<int, int, int>(paquetesCamion.LstPaquetes[i] + paquetesCamion.LstPaquetes[j], paquetesCamion.LstPaquetes[i], paquetesCamion.LstPaquetes[j]));
                    }
                    else
                    {
                        combinaciones.Add(new Tuple<int, int, int>(paquetesCamion.LstPaquetes[i] + paquetesCamion.LstPaquetes[j], paquetesCamion.LstPaquetes[j], paquetesCamion.LstPaquetes[i]));
                    }
                }
            }

            var coincidencias = combinaciones.Where(i => i.Item1 == espacioDisponible);

            if (coincidencias.Count() == 1)
            {
                return Content($"{{ [{coincidencias.First().Item2}, {coincidencias.First().Item3}] }}", "application/json");
            }
            else if (coincidencias.Count() > 1)
            {
                var paquete = coincidencias.OrderByDescending(i => i.Item2).First();
                return Content($"{{ [{paquete.Item2}, {paquete.Item3}] }}", "application/json");
            }
            else
            {
                //Se toman de combinaciones aquella cuya suma sea menos a la capacidad del camion.
                var paquetesQueCaben = combinaciones.Where(i => i.Item1 <= espacioDisponible).OrderByDescending(i => i.Item1).ThenBy(i => i.Item2);
                if (paquetesQueCaben.Count() > 0)
                {
                    return Content($"{{ [{paquetesQueCaben.First().Item2}, {paquetesQueCaben.First().Item3}] }}", "application/json");
                }
            }

            return Ok();
        }

        public class Respuesta
        {
            public int[] Paquetes { get; set; }

        }

    }
}
