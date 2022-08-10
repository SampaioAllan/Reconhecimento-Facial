using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace ReconhecimentoFacial.Services
{
    public class AwsServices
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly AmazonRekognitionClient _rekognitionClient;
        private static readonly List<string> _extensoesImagem =
        new List<string>() { "image/jpeg", "image/png", "image/jpg" };
        public AwsServices(IAmazonS3 amazonS3, AmazonRekognitionClient rekognitionClient)
        {
            _amazonS3 = amazonS3;
            _rekognitionClient = rekognitionClient;
        }
        public async Task<string> SalvarNoS3(IFormFile imagem)
        {
            if (!_extensoesImagem.Contains(imagem.ContentType))
                throw new Exception("Formato Inválido!");
            using (var streamDaImagem = new MemoryStream())
            {
                await imagem.CopyToAsync(streamDaImagem);

                var request = new PutObjectRequest();
                request.Key = "reconhecimento" + imagem.FileName;
                request.BucketName = "registro-facial";
                request.InputStream = streamDaImagem;

                var resposta = await _amazonS3.PutObjectAsync(request);
                return request.Key;
            }
        }
        public async Task DeletarNoS3(string nomeArquivo)
        {
            await _amazonS3.DeleteObjectAsync("registro-facial", nomeArquivo);
        }
        public async Task<bool> ValidarImagem(string nomeArquivo)
        {

            var entrada = new DetectFacesRequest();
            var imagem = new Image();
            var s3Object = new Amazon.Rekognition.Model.S3Object()
            {
                Bucket = "registro-facial",
                Name = nomeArquivo
            };

            imagem.S3Object = s3Object;
            entrada.Image = imagem;
            entrada.Attributes = new List<string>() { "ALL" };

            var resposta = await _rekognitionClient.DetectFacesAsync(entrada);

            if (resposta.FaceDetails.Count() == 1 && resposta.FaceDetails.First().Eyeglasses.Value == false)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> CompararImagem(string nomeArquivoS3, IFormFile fotoLogin)
        {
            using (var memoryStream = new MemoryStream())
            {
                var request = new CompareFacesRequest();
                var requestSource = new Image()
                {
                    S3Object = new Amazon.Rekognition.Model.S3Object()
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
                if (response.FaceMatches.Count == 1 && response.FaceMatches.First().Similarity >= 80)
                {
                    return true;
                }
                return false;
            }
        }
    }
}