# shelf-layout-manager

Code solution for both the Backend assignment and Frontend assignment from Telexistence.

## Live Demo 🚀
https://anzor-tx-fjahdve4h9a6bfbj.japanwest-01.azurewebsites.net

<p align="center">
  <img src = "https://i.imgur.com/jELGmdX.png" width="864">
</p>

### API
https://tx-assignment-api-hwhkhye3d5f5a3g2.japanwest-01.azurewebsites.net/swagger/index.html

<p align="center">
  <img src = "https://i.imgur.com/EDmRN2y.png" width="864">
</p>

## Tech Stack and Infrastructure
### Backend
- .NET 8
- ASP.NET Core
- EF Core 8

### Frontend
- Blazor WebAssembly App

### Deployment
- Azure
- Azure Cosmos DB for PostgreSQL

## Local Development
### Setup
```
cd setup
./setup.sh
```

***Or*** – manually:

#### Setup DB
Manually install postgres, then
```
psql postgres
CREATE DATABASE txdb;
CREATE USER tx_admin WITH PASSWORD 'SCARA123!';
GRANT ALL PRIVILEGES ON DATABASE txdb TO txadmin;
```

#### Apply initial DB migration
```
dotnet tool install --global dotnet-ef --version 8.0.11
cd src/ShelfLayoutManager.Web
dotnet ef database update
```

----

### Running

#### Backend
```
cd src/ShelfLayoutManager.Web
dotnet run
```
⮕ Navigate to ```https://localhost:7235/swagger```

#### Frontend
```
cd src/ShelfLayoutManager.Client
dotnet watch
```
⮕ Navigate to ```https://localhost:7152```

----

### Testing
```dotnet test```

----

### Date Ingestion
1. Navigate to ```https://localhost:7235/swagger```
2. Use the ```v1/utils/ingest-sku-file``` endpoint to upload a `sku.json` file
3. Use the ```v1/utils/ingest-shelf-file``` endpoint to upload a `shelf.json` file

## Technical Discussion
----

**Overall** – the paramount goal of this code design and implementation was **simplification** via **clean** and **obvious** 
code.

----

- Why PostgreSQL (instead of MongoDB, etc..)?
  - Central domain of assignment is "products" (SKUs) and things related to them and relationial DBs are excellent for 
    this. Products inevitably become related to inventory and inventory management, which again relation DBs are 
    excellent for.
  - Given I don't know the concerete specifics about use cases and scale, I don't feel super strongly about this choice
    – nosql vs relational either would have been fine.
- Why REST (instead of gRPC)?
  - Main client of this backend was a simple webapp. gRPC is great for backend<->backend communication and some bespoke
    scenarios for backend<->frontend. However given these requirements of a simple backend and simple frontend, REST was
    the simplest option.

## Time Spent
|Task|Time (hours)|
|------|------------|
| Reading assignments; clarifying questions | 1 |
| Designing backend | 1 |
| Backend impl | 3 - 5 |
| Learning Blazor | 1 - 3 |
| Designing Frontend | 1 |
| Frontend impl | 4 |
| Frontend theming | 1 |
| Learning/deploying to Azure | 7 - 10 |
| Writing README | 1 |
| Total | 20 - 27 |

## Backlog

### Backend
- Add authentication/authorization
- Add more unit test coverage
- Add integration tests
- Integrate logging (for eg with Serilog)

### Frontend
- UX for pending HTTP requests
- UX for failing HTTP requests
- Websockets for realtime updates
- Drag and drop to move drinks
- 3D visualization of store
- More robust routing
- Don't use Blazor

### Infra
- Containerize environment
- Setup Github actions