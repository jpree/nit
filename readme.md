[![Build Status](https://travis-ci.com/jpree/nit.svg?branch=master)](https://travis-ci.com/jpree/nit) ![GitHub](https://img.shields.io/github/license/jpree/nit.svg)

# Nit Help

Nit is a git flavored tool for content management.

## Use Cases

User would like to quickly and easily store notes in a way that allows them to be retrieved quickly and easily.

## Install from Nuget as a global tool
```shell
$ dotnet tool install -g nit --version 1.0.0-alpha7
```
### Store simple notes or messages associated with tags

Add a message (-m) with tags (-t). Note that tags are not case sensitive and a space separates each tag:
```shell
$ nit add -t "Invictus W.E. Henley Poetry" -m "I am the master of my fate: I am the captain of my soul."
$ nit add -t "Magic Mountain Czeslaw Milosz Poetry" -m "Even asleep we partake in the becoming of the world."
```
### Flexibly retreive content based on tags

Get all objects tagged "Poetry":
```shell
$ nit object -t "Poetry"
I am the master of my fate: I am the captain of my soul.
Even asleep we partake in the becoming of the world.
```
Get all objects tagged "Henley":
```shell
$ nit object -t "Henley"
I am the master of my fate: I am the captain of my soul.
```
Get all objects tagged Henley or Poetry, ordered by most tags matched:
```shell
$ nit object -t "Henley Poetry"
I am the master of my fate: I am the captain of my soul.
Even asleep we partake in the becoming of the world.
```
