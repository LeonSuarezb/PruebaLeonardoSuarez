using LeonardoSuarezCasas.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LeonardoSuarezCasas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Ejercicio1Controller : ControllerBase
    {

        [HttpGet]
        public IActionResult Get(EstadoCasas estadoCasas)
        {
            if (!isInputValid(estadoCasas))
            {
                return BadRequest();
            }

            EstadoCasasActualizado estadoCasasActualizado = new EstadoCasasActualizado()
            {
                Dias = estadoCasas.Dias,
                Entrada = estadoCasas.LstCasas,
                Salida = (int[])estadoCasas.LstCasas.Clone()
            };

            for (int dia = 0; dia < estadoCasas.Dias; dia++)
            {
                int[] datosOriginales =  (int[])(estadoCasasActualizado.Salida.Clone());

                for (int i = 0; i < datosOriginales.Length; i++)
                {
                    int vecinoIzquierda = i == 0 ? 0 : datosOriginales[i - 1];
                    int vecinoDerecha = i == datosOriginales.Length - 1 ? 0 : datosOriginales[i + 1];

                    estadoCasasActualizado.Salida[i] = CalcularNuevoEstado(vecinoIzquierda, vecinoDerecha);
                }
            }

            return Ok(estadoCasasActualizado);
        }

        private int CalcularNuevoEstado(int vecinoIzquierda, int vecinoDerecha)
        {
            return vecinoIzquierda.Equals(vecinoDerecha) ? 0 : 1;
        }

        private bool isInputValid(EstadoCasas estadoCasas)
        {
            if (estadoCasas == null || estadoCasas.LstCasas == null || estadoCasas.Dias <= 0)
            {
                return false;
            }

            if (estadoCasas.LstCasas.Any(i => i != 0 && i != 1))
            {
                return false;
            }


            return true;
        }

    }
}
