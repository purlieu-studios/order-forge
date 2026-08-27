param(
      [Parameter(Mandatory)]
      [ValidateSet("Orders", "Inventory", "Payments")]
      [string]$Service
  )

  $project = ".\src\backend\OrderForge.$Service\OrderForge.$Service.csproj"
  $context = "${Service}DbContext"

  dotnet tool run dotnet-ef database update `
      --project $project `
      --startup-project $project `
      --context $context

  exit $LASTEXITCODE