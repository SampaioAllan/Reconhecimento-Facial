using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Concessionaria.Lib.MinhasExceptions;
using Microsoft.AspNetCore.Mvc;

namespace ReconhecimentoFacial.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RekognitionController : ControllerBase
    {
        private readonly AmazonRekognitionClient _rekognitionClient;
        public RekognitionController(AmazonRekognitionClient rekognitionClient)
        {
            _rekognitionClient = rekognitionClient;
        }
        [HttpGet()]
        public async Task<IActionResult> AnalisarRosto(string nomeArquivo)
        {
          using (var stream = new MemoryStream())
          {
            var entrada = new DetectFacesRequest();
            var imagem = new Image();
            var s3Object = new S3Object()
            {
              Bucket = "registro-facial",
              Name = nomeArquivo
            };

            imagem.S3Object = s3Object;
            entrada.Image = imagem;
            entrada.Attributes = new List<string>(){"ALL"};            

            var resposta = await _rekognitionClient.DetectFacesAsync(entrada);
            try
            {
              if (resposta.FaceDetails.Count() == 1 & resposta.FaceDetails.First().Eyeglasses.Value == false)
                {
                  return Ok(resposta);
                }
            }
            catch (ValidacaoDeDados ex)
            {
                return BadRequest(ex.Message);
            }
          }
        }
    }
}