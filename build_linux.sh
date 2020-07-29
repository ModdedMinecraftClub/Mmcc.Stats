#!/bin/bash
dotnet publish ./src -r linux-x64 -c Release /p:PublishSingleFile=true --output ./out