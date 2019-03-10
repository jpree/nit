# Notes

## Pull from nuget.org

$ dotnet tool install -g nit --version 1.0.0-alpha1
$ dotnet tool uninstall nit -g

## Tag and deploy

$ git tag alpha2
$ git push origin alpha2

## Local Testing

$ dotnet pack -c release --version-suffix alphaz
$ dotnet tool install -g nit --add-source .\nupkg\ --version 1.0.0-alphaz
