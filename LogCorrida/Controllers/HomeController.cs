using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using LogCorrida.Utility;

namespace LogCorrida.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IFormFile log)
        {
            string fileContent;

            if (log != null && log.Length > 0)
            {
                if (log.FileName.EndsWith(".csv"))
                {
                    using (var reader = new StreamReader(log.OpenReadStream()))
                    {
                        fileContent = await reader.ReadToEndAsync();
                    }

                    try
                    {
                        return View((new Classificar(fileContent)).ObterClassificacao());
                    }
                    catch
                    {
                        ModelState.AddModelError("Arquivo", "O arquivo não corresponde como sendo de \"Log de Corrida\"");
                    }
                }
                else
                {
                    ModelState.AddModelError("Arquivo", "Formato do arquivo deve ser .csv");
                }
            }
            else
            {
                ModelState.AddModelError("Arquivo", "Selecione um arquivo para fazer o upload");
            }

            return View();
        }

        public IActionResult Sobre()
        {
            return View();
        }
    }
}
