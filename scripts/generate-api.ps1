$repoRoot = Split-Path -Parent $PSScriptRoot
$gatewayProject = Join-Path $repoRoot 'src\backend\OrderForge.Gateway'
$frontendPath = Join-Path $repoRoot 'src\frontend'
$openApiUrl = 'http://localhost:5113/openapi/v1.json'
$stdoutLog = (New-TemporaryFile).FullName
$stderrLog = (New-TemporaryFile).FullName
$gatewayProcess = $null

try
{
    $gatewayProcess = Start-Process `
        -FilePath 'dotnet' `
        -ArgumentList @('run', '--project', $gatewayProject, '--launch-profile', 'http') `
        -WorkingDirectory $repoRoot `
        -RedirectStandardOutput $stdoutLog `
        -RedirectStandardError $stderrLog `
        -PassThru

    $ready = $false

    for ($attempt = 0; $attempt -lt 60; $attempt++)
    {
        if ($gatewayProcess.HasExited)
        {
            $errorOutput = Get-Content -LiteralPath $stderrLog -Raw -ErrorAction SilentlyContinue
            throw "Gateway exited before OpenAPI was available. $errorOutput"
        }

        try
        {
            $response = Invoke-WebRequest -Uri $openApiUrl -UseBasicParsing -TimeoutSec 2

            if ($response.StatusCode -eq 200)
            {
                $ready = $true
                break
            }
        }
        catch
        {
        }

        Start-Sleep -Milliseconds 500
    }

    if (-not $ready)
    {
        $errorOutput = Get-Content -LiteralPath $stderrLog -Raw -ErrorAction SilentlyContinue
        throw "Timed out waiting for the Gateway OpenAPI endpoint. $errorOutput"
    }

    & npm run generate:api --prefix $frontendPath

    if ($LASTEXITCODE -ne 0)
    {
        throw "Hey API generation failed with exit code $LASTEXITCODE."
    }
}
finally
{
    if ($null -ne $gatewayProcess -and -not $gatewayProcess.HasExited)
    {
        & taskkill.exe /PID $gatewayProcess.Id /T /F *> $null
    }

    Remove-Item -LiteralPath $stdoutLog, $stderrLog -Force -ErrorAction SilentlyContinue
}
