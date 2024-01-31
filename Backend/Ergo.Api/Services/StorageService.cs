using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Ergo.Api.Models.AwsS3;

namespace Ergo.Api.Services
{
    public class StorageService : IStorageService
    {
        public Task<bool> DeleteFileAsync(string objectName, AWSCredential aWSCredentials)
        {
            var credentials = new BasicAWSCredentials(aWSCredentials.AwsKey, aWSCredentials.AwsSecretKey);
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUCentral1
            };
            try
            {
                using var client = new AmazonS3Client(credentials, config);
                var deleteObjectRequest = new Amazon.S3.Model.DeleteObjectRequest()
                {
                    BucketName = "ergo-project",
                    Key = objectName
                };
                client.DeleteObjectAsync(deleteObjectRequest);
                return Task.FromResult(true);
            }
            catch (AmazonS3Exception ex)
            {
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }

        public async Task<Models.AwsS3.S3ResponseDto> UploadFileAsync(S3Object s3Object, AWSCredential aWSCredentials)
        {
            var credentials = new BasicAWSCredentials(aWSCredentials.AwsKey, aWSCredentials.AwsSecretKey);

            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUCentral1
            };
            var response = new S3ResponseDto();

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = s3Object.InputStream,
                    Key = s3Object.Name,
                    BucketName = s3Object.BucketName,
                    CannedACL = S3CannedACL.PublicRead
                };
                using var client = new AmazonS3Client(credentials, config);
                var transferUtility = new TransferUtility(client);
                await transferUtility.UploadAsync(uploadRequest);
                response.StatusCode = 200;
                response.Message = $"{s3Object.Name} uploaded successfully";

            }catch(AmazonS3Exception ex)
            {
                response.StatusCode = (int)ex.StatusCode;
                response.Message = ex.Message;
            }catch(Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

    }
}
