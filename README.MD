# Example AWS Lambda C# .NET Core 3 (and 2.2) Runtime (CoreRT) with connect MySQL via not using any ORM

Example for optimize cold start with Lambda Native, I'm using CoreRT and implement based my project [corert-db](https://github.com/ifew/corert-db)


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