# shelf-layout-manager

Code solution for both the Backend assignment and Frontend assignment from Telexistence.

The backend is made with .NET 8, ASP.NET Core Web API, and EF Core 8.
The frontend is a Blazor WebAssembly App.

## Setup
These are the setup instructions to *locally* run both the backend and frontend.

The only thing required to setup the backend is to install postgres and create a database, user, and grant them permissions.


This can be done either automatically via script, or manually.

### Automatic
```
cd setup
./setup.sh
```
This will install postgres whether on Mac, Linux, or Windows, and then create the database, user, and grant them permissions.

### Manually
1. Install postgres
2. ```psql postgres```
3. ```CREATE DATABASE txdb;```
4. ```CREATE USER tx_admin WITH PASSWORD 'SCARA123!';```
5. ```GRANT ALL PRIVILEGES ON DATABASE txdb TO txadmin;```

## Running backend
1. ```dotnet tool install --global dotnet-ef --version 8.0.11```
1. ```cd src/ShelfLayoutManager.Web```
3. ```dotnet ef database update```
4. ```dotnet run --launch-profile http```
5. Navigate to ```https://localhost:7235/swagger```

## Ingesting data
In order to have a good test user experience in the frontend, you should first ingest data into the backend! 
You can do this via the swagger page.

1. Navigate to ```https://localhost:7235/swagger```
2. Find the ```v1/utils/ingest-sku-file``` endpoint
3. Open the ```sku.json``` file on your disk, and hit the ```Execute``` button
4. Find the ```v1/utils/ingest-shelf-file``` endpoint
5. Open the ```shelf.json``` file on your disk, and hit the ```Execute``` button

## Running frontend
1. ```cd src/ShelfLayoutManager.Client```
2. ```dotnet watch --launch-profile https```
3. Navigate to ```https://localhost:7152```

## Testing
From project root directory simply ```dotnet test```