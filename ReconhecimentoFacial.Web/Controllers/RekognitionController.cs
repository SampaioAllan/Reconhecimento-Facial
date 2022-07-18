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
    [HttpPost("comparar")]
    public async Task<IActionResult> CompararRosto(string nomeArquivoS3, IFormFile fotoLogin)
    {
      using (var memoryStream = new MemoryStream())
      {
        var request = new CompareFacesRequest();      
        var requestSource = new Image()
        {
          S3Object = new S3Object()
          {
            Bucket = "registro-facial",
            Name = nomeArquivoS3
          }
        };


        await fotoLogin.CopyToAsync(memoryStream);
        var requestTarget = new Image()
        {
          Bytes = memoryStream
        };

        request.SourceImage = requestSource;
        request.TargetImage = requestTarget;

        var response = await _rekognitionClient.CompareFacesAsync(request);
        return Ok(response);
      }
    }
    [HttpGet("Analisar")]
    public async Task<IActionResult> AnalisarRosto(string nomeArquivo)
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

        if (resposta.FaceDetails.Count() != 0)
        {
          if (resposta.FaceDetails.Count() == 1)
          {
            if (resposta.FaceDetails.First().Eyeglasses.Value == false)
              return Ok(resposta);
            else
              throw new ValidacaoDeDados("Usuário está usando óculos"); 
          }
          else
            throw new ValidacaoDeDados("A imagem contém mais de um rosto");
        }
        else
          throw new ValidacaoDeDados("A imagem não contém rosto");                 
    }
  }
}