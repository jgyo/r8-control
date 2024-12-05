# uptag.ps1
# Run this to enable: Set-ExecutionPolicy -ExecutionPolicy Unrestricted -scope CurrentUser
$tag = $args[0]
Write-Host "Setting tag v$tag named Release $tag"
$ok = Read-Host "Ok?"
git tag -a v$tag -m "Release $tag"
git push origin v$tag