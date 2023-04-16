# DataDock Functions

This project contains a set of Azure Functions for managing file uploads, downloads, deletion, and renaming in Azure Blob Storage.

## Getting Started

To use these functions, you will need to deploy them to an Azure Function App with a connection to an Azure Blob Storage account.

### Prerequisites

- Visual Studio Code (or another IDE of your choice)
- An Azure subscription
- An Azure Blob Storage account
- Azure Functions Core Tools

### How to Run

1. Clone this repository to your local machine.
2. Open the project in Visual Studio Code or your preferred IDE.
3. Update the `local.settings.json` file with your Azure Blob Storage account connection string.
4. Open a terminal window and navigate to the project directory.
5. Run the following command to start the Azure Functions host:
```
func start
```

# DataDock Functions

This project contains a set of Azure Functions for managing file uploads, downloads, deletion, and renaming in Azure Blob Storage.

## Functions

### UploadFile

**HTTP Verb:** POST

**Endpoint:** `/api/upload`

This function allows you to upload a file to Azure Blob Storage by sending the file contents in the request body. The function creates a new blob with a unique name in the specified container.

### DownloadFile

**HTTP Verb:** GET

**Endpoint:** `/api/download/{fileName}`

This function allows you to download a file from Azure Blob Storage by specifying the name of the blob in the URL path. The function returns the contents of the blob as the response body.

### DeleteFile

**HTTP Verb:** DELETE

**Endpoint:** `/api/delete/{fileName}`

This function allows you to delete a file from Azure Blob Storage by specifying the name of the blob in the URL path.

### RenameFile

**HTTP Verb:** PUT

**Endpoint:** `/api/rename/{oldFileName}/{newFileName}`

This function allows you to rename a file in Azure Blob Storage by specifying the current name of the blob and the desired new name in the URL path. The function copies the contents of the old blob to a new blob with the new name, and then deletes the old blob.

Note that all endpoints in this project use the base URL of your Azure Function App followed by the endpoint path. For example, if your Azure Function App URL is `https://myfunctionapp.azurewebsites.net`, the URL for the `UploadFile` endpoint would be `https://myfunctionapp.azurewebsites.net/api/upload`.
