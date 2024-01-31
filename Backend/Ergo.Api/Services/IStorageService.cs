using Ergo.Api.Models.AwsS3;
using Ergo.Domain.Common;

namespace Ergo.Api.Services
{
    public interface IStorageService
    {
        Task<S3ResponseDto> UploadFileAsync(S3Object s3Object, AWSCredential aWSCredentials);
        Task<bool> DeleteFileAsync(string objectName, AWSCredential aWSCredentials);

    }
}
