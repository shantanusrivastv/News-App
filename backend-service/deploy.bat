REM Creating and pushing image in docker
dotnet publish --framework net8.0 --os linux  -p:PublishProfile=DefaultContainer
REM Deploying to k8
REM kubectl apply -f deployment.yaml