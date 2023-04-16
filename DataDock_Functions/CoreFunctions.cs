using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DataDock_Functions;

public static class CoreFunctions
{
	[FunctionName("UploadFile")]
	public static async Task<IActionResult> UploadFile(
		[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "upload")] HttpRequest req,
		[Blob("data-dock/{rand-guid}", FileAccess.Write)] CloudBlockBlob outputBlob,
		ILogger log)
	{
		try
		{
			await using (var stream = req.Body)
			{
				await outputBlob.UploadFromStreamAsync(stream);
			}
			log.LogInformation($"File uploaded to DataDock with name {outputBlob.Name}");
			return new OkResult();
		}
		catch (Exception ex)
		{
			log.LogError(ex, "Error uploading file to DataDock");
			return new StatusCodeResult(500);
		}
	}


	[FunctionName("DownloadFile")]
	public static Task<IActionResult> DownloadFile(
		[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "download/{fileName}")] HttpRequest req,
		[Blob("data-dock/{fileName}", FileAccess.Read)] Stream inputBlob,
		string fileName,
		ILogger log)
	{
		try
		{
			var contentType = "application/octet-stream";
			return Task.FromResult<IActionResult>(new FileStreamResult(inputBlob, contentType)
			{
				FileDownloadName = fileName
			});
		}
		catch (Exception ex)
		{
			log.LogError(ex, $"Error downloading file {fileName} from DataDock");
			return Task.FromResult<IActionResult>(new StatusCodeResult(500));
		}
	}

	[FunctionName("DeleteFile")]
	public static async Task<IActionResult> DeleteFile(
		[HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "delete/{fileName}")] HttpRequest req,
		[Blob("data-dock/{fileName}", FileAccess.ReadWrite)] CloudBlockBlob blob,
		string fileName,
		ILogger log)
	{
		try
		{
			await blob.DeleteAsync();
			log.LogInformation($"File {fileName} deleted from DataDock");
			return new OkResult();
		}
		catch (Exception ex)
		{
			log.LogError(ex, $"Error deleting file {fileName} from DataDock");
			return new StatusCodeResult(500);
		}
	}


	[FunctionName("RenameFile")]
	public static async Task<IActionResult> RenameFile(
		[HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "rename/{oldFileName}/{newFileName}")] HttpRequest req,
		[Blob("data-dock/{oldFileName}", FileAccess.ReadWrite)] CloudBlockBlob inputBlob,
		[Blob("data-dock/{newFileName}", FileAccess.Write)] CloudBlockBlob outputBlob,
		string oldFileName,
		string newFileName,
		ILogger log)
	{
		try
		{
			await outputBlob.StartCopyAsync(inputBlob);
			await inputBlob.DeleteAsync();
			log.LogInformation($"File {oldFileName} renamed to {newFileName} in DataDock");
			return new OkResult();
		}
		catch (Exception ex)
		{
			log.LogError(ex, $"Error renaming file {oldFileName} to {newFileName} in DataDock");
			return new StatusCodeResult(500);
		}
	}

}