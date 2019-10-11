# Example AWS Lambda Native using .NET Core 3 with connect MySQL via Linq2DB

IT'S NOT WORK


### Build file and packing to output/package.zip

```bash
sh build_linux.sh
```

### Upload to aws s3

```bash
aws s3 cp package.zip s3://backend-layer [--profile AWS-PROFILE-NAME]
```


## Note

Can't test via lambda docker on localhot