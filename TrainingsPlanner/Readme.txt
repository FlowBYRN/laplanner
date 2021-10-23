Ef Core Tools

Create Migration:

dotnet ef migrations add Initial -o Infrastructure/Migrations/Training --context TrainingDbContext
dotnet ef migrations add Initial -o Infrastructure/Migrations --context IdentityDbContext

Update Database:

dotnet ef database update --context TrainingDbContext
dotnet ef database update --context IdentityDbContext

Drop Database:

dotnet ef database drop --context TrainingDbContext
dotnet ef database drop --context IdentityDbContext