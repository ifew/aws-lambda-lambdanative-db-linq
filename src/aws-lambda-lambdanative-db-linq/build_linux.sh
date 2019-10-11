#!/bin/bash

#
# SHOULD BUILD IMAGE BEFORE
# docker build -t lambdanative:test .
#

rm -f $(pwd)/bootstrap
rm -f $(pwd)/package.zip
docker run --rm -v $(pwd):/app lambdareadytorun:3.0
cp bin/Release/netcoreapp3.0/linux-x64/native/aws-lambda-lambdanative-db-linq bootstrap
zip package.zip bootstrap
#aws s3 cp package.zip s3://backend-layer