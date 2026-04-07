<#
PowerShell script to synchronize branches after feature work.
This script will fetch latest from origin and attempt to sync main with Rama1 and Rama2.
Usage: run from repo root with PowerShell, ensure you have permissions to push.
#>
param(
    [string]$Remote = "origin",
    [string[]]$BranchesToSync = @("Rama1", "Rama2"),
    [string]$BaseBranch = "main"
)

Write-Host "Fetching latest from $Remote..." -ForegroundColor Cyan
git fetch $Remote --prune

foreach ($b in $BranchesToSync) {
    Write-Host "Switching to $b" -ForegroundColor Yellow
    git checkout $b
    if ($LASTEXITCODE -ne 0) { Write-Error "Failed to checkout $b"; exit 1 }
    Write-Host "Merging $BaseBranch into $b" -ForegroundColor Cyan
    git merge $Remote/$BaseBranch --no-ff -m "Sync: merge $BaseBranch into $b"
    if ($LASTEXITCODE -ne 0) { Write-Error "Merge failed for $b"; exit 1 }
    Write-Host "Pushing $b to $Remote" -ForegroundColor Green
    git push -u $Remote $b
    if ($LASTEXITCODE -ne 0) { Write-Error "Push failed for $b"; exit 1 }
}

Write-Host "Branch sync complete." -ForegroundColor Green
