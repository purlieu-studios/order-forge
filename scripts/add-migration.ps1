param(
      [Parameter(Mandatory)]
      [ValidateSet("Orders", "Inventory", "Payments")]
      [string]$Service,

      [Parameter(Mandatory)]
      [string]$Name
  )

  $project = ".\src\backend\OrderForge.$Service\OrderForge.$Service.csproj"
  $context = "${Service}DbContext"

  dotnet tool run dotnet-ef migrations add $Name `
      --project $project `
      --startup-project $project `
      --context $context `
      --output-dir Infrastructure\Persistence\Migrations

  exit $LASTEXITCODE