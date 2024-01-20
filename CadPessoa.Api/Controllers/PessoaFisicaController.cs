using CadPessoa.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CadPessoa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaFisicaController : ControllerBase
    {
        private readonly DataContext _data;

        private readonly IWebHostEnvironment _hostEnviroment;

        public PessoaFisicaController(DataContext data, IWebHostEnvironment IWebHostEnviroment)
        {
            _data = data;
            _hostEnviroment = IWebHostEnviroment;
        }

        [HttpPut("upload-image/{pessoaId}")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadImage(int pessoaId)
        {
            try
            {
                var pessoa = await _data.Pessoas.Where(p => p.Id == pessoaId).FirstOrDefaultAsync();

                var file = Request.Form.Files[0];
                Console.Write("|||||||||||||||||||||||||||||||||||||||||||||||");
                Console.WriteLine(file);

                if (file.Length > 0)
                {
                    DeleteImagem(pessoa.Image);
                    pessoa.Image = await SaveImage(file);
                }
                _data.Pessoas.Update(pessoa);
                _data.SaveChanges();
                //var ProdutoRetorno = await _produtoService.UpdateProduto(produtoId, produto);

                return Ok(pessoa);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $" {ex.Message}");
            }

        }

        [HttpPost("upload-image2")]
        [AllowAnonymous]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            try
            {
                string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                                 .Take(10)
                                                 .ToArray()
                                                 ).Replace(' ', '-');
                imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

                var imagePath = Path.Combine(_hostEnviroment.ContentRootPath, @"Resources/Images", imageName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                return imageName;
            }
            catch (Exception exeption)
            {
                return exeption.ToString();
            }
        }
        [NonAction]

        public void DeleteImagem(string imageName)
        {
            var imagePath = Path.Combine(_hostEnviroment.ContentRootPath, @"Resources/Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}
