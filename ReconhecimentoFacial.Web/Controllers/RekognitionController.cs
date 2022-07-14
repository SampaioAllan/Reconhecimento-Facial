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