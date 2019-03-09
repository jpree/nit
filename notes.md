# Notes

## Create tool package

From repo root:
$ dotnet pack
$ dotnet tool install -g nit --add-source .\nupkg\
$ dotnet tool uninstall -g nit

## Pull from nuget.org

dotnet tool install -g nit --version 1.0.0-alpha1

## Just push tag

$ git push origin alpha2