using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;

namespace AWS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagensController : ControllerBase
    {
        private readonly IAmazonS3 _amazonS3;
        private static readonly List<string> _extensoesImagem = 
        new List<string>(){"image/jpeg", "image/png"};
        public ImagensController(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }
        
        [HttpPost("bucket")]
        public async Task<IActionResult> CriarBucket(string nomeBucket){
            var resposta = await _amazonS3.PutBucketAsync(nomeBucket);
            return Ok(resposta);
        }
        [HttpPost]
        public async Task<IActionResult> CriarImagem(IFormFile Imagem)
        {
            if(!_extensoesImagem.Contains(Imagem.ContentType))
            return BadRequest("Formato Inválido!");
            using (var streamDaImagem = new MemoryStream())
            {
                await Imagem.CopyToAsync(streamDaImagem);

                var request = new PutObjectRequest();
                request.Key = "reconhecimento"+ Imagem.FileName;
                request.BucketName = "registro-facial";
                request.InputStream = streamDaImagem;

                var resposta = await _amazonS3.PutObjectAsync(request);
                return Ok(resposta);
            }

        }
        [HttpGet("bucket")]
        public async Task<IActionResult> ListarBuckets()
        {
            var resposta = await _amazonS3.ListBucketsAsync();

            return Ok(resposta.Buckets.Select(x => x.BucketName));
        }
        [HttpDelete]
        public async Task<IActionResult> DeletarImagem(string nomeArquivoNovoS3)
        {
            var resposta = await _amazonS3.DeleteObjectAsync("registro-facial", nomeArquivoNovoS3);
            return Ok(resposta);
        }
    }
}